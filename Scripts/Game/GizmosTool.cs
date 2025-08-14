using Halabang.Utilities;
using UnityEngine;

namespace Halabang.Game {
  public class GizmosTool : MonoBehaviour {
    [SerializeField] private float drawLength = 5;
    [SerializeField] private Color drawLineColor = Color.green;
    [SerializeField] private float drawSize = 1;
    [SerializeField] private Color drawShapeColor = Color.red;
    [SerializeField] private GameModel.SHAPE drawShape = GameModel.SHAPE.SPHERE;
    //[SerializeField] private bool drawOnSelectOnly;
    [SerializeField] private bool drawDirectionLine;
    [SerializeField] private bool drawPositionShape;

    private void OnDrawGizmos() {
      //paint();
    }
    private void OnDrawGizmosSelected() {
      paint();
    }

    private void paint() {
      if (drawDirectionLine) paintDirection();
      if (drawPositionShape) paintShape();
    }
    public void paintDirection() {
      Gizmos.color = drawLineColor;
      Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * drawLength);
    }
    private void paintShape() {
      Gizmos.color = drawShapeColor;
      switch (drawShape) {
        case GameModel.SHAPE.BOX:
          Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.zero);
          Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
          Gizmos.matrix *= cubeTransform;
          Gizmos.DrawCube(Vector3.zero, Vector3.one);
          Gizmos.matrix = oldGizmosMatrix;
          break;
        case GameModel.SHAPE.SPHERE:
          Gizmos.DrawSphere(transform.position, drawSize);
          break;
      }
    }
  }
}
