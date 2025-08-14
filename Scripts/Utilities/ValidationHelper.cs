using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Halabang.Utilities {

  public static class ValidationHelper {
    public static bool IsNumeric(object Expression) {
      bool isNum;
      float retNum;
      isNum = float.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
      return isNum;
    }
    public static bool IsGuid(object Expression) {
      if (Expression is string) {
        Guid newGuid;
        return Guid.TryParse(Expression.ToString(), out newGuid);
      } else {
        return false;
      }
    }
    public static bool IsArray(object value) {
      try {
        Array array = value as Array;
        return array != null && array.Length > 0;
      } catch (Exception ex) {
        return false;
      }
    }
    public static bool IsDateTime(object str) {
      try {
        DateTime newDatetime;
        return DateTime.TryParse(str.ToString(), out newDatetime);
      } catch (Exception ex) {
        return false;
      }
    }
    public static bool IsValidDateRange(bool LimitToday, DateTime objDate) {
      if (LimitToday) {
        return (objDate > Convert.ToDateTime("1/1/1980") && objDate <= DateTime.Now.AddDays(1));
      } else {
        return (objDate > Convert.ToDateTime("1/1/1980"));
      }
    }
    public static bool IsEmail(object Expression) {
      try {
        return Regex.IsMatch(Expression.ToString(),
           @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
           RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
      } catch (Exception ex) {
        return false;
      }
    }
    public static bool IsLetterNumberHyphen(object Expression) {
      try {
        return Regex.IsMatch(Expression.ToString(),
           @"^[a-zA-Z0-9_.-]*$",
           RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
      } catch (Exception ex) {
        return false;
      }
    }
    public static bool IsValidURL(object Expression) {
      try {
        return Regex.IsMatch(Expression.ToString(),
           @"^(ht|f)tp(s?)\:\/\/(([a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+)+)|localhost)(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?([\d\w\.\/\%\+\-\=\&amp;\?\:\\\&quot;\'\,\|\~\;]*)$",
           RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
      } catch (Exception ex) {
        return false;
      }
    }
    public static bool IsValidHexColor(object Expression) {
      try {
        return Regex.IsMatch(Expression.ToString(),
           @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
           RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
      } catch (Exception ex) {
        return false;
      }
    }
    public static bool IsValidStringLength(object Expression, int MinLength, int MaxLength) {
      bool IsValid = false;
      try {
        if (Expression != null) {
          int givenNum = Expression.ToString().Trim().Length;
          if (MinLength <= givenNum && givenNum <= MaxLength) {
            IsValid = true;
          }
        }
      } catch (Exception ex) {
      }
      return IsValid;
    }
    public static bool IsValidCountry(string countryCode) {
      return CultureInfo
          .GetCultures(CultureTypes.SpecificCultures)
              .Select(culture => new RegionInfo(culture.LCID))
                  .Any(region => region.TwoLetterISORegionName == countryCode);
    }

    public static bool HttpURLExist(string url) {
      bool result = false;

      WebRequest webRequest = WebRequest.Create(url);
      webRequest.Timeout = 1200; // miliseconds
      webRequest.Method = "HEAD";

      HttpWebResponse response = null;

      try {
        response = (HttpWebResponse)webRequest.GetResponse();
        result = true;
      } catch (Exception ex) {
      } finally {
        if (response != null) {
          response.Close();
        }
      }

      return result;
    }
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
      return collection == null || collection.Count == 0;
    }
  }
}
