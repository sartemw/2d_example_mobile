using CodeBase.Infrastructure;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.Ads;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

public class BootstrappInstaller : MonoInstaller, IInitializable
{
    public LoadingCurtain CurtainPrefab;
    public GameObject Bootsprapper;

    private GameObject _bootstrapper;
    public override void InstallBindings()
    {
        BindThis();

        BindStaticData();
        BindWindowService();
        BindAdsService();
        BindAssetProvider();
        BindInputService();
        BindRandomService();
        BindProgressService();
        BindUIFactory();
        BindGameFactory();
        BindSaveLoadService();
    }

    private void BindAdsService()
    {
        IAdsService adsService = new AdsService();
        adsService.Initialize();
        Container.Bind<IAdsService>().FromInstance(adsService).AsSingle();
    }

    private void BindSaveLoadService()
    {
        Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
    }

    private void BindThis()
    {
        Container.BindInterfacesTo<BootstrappInstaller>().FromInstance(this).AsSingle();
    }

    private void BindUIFactory() => 
        Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();

    private void BindWindowService() => 
        Container.Bind<IWindowService>().To<WindowService>().AsSingle();

    private void BindProgressService() => 
        Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();

    private void BindRandomService() => 
        Container.Bind<IRandomService>().To<RandomService>().AsSingle();

    private void BindAssetProvider()
    {
        IAssetProvider assetProvider = new AssetProvider();
        assetProvider.Initialize();
        Container.Bind<IAssetProvider>().FromInstance(assetProvider).AsSingle();
    }

    private void BindGameFactory()
    {
        IGameFactory gameFactory = new GameFactory(Container);
        Container
            .Bind<IGameFactory>()
            .FromInstance(gameFactory)
            .AsSingle();
    }

    private void BindStaticData()
    {
        var staticData = new StaticDataService();
        staticData.Load();
        Container
            .Bind<IStaticDataService>()
            .FromInstance(staticData)
            .AsSingle();
    }

    private void BindInputService()
    {
        var inputService = ChangeInputService();
        
        Container
            .Bind<IInputService>()
            .FromInstance(inputService)
            .AsSingle();
    }
    private static IInputService ChangeInputService() =>
        Application.isEditor
            ? (IInputService) new StandaloneInputService()
            : new MobileInputService();

    public void Initialize()
    {
        if (!_bootstrapper)
        {
            _bootstrapper = Container.InstantiatePrefab(Bootsprapper);
            var gameBootstrapper = _bootstrapper.GetComponent<GameBootstrapper>();
            gameBootstrapper.Init(CurtainPrefab, Container);
        }
    }
}