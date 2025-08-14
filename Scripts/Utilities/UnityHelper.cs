using UnityEngine;

namespace Halabang.Utilities {
  public static class UnityHelper {

    #region RANDOM_VALUES
    //base on collider bounds, return a position vector3 within
    public static Vector3 RandomPosition(this Bounds parent) {
      return new Vector3(
        UnityEngine.Random.Range(parent.min.x, parent.max.x),
        UnityEngine.Random.Range(parent.min.y, parent.max.y),
        UnityEngine.Random.Range(parent.min.z, parent.max.z)
      );
    }
    //base on this vector, return a float between x,y
    public static float RandomBetween(this Vector2 range) {
      return UnityEngine.Random.Range(range.x, range.y);
    }
    public static int RandomIndex(this int count) {
      System.Random rnd = new System.Random();
      return rnd.Next(count);
    }
    //value based
    public static int RandomWithOffset(this int value, float offset) {
      return (int)(UnityEngine.Random.Range(value - offset, value + offset));
    }
    public static float RandomWithOffset(this float value, float offset) {
      return UnityEngine.Random.Range(value - offset, value + offset);
    }
    public static Color RandomWithOffset(this Color value, float offset) {
      return value * (new Vector2(-offset, offset)).RandomBetween();
    }
    public static Vector2 RandomWithOffset(this Vector2 value, float offset) {
      return new Vector2(
        new Vector2(value.x * (1 - offset), value.x * (1 + offset)).RandomBetween(),
        new Vector2(value.y * (1 - offset), value.y * (1 + offset)).RandomBetween()
      );
    }
    public static Vector3 RandomWithOffset(this Vector3 value, float offset) {
      return new Vector3(
        new Vector2(value.x * (1 - offset), value.x * (1 + offset)).RandomBetween(),
        new Vector2(value.y * (1 - offset), value.y * (1 + offset)).RandomBetween(),
        new Vector2(value.z * (1 - offset), value.z * (1 + offset)).RandomBetween()
      );
    }
    public static Vector4 RandomWithOffset(this Vector4 value, float offset) {
      return new Vector4(
        new Vector2(value.x * (1 - offset), value.x * (1 + offset)).RandomBetween(),
        new Vector2(value.y * (1 - offset), value.y * (1 + offset)).RandomBetween(),
        new Vector2(value.z * (1 - offset), value.z * (1 + offset)).RandomBetween(),
        new Vector2(value.w * (1 - offset), value.w * (1 + offset)).RandomBetween()
      );
    }
    public static Quaternion RandomWithOffset(this Quaternion value, float offset) {
      Vector3 euler = value.eulerAngles;
      return Quaternion.Euler(
        new Vector3(
        new Vector2(euler.x * (1 - offset), euler.x * (1 + offset)).RandomBetween(),
        new Vector2(euler.y * (1 - offset), euler.y * (1 + offset)).RandomBetween(),
        new Vector2(euler.z * (1 - offset), euler.z * (1 + offset)).RandomBetween()
        )
      );
    }
    #endregion


  }
}
