using System;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using UnityEngine;


namespace CodeBase.Enemy
{
  [RequireComponent(typeof(Rigidbody2D))]
  public class AgentMoveToPlayer : Follow
  {
    public Rigidbody2D Rigidbody2D;
    public float MinimalDistance = 6;
    public float RemainingDistance => _remainingDistance;
    public bool IsPlatformer;
    
    private float _remainingDistance;
    private IGameFactory _gameFactory;
    private Transform _heroTransform;

    public void Construct(Transform heroTransform, float speed)
    {
      Speed = speed;
      _heroTransform = heroTransform;
    }
    private void Update()
    {
      if (_heroTransform && IsHeroNotReached())
        MoveToTarget(_heroTransform.position);
      else
        ReachedToHero();
    }

    private void ReachedToHero()
    {
      Velocity = 0;
    }

    public override void MoveToTarget(Vector2 target)
    {
      Velocity = 1;
      Vector2 directionAtTarget = transform.position.DirectionAtTarget(target);

      if (IsPlatformer)
        directionAtTarget = directionAtTarget.BlockYAxis();
      
      transform.right = directionAtTarget;
      Rigidbody2D.MovePosition(Speed * directionAtTarget * Time.deltaTime  + Rigidbody2D.position);
    }

    private bool IsHeroNotReached()
    {
      _remainingDistance = gameObject.transform.position.SqrMagnitudeTo(_heroTransform.position);
      return _remainingDistance >= MinimalDistance;
    }
    
  }
}