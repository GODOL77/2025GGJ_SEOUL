using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Util;

namespace Player
{
    public class PlayerCameraControl : MonoBehaviour
    {
        public float moveDuration = 0.3f; // 카메라 움직임에 걸리는 시간
        public Transform[] moveTransform;
        public Button leftArrowButton;
        public Button rightArrowButton;
        
        private StatusValue<int> _moveTransformCount = new(1, 0, 2);
        private Vector3 _movePosition;
        private Tween _moveTween;

        public void Awake()
        {
            InputManager.CameraMoveRight.performed += MoveRight;
            InputManager.CameraMoveLeft.performed += MoveLeft;

            InputAction.CallbackContext c = new();
            leftArrowButton.onClick.AddListener(() =>MoveLeft(c));
            rightArrowButton.onClick.AddListener(() => MoveRight(c));
        }

        public void OnDestroy()
        {
            if (!InputManager.HasInstance) return;
            InputManager.CameraMoveRight.performed -= MoveRight;
            InputManager.CameraMoveLeft.performed -= MoveLeft;
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

        private void MoveLeft(InputAction.CallbackContext context)
        {
            _moveTransformCount.Current--;
            Move(moveTransform[_moveTransformCount.Current].position);
            leftArrowButton.gameObject.SetActive(_moveTransformCount.Current != 0);
            rightArrowButton.gameObject.SetActive(_moveTransformCount.Current != 2);
        }

        private void MoveRight(InputAction.CallbackContext context)
        {
            _moveTransformCount.Current++;
            Move(moveTransform[_moveTransformCount.Current].position);
            leftArrowButton.gameObject.SetActive(_moveTransformCount.Current != 0);
            rightArrowButton.gameObject.SetActive(_moveTransformCount.Current != 2);
        }
    }
}