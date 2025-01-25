using System;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Player
{
    public class PlayerCameraControl : MonoBehaviour
    {
        public float moveDuration = 0.3f; // 카메라 움직임에 걸리는 시간
        public Transform[] moveTransform;

        private StatusValue<int> _moveTransformCount = new(0, 0, 2);
        private Vector3 _movePosition;
        private Tween _moveTween;

        public void Awake()
        {
            InputManager.CameraMoveRight.performed += context =>
            {
                _moveTransformCount.Current++;
                Move(moveTransform[_moveTransformCount.Current].position);
            };
            InputManager.CameraMoveLeft.performed += context =>
            {
                _moveTransformCount.Current--;
                Move(moveTransform[_moveTransformCount.Current].position);
            };
        }

        public void Move(Vector3 movePosition)
        {
            if (!ReferenceEquals(_moveTween, null))
            {
                _moveTween.Pause();
                _moveTween = null;
            }
             
            _movePosition = movePosition;
            _moveTween = Camera.main.gameObject.transform.DOMove(_movePosition, moveDuration).SetEase(Ease.OutExpo);
        }
    }
}