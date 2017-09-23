
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.IO;
  using Database;
  
  #endregion

  /// <summary>
  /// Host object
  /// </summary>
  public class Host
  {
    #region Locals

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance if the <see cref="Host"/> class.
    /// </summary>
    public Host()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Host"/> class.
    /// </summary>
    /// <param name="databaseName">Database name to analyse</param>
    /// <param name="exportFolderName">Foldername where the classes are to be stored.</param>
    /// <param name="nameSpace">Namespace of the classes.</param>
    public Host(string databaseName, string exportFolderName, string nameSpace)
    {
      _databaseName = databaseName;
      _exportFolderName = exportFolderName;
      _nameSpace = nameSpace;
    }

    #endregion

    #region Properties

    #region ExportFolderName
    public string ExportFolderName
    {
      get { return _exportFolderName = string.Empty; }
      set { _exportFolderName  = value; }
    }
    private string _exportFolderName = string.Empty;
    #endregion

    #region Database
    public string DatabaseName
    {
      get { return _databaseName; }
      set { _databaseName = value; }
    }
    private string _databaseName = string.Empty;
    #endregion

    #region Namespace
    public string Namespace
    {
      get { return _nameSpace; }
      set { _nameSpace = value; }
    }
    private string _nameSpace = string.Empty;
    #endregion

    #endregion

    #region Public Routines
    public void Execute()
    {
      if (PrepareExportLocation())
      {
        ADOFactory.SetCreateMethod("default", DefaultConnection, true);


        using (var conn = ADOFactory.Create())
        {
          DbDatabase myDb = new DbDatabase(_databaseName,_nameSpace );

          myDb.Analyse(conn);

          string modelFolderName = Path.Combine(_exportFolderName, "Model");
          myDb.CreateModel(modelFolderName);

          string mapperFolderName = Path.Combine(_exportFolderName, "Mapper");
          myDb.CreateMapper(mapperFolderName);


        }
      }
    }

    #endregion

    #region Private routines

    /// <summary>
    /// Make sure the export location is prepped.
    /// </summary>
    private bool PrepareExportLocation()
    {
      // Check if the export folder exists.
      if (!Directory.Exists(_exportFolderName))
      {
        Console.WriteLine($"Folder '{_exportFolderName}' does not exist.");
        return false;
      }
      string modelFolderName = Path.Combine(_exportFolderName, "Model");
      // Check if the Model Folder exists
      if (!Directory.Exists(modelFolderName))
      {
        try
        {
          Directory.CreateDirectory(modelFolderName);
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Unable to create Model folder. {ex.Message}");
          return false;
        }
      }
      string mapperFolderName = Path.Combine(_exportFolderName, "Mapper");
      // Check if the Mapper Folder exists
      if (!Directory.Exists(mapperFolderName))
      {
        try
        {
          Directory.CreateDirectory(mapperFolderName);
        }
        catch(Exception ex)
        {
          Console.WriteLine($"Unable to create the Mapper folder. {ex.Message}");
          return false;
        }

      }
      return true;
    }



    private IADOConnection DefaultConnection()
    {
      string connString = string.Format(@"Data Source =.; Initial Catalog = {0}; Integrated Security = True; ",_databaseName );
      return  ADOConnection.Create(connString);
    }

    #endregion

  }
}
