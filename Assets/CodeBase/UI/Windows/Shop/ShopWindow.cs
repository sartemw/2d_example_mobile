using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
  public class ShopWindow : WindowBase
  {
    public TextMeshProUGUI SkullText;
    public RewardedAdItem AdItem;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      base.Construct(progressService);
      AdItem.Construct(adsService, progressService);
    }
    
    protected override void Initialize()
    {
      AdItem.Initialize();
      RefreshSkullText();
    }

    protected override void SubscribeUpdates()
    {
      AdItem.Subscribe();
      Progress.WorldData.LootData.Changed += RefreshSkullText;
    }

    protected override void Cleanup()
    {
      base.Cleanup();
      AdItem.Cleanup();
      Progress.WorldData.LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() => 
      SkullText.text = Progress.WorldData.LootData.Collected.ToString();
  }
}