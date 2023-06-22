using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class EnemyHealth : MonoBehaviour, IHealth
  {
    public EnemyAnimator Animator;

    private float _current;

    [SerializeField]
    private float _max;

    public event Action HealthChanged;

    public float Current
    {
      get => _current;
      set => _current = value;
    }

    public float Max
    {
      get => _max;
      set => _max = value;
    }

    private void Start()
    {
      _current = _max;
    }

    public void TakeDamage(float damage)
    {
      Current -= damage;
      
      Animator.PlayHit();
      
      HealthChanged?.Invoke();
    }

  }
}