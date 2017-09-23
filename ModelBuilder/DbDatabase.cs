
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Text;
  using System.Data;
  using System.Collections.Generic;

  using Database;
  #endregion

  /// <summary>
  /// DbDatabase object
  /// </summary>
  public class DbDatabase
  {
    #region Contructor
    /// <summary>
    /// Initializes a new instance of the <see cref="DbDatabase"/> class.
    /// </summary>
    public DbDatabase()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbDatabase"/> Class.
    /// </summary>
    public DbDatabase(string name, string nameSpace)
    {
      _name = name;
      _nameSpace = nameSpace;
    }
    #endregion

    #region Properties

    #region Name
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _name = string.Empty;
    #endregion

    #region Tables
    public ICollection<DbTable> Tables
    {
      get { return _tables; }
    }
    private ICollection<DbTable> _tables = new List<DbTable>();
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
    public void Analyse(IADOConnection conn)
    {
      using (var cmd = conn.CreateCommand())
      {
        StringBuilder query = new StringBuilder();

        query.AppendLine("SELECT * ");
        query.AppendLine("From INFORMATION_SCHEMA.TABLES ");
        query.AppendLine("Where TABLE_TYPE = 'BASE TABLE'");

        cmd.CommandText = query.ToString();
        IDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
          DbTable table = new DbTable(reader["TABLE_NAME"].ToString() );
          if (Name == string.Empty)
          {
            Name = reader["TABLE_CATALOG"].ToString();
          }
          _tables.Add(table);
        }
        reader.Close();
        reader = null;

        foreach(DbTable table in _tables )
        {
          table.Analyse(conn);
        }
      }
    }

    public void CreateModel(string path)
    {
      foreach(DbTable table in _tables )
      {
        table.CreateModelClass(_nameSpace , path);
      }
    }


    public void CreateMapper(string path)
    {
      foreach(DbTable table in _tables)
      {
        table.CreateMapperClass(_nameSpace , path);
      }


    }

    #endregion

  }
}
