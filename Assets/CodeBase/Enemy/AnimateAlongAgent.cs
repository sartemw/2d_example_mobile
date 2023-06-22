using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class AnimateAlongAgent : MonoBehaviour
  {
    public AgentMoveToPlayer Agent;
    private const float MinimalVelocity = 0.1f;
    
    [SerializeField] private EnemyAnimator Animator;

    private void Update()
    {
      if (ShouldMove())
        Animator.Move(Agent.Velocity * Agent.Speed);
      else
      {
        Animator.StopMoving();
      }
    }

    private bool ShouldMove()
    {
      //Debug.Log("Agent.Velocity " + Agent.Velocity + "||| Agent.RemainingDistance " + Agent.RemainingDistance);
      
      return Agent.Velocity > MinimalVelocity && Agent.RemainingDistance > Agent.MinimalDistance;
    }
  }
}