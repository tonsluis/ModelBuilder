
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Text;
  using System.Data;
  using System.IO;
  using System.Collections.Generic;

  // CSi Directives
  using Database;
  #endregion

  /// <summary>
  /// DbTable object
  /// </summary>
  public class DbTable
  {
    #region Locals
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="DbTable"/> Class.
    /// </summary>
    public DbTable(string name)
    {
      _name = name;
      ClassName = ConvertToClassName(name);
    }
    #endregion

    #region Properties
    #region Name
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _name;
    #endregion

    #region ClassName
    public string ClassName
    {
      get { return _className = string.Empty; }
      set { _className  = value; }
    }
    private string _className = string.Empty;
    #endregion

    #region Columns
    public ICollection<DbColumn> Columns
    {
      get { return _columns; }
    }
    private ICollection<DbColumn> _columns = new List<DbColumn>();
    #endregion

    #endregion

    #region Public Routines
    /// <summary>
    /// Analyses the table.
    /// </summary>
    public void Analyse(IADOConnection conn)
    {
      using (var cmd = conn.CreateCommand())
      {
        StringBuilder query = new StringBuilder();
        query.AppendLine("SELECT  COLUMNPROPERTY(SC.id, name, 'IsIdentity') as [Identity] , * ");
        query.AppendLine("FROM INFORMATION_SCHEMA.COLUMNS i_S ");
        query.AppendLine("Inner Join syscolumns SC on (SC.Name = i_S.COLUMN_NAME) and (OBJECT_NAME(SC.id) = I_S.TABLE_NAME) ");
        query.AppendLine($"Where I_S.TABLE_NAME = '{_name}'");

        cmd.CommandText = query.ToString();
        IDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          DbColumn column = new DbColumn(_name, reader["COLUMN_NAME"].ToString())
          {
            IsPrimary = (Convert.ToInt32(reader["Identity"]) != 0),
            DataType = reader["DATA_TYPE"].ToString().ToLower(),
            IsNullAble = (reader["IS_NULLABLE"].ToString().ToUpper() == "YES"),
            Length = reader["CHARACTER_MAXIMUM_LENGTH"] as int? ?? default(int)
          };
          _columns.Add( column);
        }
        reader.Close();
        reader = null;

        foreach(var item in _columns)
        {
          item.Analyse(conn);
        }
      }
    }


    public  void CreateModelClass(string nameSpace, string folderName)
    {
      string filename = $"{_className}.cs";
      string FilePath = Path.Combine(folderName, filename);
      StreamWriter file = new StreamWriter(FilePath);

      file.WriteLine("");
      file.WriteLine($"namespace {nameSpace}");
      file.WriteLine("{");

      file.WriteLine("  #region Directives");

      file.WriteLine("  // Standard .NET Directives");
      file.WriteLine("  using System;");
      file.WriteLine("  using System.ComponentModel.DataAnnotations.Schema;");
      file.WriteLine("  ");
      file.WriteLine("  // CSi Directves");
      file.WriteLine("  #endregion");
      file.WriteLine("  ");
      file.WriteLine("  /// <summary>");
      file.WriteLine($"  /// {_className} class.");
      file.WriteLine("  /// </summary>");
      file.WriteLine("  [Serializable]");
      file.WriteLine($"  public class {_className}");
      file.WriteLine("  {");

      foreach (var column in _columns)
      {
        column.Save(file);
      }

      file.WriteLine("  }");
      file.WriteLine("}");

      file.Close();
      file = null;
    }

    public void CreateMapperClass(string nameSpace, string folderName)
    {
      string filename = $"{_className}.cs";
      string FilePath = Path.Combine(folderName, filename);
      StreamWriter file = new StreamWriter(FilePath);

      file.WriteLine("");
      file.WriteLine($"namespace {nameSpace}");
      file.WriteLine("{");
      file.WriteLine("  #region Directives");

      file.WriteLine("  // Standard .NET Directives");
      file.WriteLine("  using System;");
      file.WriteLine("  using More.Logistics.Framework.Data.Mapper;");

      file.WriteLine("  // CSi Directves");
      file.WriteLine("  #endregion");
      file.WriteLine("  ");
      file.WriteLine("  /// <summary>");
      file.WriteLine($"  /// {_className}Mapper class.");
      file.WriteLine("  /// </summary>");
      file.WriteLine($"  public class {_className}Mapper : CrudEdentityMapper<{_className}>");
      file.WriteLine("  {");
      file.WriteLine("    /// <summary>");
      file.WriteLine($"    /// Initializes a new instance of the <see cref=\"{_className}Mapper\"/> class.");
      file.WriteLine("    /// <summary>");
      file.WriteLine($"    public {_className}Mapper() : base(\"{_name}\")");
      file.WriteLine("    {");
      foreach (DbColumn column in _columns)
      {
        if (column.IsPrimary )
        {
          file.WriteLine($"      Property(a => a.{column.Name}).PrimaryKey(true);");
        }

      }
      file.WriteLine("    }");

      file.WriteLine("  }");

      file.WriteLine("}");
      file.Close();
      file = null;
    }




      private string ConvertToClassName(string name)
    {
      if (name.Right(1) == "s")
      {
        return name.Substring(0, name.Length - 1);
      }
      return name;
    }

    #endregion
  }
}
