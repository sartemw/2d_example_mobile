using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
  public class AdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
  {
    private const string AndroidGameId = "5109329";
    private const string IOSGameId = "5109328";

    private const string UnityRewardedVideoIdAndroid = "Rewarded_Android";
    private const string UnityRewardedVideoIdIOS = "Rewarded_iOS";

    private string _gameId;
    private string _placementId;
    private bool _testMode = true;
    
    private Action _onVideoFinished;

    public event Action RewardedVideoReady;
    public bool _videoIsReady;

    public int Reward
    {
      get { return 15; }
    }

    public void Initialize()
    {
      SetIdsForCurrentPlatform();
      Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void ShowRewardedVideo(Action onVideoFinished)
    {
      _onVideoFinished = onVideoFinished;
      Advertisement.Show(_placementId, this);
    }
    public void LoadAd()
    {
      Debug.Log("Loading Ad: " + _placementId);
      Advertisement.Load(_placementId, this);
      _videoIsReady = true;
    }
    bool IAdsService.IsRewardedVideoReady => 
      _videoIsReady;

    private void SetIdsForCurrentPlatform()
    {
      switch (Application.platform)
      {
        case RuntimePlatform.Android:
          _gameId = AndroidGameId;
          _placementId = UnityRewardedVideoIdAndroid;
          break;

        case RuntimePlatform.IPhonePlayer:
          _gameId = IOSGameId;
          _placementId = UnityRewardedVideoIdIOS;
          break;

        case RuntimePlatform.WindowsEditor:
          _gameId = IOSGameId;
          _placementId = UnityRewardedVideoIdIOS;
          break;

        default:
          Debug.Log("Unsupported platform for ads.");
          break;
      }
    }

    public void OnInitializationComplete()
    {
      Debug.Log("OnInitializationComplete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
      Debug.Log($"OnInitializationFailed {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
      Debug.Log($"OnUnityAdsAdLoaded {placementId}");
      if (placementId == _placementId)
        RewardedVideoReady?.Invoke();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
      Debug.Log($"OnUnityAdsFailedToLoad {_gameId} - {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
      Debug.Log($"OnUnityAdsShowFailure {_gameId} - {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
      _videoIsReady = false;
      Debug.Log($"OnUnityAdsShowStart {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
      Debug.Log($"OnUnityAdsShowClick {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
      switch (showCompletionState)
      {
        case UnityAdsShowCompletionState.SKIPPED:
          Debug.LogError($"OnUnityAdsDidFinish {showCompletionState} | SKIPPED");
          break;
        case UnityAdsShowCompletionState.COMPLETED:
          _onVideoFinished?.Invoke();
          break;
        case UnityAdsShowCompletionState.UNKNOWN:
          Debug.LogError($"OnUnityAdsDidFinish {showCompletionState} | UNKNOWN");
          break;
        default:
          Debug.LogError($"OnUnityAdsDidFinish {showCompletionState} | default");
          break;
      }
      LoadAd();
      _onVideoFinished = null;
      
    }
  }
}