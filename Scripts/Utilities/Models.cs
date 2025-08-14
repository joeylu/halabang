using System;
using UnityEngine;

namespace Halabang.Utilities {
  public class GameModel {
    public enum SHAPE { NULL, SPHERE, BOX}
  }

  public class GameSetting {
    public static float LONG_DURATION = 5f;
    public static float STANDARD_DURATION = 1f;
    public static float SHORT_DURATION = 0.1f;
    public static float EXTREME_SHORT_DURATION = 0.01f;
  }
}
