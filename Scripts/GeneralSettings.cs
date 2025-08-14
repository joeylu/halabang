using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UnityEngine;

namespace Halabang {
  public static class Constants {
    public const string NULL = "NULL";
    public const string NONE = "<None>";
    public const string _NONE = "_None";
    public const string ROW = "row";
    public const string COLUMN = "column";
    public const string AND = " and "; //CAUTION: space front and back
    public const string OR = " or "; //CAUTION: space front and back
    public const string TRUE = "true";
    public const string FALSE = "false";
    public const string _IS_ = "_is_";
    public const string _ARE_ = "_are_";
    public const string _HAS_ = "_has_";
    public const string _AND_ = "_and_";
    public const string SEMICOLON = ";";
    public const string UNDERSCORE = "_";
    public const string DEFAULT = "default";
    public const string UNKNOW = "unknown";
    public const string IGNORE = "ignore";
    public const string NEW = "new";
    public const string OBTAINED = "obtained";
    public const string CONSUMED = "consumed";

    [Flags]
    public enum OPERATOR {
      [Description(_NONE)] NONE = 1,
      [Description("_equal")] EQUAL = 2,
      [Description("_greater")] GREATER = 4,
      [Description("_less")] LESS = 8,
      [Description("_greaterequal")] GREATER_EQUAL = GREATER | EQUAL,
      [Description("_lessequal")] LESS_EQUAL = LESS | EQUAL
    }
  }
  public class GeneralSettings {
    public enum VALUE_TYPE {
      [Description("Null")] NULL, 
      [Description("System.String")] STRING,
      [Description("System.Boolean")] BOOL,
      [Description("System.Int32")] INTEGER,
      [Description("System.Single")] FLOAT,
      [Description("UnityEngine.Color")] COLOR,
      [Description("UnityEngine.Color")] COLOR_HDR,
      [Description("UnityEngine.Gradient")] COLOR_GRADIENT,
      [Description("UnityEngine.Vector2")] VECTOR2,
      [Description("UnityEngine.Vector3")] VECTOR3,
      [Description("UnityEngine.Vector4")] VECTOR4,
      [Description("UnityEngine.Quaternion")] QUATERNION,
      [Description("UnityEngine.Texture")] TEXTURE,
      [Description("UnityEngine.Texture2D")] TEXTURE_2D,
      [Description("UnityEngine.ParticleSystem.MinMaxCurve")] PARTICLE_CURVE, //does not effect on tweener, only on particle animation helper's  instant trigger
      [Description("UnityEngine.ParticleSystem.MinMaxGradient")] PARTICLE_GRADIENT //does not effect on tweener, only on particle animation helper's  instant trigger
    }
    public enum VALUE_TYPE_SIMPLE {
      [Description("System.String")] STRING,
      [Description("System.Boolean")] BOOL,
      [Description("System.Int32")] INTEGER,
      [Description("System.Single")] FLOAT
    }
    public enum REFLECTION_TYPE {
      [Description("Null")] NULL,
      [Description("Field")] FIELD,
      [Description("Property")] PROPERTY,
      [Description("Method")] METHOD,
    }
    public enum PRIVILEGE {
      PLAYER, DEVELOPER
    }

    [Flags]
    public enum VALUE_LOCK {
      [Description("")] NULL = 1,
      [Description("Lock_X")] LOCK_X = 2,
      [Description("Lock_Y")] LOCK_Y = 4,
      [Description("Lock_Z")] LOCK_Z = 8,
      [Description("Lock_W")] LOCK_W = 16,
    }

    public enum UNITY_UPDATE_TYPE {
      UPDATE, LATE_UPDATE, FIXED_UPDATE
    }
  }

  [Serializable]
  public struct GenericStringPair {
    public string name;
    public string value;
  }
  [Serializable]
  public struct ValueCollection {
    public string ValueString;
    public bool ValueBoolean;
    public int ValueInt32;
    public float ValueFloat;
    public Color ValueColor;
    public Gradient ValueColorGradient;
    public Vector2 ValueVector2;
    public Vector3 ValueVector3;
    public Vector4 ValueVector4;
    public Quaternion ValueQuaternion;
    public ParticleSystem.MinMaxCurve ValueMinMaxCurve;
    public ParticleSystem.MinMaxGradient ValueMinMaxGradient;
    public Texture ValueTexture;
    public Texture2D ValueTexture2D;
  }
  [Serializable]
  public struct ArrayCollection {
    public string[] ArrayString;
    public bool[] ArrayBoolean;
    public int[] ArrayInt32;
    public float[] ArrayFloat;
    public Color[] ArrayColor;
    public Gradient ValueColorGradient;
    public Vector2[] ArrayVector2;
    public Vector3[] ArrayVector3;
    public Vector4[] ArrayVector4;
    public Quaternion[] ArrayQuaternion;
    public Texture[] ArrayTexture;
    public Texture2D[] ArrayTexture2D;
  }
  [Serializable]
  public struct ValueCondition {
    public string Name;
    public GeneralSettings.VALUE_TYPE_SIMPLE ValueType;
    public Constants.OPERATOR Condition;
    public float Value;
  }
  [Serializable]
  public struct UnityModuleCollection {
    public ForceMode ForceMode;
  }
}