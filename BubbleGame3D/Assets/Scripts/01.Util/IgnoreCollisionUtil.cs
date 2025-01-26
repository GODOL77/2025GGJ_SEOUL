using System;
using UnityEngine;

namespace Util
{
    public class IgnoreCollisionUtil : MonoBehaviour
    {
        public Collider[] targetIgnoreColliders;

        public void Awake()
        {
            var c = GetComponent<Collider>();
            Debug.Assert(c != null, "콜라이더가 존재하지 않습니다.");

            foreach (var targetIgnoreCollider in targetIgnoreColliders)
            {
                Physics.IgnoreCollision(targetIgnoreCollider, c, true);
            }
        }
    }
}