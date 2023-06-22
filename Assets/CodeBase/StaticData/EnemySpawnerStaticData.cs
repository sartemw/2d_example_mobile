using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class EnemySpawnerStaticData
  {
    public string Id;
    public MonsterTypeId MonsterTypeId;
    public Vector3 Position;

    public EnemySpawnerStaticData(string id, MonsterTypeId monsterTypeId, Vector3 position)
    {
      Id = id;
      MonsterTypeId = monsterTypeId;
      Position = position;
    }
  }
}