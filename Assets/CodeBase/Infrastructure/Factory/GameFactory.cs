using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public IGameStateMachine StateMachine { get; set; }
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    
    private readonly DiContainer _diContainer;
    private IAssetProvider _assets;
    private IStaticDataService _staticData;
    private IRandomService _randomService;
    private IPersistentProgressService _persistentProgressService;
    private GameObject _heroGameObject;
    private IWindowService _windowService;
    private IInputService _inputService;
    public GameFactory(DiContainer diContainer)
    {
      _diContainer = diContainer;

      Construct();
    }
    public void Construct()
    {
      _windowService = _diContainer.Resolve<IWindowService>();
      _inputService = _diContainer.Resolve<IInputService>();
      _staticData = _diContainer.Resolve<IStaticDataService>();
      _assets = _diContainer.Resolve<IAssetProvider>();
      _randomService = _diContainer.Resolve<IRandomService>();
      _persistentProgressService = _diContainer.Resolve<IPersistentProgressService>();
    }

    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
      _heroGameObject = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);
      
      _heroGameObject.GetComponent<HeroAttack>().InputService = _inputService;
      _heroGameObject.GetComponent<HeroMove2D>().InputService = _inputService;

      return _heroGameObject;
    }

    public async Task CreateLevelTransfer(Vector3 at)
    {
      GameObject prefab = await InstantiateRegisteredAsync(AssetAddress.LevelTransferTrigger, at);
      LevelTransferTrigger levelTransfer = prefab.GetComponent<LevelTransferTrigger>();
      
      levelTransfer.Construct(StateMachine);
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
      
      hud.GetComponentInChildren<LootCounter>()
        .Construct(_persistentProgressService.Progress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
        openWindowButton.Init(_windowService);

      return hud;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      
      LootPiece lootPiece = InstantiateRegistered(prefab)
        .GetComponent<LootPiece>();
      
      lootPiece.Construct(_persistentProgressService.Progress.WorldData);

      return lootPiece;
    }

    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(_heroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<AgentMoveToPlayer>()?.Construct(_heroGameObject.transform, monsterData.MoveSpeed);
      monster.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);

      LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.Construct(this, _randomService);
      lootSpawner.SetLootValue(monsterData.MinLootValue, monsterData.MaxLootValue);

      return monster;
    }

    public async Task CreateSpawner(string spawnerId, Vector3 at, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
      
      SpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<SpawnPoint>();
      
      spawner.Construct(this);
      spawner.MonsterTypeId = monsterTypeId;
      spawner.Id = spawnerId;
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      
      _assets.Cleanup();
    }
    
    private GameObject InstantiateRegistered(GameObject prefab, Vector2 at)
    {
      GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }
    
    private GameObject InstantiateRegistered(GameObject prefab)
    {
      GameObject gameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector2 at)
    {
      GameObject gameObject = await _assets.Instantiate(path: prefabPath, at: at);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject gameObject = await _assets.Instantiate(path: prefabPath);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }
  }
}