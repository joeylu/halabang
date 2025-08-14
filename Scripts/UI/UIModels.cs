using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Halabang.UI {
  public class UI_DisplaySets {
    public AssetReferenceSprite SlotIcon { get; private set; }
    public AssetReferenceSprite ConsoleIcon { get; private set; }
    public AssetReferenceSprite BarkIcon { get; private set; }
    public AssetReferenceSprite NotificationIcon { get; private set; }
    public AssetReferenceSprite BackgroundImage { get; private set; }
    public AssetReferenceSprite PortraitImage { get; private set; }
    public AssetReferenceSprite HeaderImage { get; private set; }
    public AssetReferenceSprite DocumentImage { get; private set; }
    public AssetReferenceSprite BannerImage { get; private set; }

    public void SetImages(UI_Images_Setting imageSettings) {
      BackgroundImage = imageSettings.Background;
      PortraitImage = imageSettings.Portrait;
      HeaderImage = imageSettings.Header;
      DocumentImage = imageSettings.Document;
      BannerImage = imageSettings.Banner;
    }
    public void SetIcons(UI_Icons_Setting iconSettings) {
      SlotIcon = iconSettings.Slot;
      ConsoleIcon = iconSettings.Console;
      BarkIcon = iconSettings.Bark;
      NotificationIcon = iconSettings.Notification;
    }
    public AssetReferenceSprite GetIconReference(UI_Dictionary.ICON_SET_TYPE iconType) {
      AssetReferenceSprite icon = null;
      switch (iconType) {
        case UI_Dictionary.ICON_SET_TYPE.SLOT:
          icon = SlotIcon;
          break;
        case UI_Dictionary.ICON_SET_TYPE.CONSOLE:
          icon = ConsoleIcon;
          break;
        case UI_Dictionary.ICON_SET_TYPE.BARK:
          icon = BarkIcon;
          break;
        case UI_Dictionary.ICON_SET_TYPE.NOTIFICATION:
          icon = NotificationIcon;
          break;
      }
      return icon;
    }
    public AssetReferenceSprite GetImageReference(UI_Dictionary.IMAGE_TYPE imageType) {
      AssetReferenceSprite image = null;
      switch (imageType) {
        case UI_Dictionary.IMAGE_TYPE.BACKGROUND:
          image = BackgroundImage;
          break;
        case UI_Dictionary.IMAGE_TYPE.HEADER:
          image = HeaderImage;
          break;
        case UI_Dictionary.IMAGE_TYPE.PORTRAIT:
          image = PortraitImage;
          break;
        case UI_Dictionary.IMAGE_TYPE.DOCUMENT:
          image = DocumentImage;
          break;
        case UI_Dictionary.IMAGE_TYPE.BANNER:
          image = BannerImage;
          break;
      }
      return image;
    }
  }
  [Serializable]
  public struct UI_Icons_Setting {
    [Tooltip("Icon that will be displayed in a standard slot")]
    public AssetReferenceSprite Slot;
    [Tooltip("Icon that will be displayed in a console screen")]
    public AssetReferenceSprite Console;
    [Tooltip("Icon that will be displayed in a bark UI")]
    public AssetReferenceSprite Bark;
    [Tooltip("Icon that will be displayed in an notification such as examinable")]
    public AssetReferenceSprite Notification;
  }

  [Serializable]
  public struct UI_Images_Setting {
    [Tooltip("An image can be placed in any general unspecified aspect ratio background")]
    public AssetReferenceSprite Background;
    [Tooltip("An image can be placed as a header image, usually in wide aspect ratio")]
    public AssetReferenceSprite Header;
    [Tooltip("An image can be placed as a banner image, usually in super wide aspect ratio")]
    public AssetReferenceSprite Banner;
    [Tooltip("An image can be placed in a portrait, usually an aspect ration between 1:1 to 1:1.5")]
    public AssetReferenceSprite Portrait;
    [Tooltip("An image can be placed in general detail, a rectangle with minor portrait or landscape aspect ratio")]
    public AssetReferenceSprite Document;
  }


  [Serializable]
  public class UI_Dictionary {
    /// <summary>
    /// Indicate how to fill current skill to the target skill slot logic
    /// </summary>
    public enum SLOT_POSITION {
      DEFAULT, FIRST_AVAILABLE, REPLACE_FIRST, CUSTOM
    }
    public enum ICON_SET_TYPE {
      NONE, SLOT, CONSOLE, BARK, NOTIFICATION
    }
    public enum IMAGE_TYPE {
      NONE, ICON, HEADER, BANNER, DOCUMENT, PORTRAIT, BACKGROUND, TRANSPARENT
    }
    public enum EXIT_BUTTON_TYPE {
      NULL, EXAMIN, STORY, INVENTORY
    }
    public enum FONT_TYPE {
      DEFAULT, NORMAL, LIGHT, BOLD, HEADER, STYLISH, ALTERNATIVE
    }
    public enum TEXT_TYPE {
      STATIC_TEXT, LOCALIZED_UI_TEXT
    }
  }
}