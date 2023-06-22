using System;

namespace CodeBase.Data
{
  [Serializable]
  public class LootPieceData
  {
    public Vector3Data Position;
    public Loot Loot;

    public LootPieceData(Vector3Data position, Loot loot)
    {
      Position = position;
      Loot = loot;
    }
  }
}