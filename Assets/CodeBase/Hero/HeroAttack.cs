using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroAnimator), typeof(Collider2D))]
  public class HeroAttack : MonoBehaviour, ISavedProgressReader
  {
    public HeroAnimator Animator;
    public Collider2D HeroCollider;
    public float AttackDistance;

    public IInputService InputService;

    private static int _layerMask;
    private Collider2D[] _hits = new Collider2D[3];
    private Stats _stats;


    private void Awake() => 
      _layerMask = 1 << LayerMask.NameToLayer("Hittable");

    private void Update()
    {
      if (InputService == null)
        return;
      
      if(InputService.IsAttackButtonUp() && !Animator.IsAttacking)
        Animator.PlayAttack();
    }

    private void OnAttack()
    {
      PhysicsDebug.DrawDebug(StartPoint() + (transform.right * AttackDistance), _stats.DamageRadius, 1.0f);
      for (int i = 0; i < Hit(); ++i)
      {
        _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
      }
    }

    private int Hit() => 
      Physics2D.OverlapCircleNonAlloc(StartPoint() + (transform.right * AttackDistance) , _stats.DamageRadius, _hits, _layerMask);

    private Vector3 StartPoint() =>
      new Vector3(GetCenter(HeroCollider).x,transform.position.y, transform.position.z);

    public void LoadProgress(PlayerProgress progress) => 
      _stats = progress.HeroStats;

    private Vector2 GetCenter(Collider2D heroCollider) =>
      new Vector2(
        heroCollider.transform.position.x + heroCollider.offset.x,
        heroCollider.transform.position.y + heroCollider.offset.y);
  }
}