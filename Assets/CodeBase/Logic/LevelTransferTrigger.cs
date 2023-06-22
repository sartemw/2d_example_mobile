using System;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private const string PlayerTag = "Player";
    public string TransferTo;
    private IGameStateMachine _stateMachine;
    private bool _triggered;

    public void Construct(IGameStateMachine stateMachine) => 
      _stateMachine = stateMachine;

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(_triggered)
        return;

      if (other.CompareTag(PlayerTag))
      {
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
      }
    }
  }
}