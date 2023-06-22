using UnityEngine;

namespace CodeBase.Data
{
  public static class DataExtensions
  {
    public static Vector3Data AsVectorData(this Vector3 vector) => 
      new Vector3Data(vector.x, vector.y, vector.z);
    
    public static Vector3 AsUnityVector(this Vector3Data vector3Data) => 
      new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

    public static Vector3 AddY(this Vector3 vector, float y)
    {
      vector.y = y;
      return vector;
    }

    public static float SqrMagnitudeTo(this Vector3 from, Vector3 to)
    {
      return Vector3.SqrMagnitude(to - from);
    }

    public static string ToJson(this object obj) => 
      JsonUtility.ToJson(obj);

    public static T ToDeserialized<T>(this string json) =>
      JsonUtility.FromJson<T>(json);
    
    public static Vector2 DirectionAtTarget(this Vector3 from, Vector2 to)
    {
      Vector2 directionToPlayer = new Vector2 (
          to.x - from.x,
          to.y - from.y)
        .normalized;
      return directionToPlayer;
    }
    
    public static Vector2 BlockYAxis(this Vector2 direction) => 
      new Vector2(direction.x, direction.y = 0);
  }
}