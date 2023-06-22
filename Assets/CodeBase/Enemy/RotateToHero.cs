using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class RotateToHero : Follow
  {
    private Transform _heroTransform;
    private Vector3 _positionToLook;

    public void Construct(Transform heroTransform)
      => _heroTransform = heroTransform;
    
    private void Update()
    {
      if (_heroTransform)
        RotateTowardsHero();
    }

    private void RotateTowardsHero()
    {
      UpdatePositionToLookAt();

      transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }

    private void UpdatePositionToLookAt()
    {
      Vector3 positionDelta = _heroTransform.position - transform.position;
      _positionToLook = new Vector3(positionDelta.x, transform.position.y, positionDelta.z);
    }
    
    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
      Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

    private Quaternion TargetRotation(Vector3 position) =>
      Quaternion.LookRotation(position);

    private float SpeedFactor() =>
      Speed * Time.deltaTime;

    public override void MoveToTarget(Vector2 position)
    {
      
    }
  }
}