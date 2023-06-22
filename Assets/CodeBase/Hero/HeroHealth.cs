using System;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
  public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
  {
    public HeroAnimator Animator;
    
    private State _state;

    public event Action HealthChanged;

    public float Current
    {
      get => _state.CurrentHP;
      set
      {
        if (value != _state.CurrentHP)
        {
          _state.CurrentHP = value;
          
          HealthChanged?.Invoke();
        }
      }
    }

    public float Max
    {
      get => _state.MaxHP;
      set => _state.MaxHP = value;
    }


    public void LoadProgress(PlayerProgress progress)
    {
      _state = progress.HeroState;
      HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.HeroState.CurrentHP = Current;
      progress.HeroState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
      if(Current <= 0)
        return;
      
      Current -= damage;
      Animator.PlayHit();
    }
  }
}