using System;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class BackToStartPosition: MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D;
        public Follow Follow;
        public bool IsPlatformer;
        public float MinimalDistance = 1;

        private Vector2 _startPoint;


        private void Start() => 
            Invoke(nameof(SaveStartPoint), 1f);

        private void Update()
        {
            if (Follow.enabled != false)
                return;

            if (IsStartPointNotReached() && _startPoint != Vector2.zero)
            {
                MoveToTarget(_startPoint);    
            }
            else
            {
                ReachedToStartPoint();
            }
        }

        private void MoveToTarget(Vector2 target)
        {
            Follow.Velocity = 1;
            Vector2 directionAtTarget = transform.position.DirectionAtTarget(target).BlockYAxis();
            
            if (IsPlatformer)
                directionAtTarget.BlockYAxis();
            
            transform.right = directionAtTarget;
            Rigidbody2D.MovePosition(Follow.Speed * directionAtTarget * Time.deltaTime  + Rigidbody2D.position);
        }

        private bool IsStartPointNotReached()
        {
            var remainingDistance = gameObject.transform.position.SqrMagnitudeTo(_startPoint);
            return remainingDistance >= MinimalDistance;
        }

        private void SaveStartPoint() => 
            _startPoint = transform.position;

        private void ReachedToStartPoint()
        {
            Follow.Velocity = 0;
        }
    }
}