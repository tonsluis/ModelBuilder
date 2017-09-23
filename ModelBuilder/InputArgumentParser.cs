
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Text;
  using System.Collections.Generic;
  #endregion

  /// <summary>
  /// InputArgumentParser object
  /// </summary>
  /// <remarks>
  /// This class parses the input arguments with which the application is started.
  /// </remarks>
  public class InputArgumentParser
  {
    #region Locals

    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="InputArgumentParser"/> Class.
    /// </summary>
    public InputArgumentParser()
    {

    }

    #endregion

    #region properties


    #region Namespace
    public string Namespace
    {
      get { return _nameSpace; }
      set { _nameSpace = value; }
    }
    private string _nameSpace = string.Empty;
    #endregion



    #region DatabaseName
    public string DatabaseName    
    {
      get { return _databaseName; }
      set { _databaseName = value; }
    }
    private string _databaseName = string.Empty;
    #endregion

    #region ExportFolderName
    public string ExportFolderName
    { 
      get { return _exportFolderName; }
      set { _exportFolderName = value; }
    }
    private string _exportFolderName = string.Empty;
    #endregion

    #region Silent
    public bool Silent
    {
      get { return _silent; }
      set { _silent = value; }
    }
    private bool _silent = false;
    #endregion

    #endregion

    #region Public Routines

    /// <summary>
    /// Parses the input arguments.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public bool ParseInputArgs(string[] args)
    {
      Dictionary<string, string> _reports = new Dictionary<string, string>
      {
        { "database", "Missing the /Database argument." },
        { "exportfolder", "Missing the /ExportFolder argument." },
        { "namespace", "Missing the /Namespace argument." }
      };

      foreach (string arg in args)
      {
        if (arg.Substring(0, 1) != "/")
        {
          Console.WriteLine($"unable to parse {arg} are you missing a '/' ?");
          return false;
        }

        if (!arg.Contains("?"))
        {
          if (!arg.Contains(":"))
          {
            Console.WriteLine($"unable to parse {arg} parameter does not contain a value. are you missing a ':' ?");
            return false;
          }
        }

        KeyValuePair<string, string> parameter = ArgumentToParameter(arg);
        switch (parameter.Key.ToLower())
        {
          case "?":
            ShowArguments();
            return false;
          case "database":
            _databaseName = parameter.Value;
            _reports.Remove("database");
            break;
          case "exportfolder":
            _exportFolderName = parameter.Value;
            _reports.Remove("exportfolder");
            break;
          case "namespace":
            _nameSpace = parameter.Value;
            _reports.Remove("namespace");
            break;
          case "silent":
            _silent = (parameter.Value.ToUpper() == "YES");
            break;
        }
      }

      if (_reports.Count != 0)
      {
        foreach(KeyValuePair<string,string> item in _reports )
        {
          Console.WriteLine(item.Value);
        }
        return false;
      }
      else
      {
        return true;
      }

    }

    #endregion

    #region Private routines

    private KeyValuePair<string,string> ArgumentToParameter(string parameter)
    {

      KeyValuePair<string, string> returnValue = new KeyValuePair<string, string>();

      // Peal of the first '/' forward slash
      string cleanParameter = parameter.Substring(1, parameter.Length - 1);

      string[] values = cleanParameter.Split(':');
      if (values.GetUpperBound(0) >= 0)
      {
        if (values.GetUpperBound(0) == 0)
        {
          return new KeyValuePair<string, string>(values[0], "");
        }
        else
        {
          StringBuilder prmValue = new StringBuilder();
          if (values.GetUpperBound(0) > 1)
          {
            int i = 1;
            while(i <= values.GetUpperBound(0) )
            {
              prmValue.Append (  (prmValue.Length == 0) ? values[i] : ":" + values[i]);
              i++;
            }


          }
          else
          {
            prmValue.Append ( values[1]);
          }

          return new KeyValuePair<string, string>(values[0], prmValue.ToString());
        }
      }
      return returnValue;
    }


    private void ShowArguments()
    {
      Console.WriteLine("");
      Console.WriteLine("This application accepts the following commands:");
      Console.WriteLine(" /? - Shows this help.");
      Console.WriteLine(" /Database - Sets the database name. (local only)");
      Console.WriteLine(" /ExportFolder - Sets the folderName where the generated classes will be exported.");
      Console.WriteLine(" /Silent - Optionally. ('Yes'/'No') Surpresses any messageas and closes the application on completion.");
      Console.WriteLine("");
      Console.WriteLine("The parameters accept values by added the parameter value seperated from the parameter by a simicolon ':'");
      Console.WriteLine("");
      Console.WriteLine(@"Example: ModelBuilder.exe /Database:MM_MultiScience /ExportFolder:C:\temp\exports\MM /Silent:Yes");
    }

    #endregion


  }
}
