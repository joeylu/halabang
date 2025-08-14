using System;
using Halabang.Utilities;
using System.Collections.Generic;
using System.Text;

public class ActorPresetData {
  public int ID { get; set; }
  public string Guid { get; set; }
  public string DisplayNameEN { get; set; }
  public string DisplayNameCNS { get; set; }
  public string DisplayNameCNT { get; set; }
  public int Gender { get; set; }
  public string[] CopywritingBasic { get; set; } //guid of a copywriting preset(data)
  public string[] CopywritingResume { get; set; } //guid of a copywriting preset(data)
  public string[] CopywritingRules { get; set; } //guid of a copywriting preset(data)
  public string[] CopywritingResponseRules { get; set; } //guid of a copywriting preset(data)
  public string[] SlotIcon { get; set; }
  public string[] ConsoleIcon { get; set; }
  public string[] BarkIcon { get; set; }
  public string[] NotificationIcon { get; set; }
  public string[] BackgroundImage { get; set; }
  public string[] HeaderImage { get; set; }
  public string[] PortraitImage { get; set; }
  public string[] DocumentImage { get; set; }
  public long AvatarChipItemID { get; set; } //item runtime ID
  public List<string> Appearances { get; set; } //Where this actor has appeared across times
  public List<string> Timelines { get; set; } //When this actor has been across spaces


  public string IsValid() {
    StringBuilder errorMsg = new StringBuilder();

    if (ID <= 0) errorMsg.Append("ID must be presented, and should be the same unique ID from dialogue actor database");
    if (string.IsNullOrWhiteSpace(DisplayNameEN)) errorMsg.Append("Not an valid first and last name because some of the field are empty. " + Environment.NewLine);
    if (ValidationHelper.IsGuid(Guid) == false) errorMsg.Append("Not an valid actor guid. " + Environment.NewLine);

    return errorMsg.ToString();
  }
}