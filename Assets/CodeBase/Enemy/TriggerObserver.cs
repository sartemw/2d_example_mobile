using System;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(Collider2D))]
  public class TriggerObserver : MonoBehaviour
  {
    public event Action<Collider2D> TriggerEnter;
    public event Action<Collider2D> TriggerExit;
    
    private void OnTriggerEnter2D(Collider2D col) => 
      TriggerEnter?.Invoke(col);

    private void OnTriggerExit2D(Collider2D other) => 
      TriggerExit?.Invoke(other);
  }
}