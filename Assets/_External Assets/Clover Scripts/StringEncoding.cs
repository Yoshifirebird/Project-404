//Original Script written by Fernode

using System;
using System.Text;

public class StringEncoding {

  public static string base64Encode (string data) {
    byte[] x = Encoding.UTF8.GetBytes (data);
    return Convert.ToBase64String (x);
  }

  public static string base64Decode (string data) {
    byte[] x = Convert.FromBase64String (data);
    return Encoding.UTF8.GetString (x);
  }

  /// <summary>
  /// Trying to decode base64 encoded data and returns it's result
  /// </summary>
  /// <typeparam name="T">Mostly string</typeparam>
  /// <param name="input">The data you want to test</param>
  /// <returns>bool</returns>
  public static bool IsValidBase64<T> (T input) {
    try {
      base64Decode (input.ToString ());
    }
    catch { return false; }
    return true;
  }

  public static string ReverseString (string i) {
    char[] charArray = i.ToCharArray ();
    Array.Reverse (charArray);
    return new string (charArray);
  }

}
