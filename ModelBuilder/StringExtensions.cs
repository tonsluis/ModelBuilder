
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  #endregion

  /// <summary>
  /// StringExtensions object
  /// </summary>
  public static class StringExtensions
  {

    public static string Right(this string str, int length)
    {
      if (length > str.Length)
        throw new InvalidOperationException("Length can not be bigger than the string length ");

      return str.Substring(str.Length - length, length);
    }



  }
}
