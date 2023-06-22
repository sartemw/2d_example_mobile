using System;

namespace CodeBase.Data
{
  [Serializable]
  public class LootData
  {
    public int Collected;
    public LootPieceDataDictionary LootPiecesOnScene = new LootPieceDataDictionary();
    
    public Action Changed;

    public void Collect(Loot loot)
    {
      Collected += loot.Value;
      Changed?.Invoke();
    }

    public void Add(int lootValue)
    {
      Collected += lootValue;
      Changed?.Invoke();
    }
  }
}