using System;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(Attack))]
  public class CheckAttackRange : MonoBehaviour
  {
    public Attack Attack;
    public TriggerObserver TriggerObserver;

    private void Start()
    {
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;
      
      Attack.DisableAttack();
    }

    private void OnDisable()
    {
      TriggerObserver.TriggerEnter -= TriggerEnter;
      TriggerObserver.TriggerExit -= TriggerExit;
    }

    private void TriggerExit(Collider2D obj)
    {
      Attack.DisableAttack();
    }

    private void TriggerEnter(Collider2D obj)
    {
      Attack.EnableAttack();
    }
  }
}