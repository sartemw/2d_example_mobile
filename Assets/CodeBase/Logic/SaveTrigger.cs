using CodeBase.Services;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
  public class SaveTrigger : MonoBehaviour
  {
    private ISaveLoadService _saveLoadService;

    [Inject]
    public void Construct(ISaveLoadService saveLoadService)
    {
      _saveLoadService = saveLoadService;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      _saveLoadService.SaveProgress();
      Debug.Log("Progress saved!");
      gameObject.SetActive(false);
    }

  }
}