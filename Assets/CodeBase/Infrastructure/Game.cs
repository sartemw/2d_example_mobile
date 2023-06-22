using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain, DiContainer container)
    {
      StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, container);
      container.Resolve<IGameFactory>().StateMachine = StateMachine;
    }
  }
}