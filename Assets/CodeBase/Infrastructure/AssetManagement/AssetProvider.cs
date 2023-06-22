using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    private readonly Dictionary<string, AsyncOperationHandle> _completedCashe = new Dictionary<string, AsyncOperationHandle>();
    private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

    public void Initialize()
    {
      Addressables.InitializeAsync();
    }
    
    public async Task<T> Load<T>(AssetReference assetReference) where T : class
    {
      if (_completedCashe.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
        return completedHandle.Result as T;

      return await RunWithCacheOnComplete(
        Addressables.LoadAssetAsync<T>(assetReference), 
        cacheKey: assetReference.AssetGUID);
    }

    public async Task<T> Load<T>(string address) where T : class
    {
      if (_completedCashe.TryGetValue(address, out AsyncOperationHandle completedHandle))
        return completedHandle.Result as T;
      
      AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);

      return await RunWithCacheOnComplete(
        Addressables.LoadAssetAsync<T>(address), 
        cacheKey: address);
    }

    public Task<GameObject> Instantiate(string address, Vector3 at) => 
      Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

    public Task<GameObject> Instantiate(string address) => 
      Addressables.InstantiateAsync(address).Task;

    public void Cleanup()
    {
      foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
      foreach (AsyncOperationHandle handle in resourceHandles)
        Addressables.Release(handle);
      
      _completedCashe.Clear();
      _handles.Clear();
    }

    private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
    {
      handle.Completed += completeHandle =>
      {
        _completedCashe[cacheKey] = completeHandle;
      };

      AddHandle<T>(cacheKey, handle);

      return await handle.Task;
    }

    private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
    {
      if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
      {
        resourceHandles = new List<AsyncOperationHandle>();
        _handles[key] = resourceHandles;
      }

      resourceHandles.Add(handle);
    }
  }
}