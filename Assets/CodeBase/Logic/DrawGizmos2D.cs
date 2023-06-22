using System;
using UnityEngine;

namespace CodeBase.Logic
{
  public class DrawGizmos2D : MonoBehaviour
  {
    public BoxCollider2D Collider;

    public ColorsGizmo ColorsGizmo;
    
    private void OnDrawGizmos()
    {
      if(!Collider) return;

      SwitchColor();
      
      Gizmos.DrawCube(
        GetCenter(Collider.transform),
        Collider.size);
    }

    private void SwitchColor()
    {
      switch (ColorsGizmo)
      {
        case ColorsGizmo.red:
          Gizmos.color = Color.red;
          break;
        case ColorsGizmo.green:
          Gizmos.color = Color.green;
          break;
        case ColorsGizmo.blue:
          Gizmos.color = Color.blue;
          break;
        case ColorsGizmo.yelow:
          Gizmos.color = Color.yellow;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private Vector2 GetCenter(Transform colliderObject)
    {
      return new Vector2(
        colliderObject.position.x + Collider.offset.x,
        colliderObject.position.y + Collider.offset.y);
    }
  }
}
