using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;

namespace Halabang.Utilities {
  public static class DataHelper {    
    public static T CloneShallowCopy<T>(this T obj) {
      var inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

      return (T)inst?.Invoke(obj, null);
    }
    /// <summary>
    /// When there are more than one same description, only return the first one found in enum
    /// </summary>
    /// <returns></returns>
    public static T GetEnumByDescription<T>(string description) {
      Type enumType = typeof(T);
      if (!enumType.IsEnum) {
        throw new Exception("T must be an Enumeration type.");
      }
      foreach (T ev in Enum.GetValues(enumType)) {
        var field = enumType.GetField(ev.ToString());
        var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0) {
          //UnityEngine.Debug.Log("111: " + ((DescriptionAttribute)attributes[0]).Description);
          if (((DescriptionAttribute)attributes[0]).Description == description) {
            //found result
            return ev;
          }
        }
      }
      throw new Exception("Cannot get enum by given description");
    }
    public static T GetEnumValue<T>(string str) where T : struct, IConvertible {
      Type enumType = typeof(T);
      if (!enumType.IsEnum) {
        throw new Exception("T must be an Enumeration type.");
      }
      T val;
      return Enum.TryParse<T>(str, true, out val) ? val : default(T);
    }
    public static IEnumerable<T> SortEnumByName<T>() {
      return from e in Enum.GetValues(typeof(T)).Cast<T>()
             let nm = e.ToString()
             orderby nm
             select e;
    }
    public static IEnumerable<T> EnumStringList<T>() {
      return from e in Enum.GetValues(typeof(T)).Cast<T>()
             let nm = e.ToString()
             select e;
    }
    public static string GetRandomLetter(int len) {
      Random rnd = default(Random);
      string str = null;
      string result = null;
      rnd = new Random();
      str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      result = "";
      while (len > 0) {
        result += str[rnd.Next(0, str.Length - 1)];
        len -= 1;
      }
      return result;
    }
    public static string GetRandomStringFromArray(List<string> names) {
      try {
        int index = new Random().Next(0, names.Count());
        var name = names[index];
        names.RemoveAt(index);
        return name;
      } catch (Exception ex) {
        return "";
      }
    }
    public static string PluralFormat(int Unit, string SingleUnit, string PluralUnit) {
      if (Unit > 1) {
        return PluralUnit;
      } else {
        return SingleUnit;
      }
    }
    public static DataTable ConvertToDataTable<TSource>(IEnumerable<TSource> source) {
      if (source != null) {
        try {
          var props = typeof(TSource).GetProperties();

          var dt = new DataTable();
          dt.Columns.AddRange(
            props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
          );

          source.ToList().ForEach(
            i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
          );

          return dt;
        } catch (Exception ex) {
          return null;
        }
      } else {
        return null;
      }
    }
    public static string TraditionalToSimplied(object Expression) {
      try {
        return "";
      } catch (Exception ex) {
        return "";
      }
    }
    public static string GenerateSlug(this string phrase) {
      string str = phrase.ToLower();
      //str = phrase.RemoveAccent().ToLower();
      // remove invalid chars           
      str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
      //str = Regex.Replace(str, @"[^A-Za-z0-9_\.~]+", "-");
      // convert multiple spaces into one space   
      str = Regex.Replace(str, @"\s+", " ").Trim();
      // cut and trim 
      str = str.Substring(0, str.Length <= 200 ? str.Length : 200).Trim();
      str = Regex.Replace(str, @"\s", "-"); // hyphens   
      str = str.Replace("---", "-").Replace("--", "-");
      return str;
    }
    public static int GetSlugID(this string slug) {
      if (!string.IsNullOrWhiteSpace(slug)) {
        string result = slug.Split('-').Last();
        if (ValidationHelper.IsNumeric(result)) {
          return Int16.Parse(result);
        }
      }
      return -1;
    }
    public static string RemoveAccent(this string txt) {
      byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
      return System.Text.Encoding.ASCII.GetString(bytes);
    }
    public static string UnderscoreToSpace(this string txt, bool reverse = false) {
      return reverse ? txt.Replace(" ", "_") : txt.Replace("_", " ");
    }
    public static decimal Round95(decimal value) {
      int newvalue = (int)(value / 100) * 100;
      newvalue = newvalue + 95;
      if ((value % 100) > 95)
        newvalue = newvalue + 100;
      return newvalue;
    }
    public static decimal Round50(decimal value) {
      int newvalue = (int)(value / 100) * 100;
      newvalue = newvalue + 50;
      if ((value % 100) > 50)
        newvalue = newvalue + 100;
      return newvalue;
    }
    public static string NumberToWords(int number) {
      if (number == 0)
        return "zero";

      if (number < 0)
        return "minus " + NumberToWords(Math.Abs(number));

      string words = "";

      if ((number / 1000000) > 0) {
        words += NumberToWords(number / 1000000) + " million ";
        number %= 1000000;
      }

      if ((number / 1000) > 0) {
        words += NumberToWords(number / 1000) + " thousand ";
        number %= 1000;
      }

      if ((number / 100) > 0) {
        words += NumberToWords(number / 100) + " hundred ";
        number %= 100;
      }

      if (number > 0) {
        if (words != "")
          words += "and ";

        var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        if (number < 20)
          words += unitsMap[number];
        else {
          words += tensMap[number / 10];
          if ((number % 10) > 0)
            words += "-" + unitsMap[number % 10];
        }
      }

      return words;
    }
    public static string ToUTCString(this DateTime val) {
      //format: YYYYMMDDThhmmssZ where YYYY = year, MM = month, DD = date, T = start of time character, HH or hh = hours, mm = minutes,
      //ss = seconds, Z = end of tag character. The entire tag uses Greenwich Mean Time (GMT)
      return val.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
    }
    public static string UppercaseFirst(string str) {
      string s = "";
      try {
        if (string.IsNullOrWhiteSpace(str)) {
          s = char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
      } catch (Exception ex) {
      }
      return s;
    }
    public static U GetValueByKeyOrNull<T, U>(this Dictionary<T, U> dict, T key)
      where U : class {
      if (dict.ContainsKey(key))
        return dict[key];
      else
        return null;
    }
    public static string GetHtmlTagValue(string HTMLText, string TagType) {
      try {
        Match m = Regex.Match(HTMLText, @"<p>\s*(.+?)\s*</p>");
        switch (TagType) {
          case "h2":
            m = Regex.Match(HTMLText, @"<h2>\s*(.+?)\s*</h2>");
            break;
        }
        if (m.Success) {
          return m.Groups[1].Value;
        } else {
          return "";
        }
      } catch (Exception ex) {
        return "";
      }
    }
    public static string RemoveHtmlTag(string HTMLText) {
      try {
        return Regex.Replace(HTMLText, "<.*?>|&.*?;", string.Empty);
      } catch (Exception ex) {
        return "";
      }
    }
    public static string ConvertDataTableToHTML(DataTable dt) {
      try {
        string html = "<table>";
        //add header row
        html += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
          html += "<td>" + dt.Columns[i].ColumnName + "</td>";
        html += "</tr>";
        //add rows
        for (int i = 0; i < dt.Rows.Count; i++) {
          html += "<tr>";
          for (int j = 0; j < dt.Columns.Count; j++)
            html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
          html += "</tr>";
        }
        html += "</table>";
        return html;
      } catch (Exception ex) {
        return "";
      }
    }
    public static int GetWeekNumber(DateTime date) {
      CultureInfo ciCurr = CultureInfo.CurrentCulture;
      int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
      return weekNum;
    }
    public static bool IsNullableType(Type type) {
      return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
    }
    public static void PopulateDynamicAttributes(object targetObject, PropertyInfo property, object value) {
      try {
        //if attribute data value is null, no need to populate the data
        if (value != null) {
          //Get this property's type
          Type propertyType = property.PropertyType;
          //Get value from datarow
          //Convert.ChangeType does not handle conversion to nullable types
          //if the property type is nullable, we need to get the underlying type of the property
          var targetType = IsNullableType(property.PropertyType) ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
          //if property is a boolean and data value equals true or false
          if (property.PropertyType == typeof(bool) && (value.ToString().ToLower() == "true" || value.ToString().ToLower() == "false")) {
            //Set the value of the property
            property.SetValue(targetObject, value.ToString().ToLower() == "true" ? true : false, null);
          } else {
            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            value = Convert.ChangeType(value, targetType);
            //Set the value of the property
            property.SetValue(targetObject, value, null);
          }
        }
      } catch (Exception ex) {
      }
    }
    public static string SubRightString(string str, string trigger) {
      try {
        str = str.ToLower(); trigger = trigger.ToLower();
        int indexFrom = str.LastIndexOf(trigger);
        if (indexFrom < 0) indexFrom = str.Length;
        str = str.Substring(indexFrom, str.Length - indexFrom);
        return str;
      } catch (Exception ex) {
        return "";
      }
    }
    public static string SubLeftString(string str, string trigger) {
      try {
        int indexTo = str.LastIndexOf(trigger);
        if (indexTo < 0) { indexTo = str.Length; trigger = ""; }
        str = str.Substring(0, indexTo - trigger.Length);
        return str;
      } catch (Exception ex) {
        return "";
      }
    }
    public static bool IsUniqueStringList(this IEnumerable<string> list) {
      return list.Distinct().Count() == list.Count();
    }
    public static T Random<T>(this List<T> list) {
      if (list == null) return default(T);

      Random rng = new Random();
      int randomIndex = rng.Next(0, list.Count);
      return list[randomIndex];
    }
  }
}
