
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Collections.Generic;
  #endregion

  /// <summary>
  /// DataTypeMatter object
  /// </summary>
  public static class DataTypeMatter
  {
    #region Locals
    private static Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sqlType"></param>
    /// <returns></returns>
    public static string ConvertToDotNetType(string sqlType)
    {
      Initialize();
      if (!_dictionary.ContainsKey(sqlType.ToLower()))
        throw new NullReferenceException($"datatype {sqlType.ToLower()} not present in the conversion mapper."); 

      return _dictionary[sqlType];
    }


    private static void Initialize()
    {
      if (_dictionary.Count == 0)
      {
        _dictionary.Add("bigint", "long");
        _dictionary.Add("binary", "byte[]");
        _dictionary.Add("bit", "bool");
        _dictionary.Add("char", "Char[]");
        _dictionary.Add("date", "DateTime");
        _dictionary.Add("datetime", "DateTime");
        _dictionary.Add("datetime2", "DateTime");
        _dictionary.Add("datetimeoffset", "DateTimeOffset");
        _dictionary.Add("decimal", "decimal");
        _dictionary.Add("varbinary", "byte[]");
        _dictionary.Add("float", "double");
        _dictionary.Add("image", "byte[]");
        _dictionary.Add("int", "Int32");
        _dictionary.Add("money", "decimal");
        _dictionary.Add("nchar", "string");
        _dictionary.Add("ntext", "string");
        _dictionary.Add("numeric", "decimal");
        _dictionary.Add("nvarchar", "string");
        _dictionary.Add("real", "single");
        _dictionary.Add("rowversion", "byte[]");
        _dictionary.Add("smallint", "Int16");
        _dictionary.Add("smallmoney", "decimal");
        _dictionary.Add("text", "string");
        _dictionary.Add("timestamp", "byte[]");
        _dictionary.Add("tinyint", "byte");
        _dictionary.Add("uniqueidentifier", "Guid");
        _dictionary.Add("varchar", "string");
        _dictionary.Add("xml", "Xml");
      }
    }



  }
}
