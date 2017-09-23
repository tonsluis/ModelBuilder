
namespace ModelBuilder
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.IO;
  using System.Text;
  using System.Data;
  using Database;

  #endregion

  /// <summary>
  /// DbColumn object
  /// </summary>
  public class DbColumn
  {
    #region Locals

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instnace of the <see cref="DbColumn"/> Class.
    /// </summary>
    public DbColumn()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbColumn"/> Class.
    /// </summary>
    public DbColumn(string tableName,string name)
    {
      _tableName = tableName;
      _name = name;
    }

    #endregion

    #region Properties

    #region TableName
    public string TableName
    {
      get { return _tableName; }
      set { _tableName = value; }
    }
    private string _tableName;
    #endregion

    #region Name
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _name;
    #endregion

    #region isPrimary
    public bool IsPrimary 
    {
      get { return _isPrimary; }
      set { _isPrimary = value; }
    }
    private bool _isPrimary;
    #endregion

    #region IsForeignKey
    public bool IsForeignKey
    {
      get { return _isForeignKey ; }
      set { _isForeignKey = value; }
    }
    private bool _isForeignKey = false;
    #endregion

    #region TargetTable
    public string TargetTable
    {
      get { return _targetTable; }
      set { _targetTable = value; }
    }
    private string _targetTable = string.Empty;
    #endregion

    #region TargetColumn
    public string TargetColumn
    { 
      get { return _targetColumn; }
      set { _targetColumn = value; }
    }
    private string _targetColumn = string.Empty;
    #endregion

    #region DataType
    public string DataType
    {
      get { return _dataType; }
      set { _dataType = value;
            _type = DataTypeMatter.ConvertToDotNetType(_dataType);
      }
    }
    private string _dataType;
    #endregion

    #region Type
    public string Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private string _type;
    #endregion

    #region IsNullAble
    public bool IsNullAble
    {
      get { return _isNullable; }
      set { _isNullable = value; }
    }
    private bool _isNullable;
    #endregion

    #region Length
    public int Length
    {
      get { return _length; }
      set { _length = value; }
    }
    private int _length;
    #endregion

    #endregion

    #region Public Routines

    public void Analyse(IADOConnection conn)
    {

      using (var cmd = conn.CreateCommand())
      {
        StringBuilder query = new StringBuilder();
        query.AppendLine("select CCU.table_name src_table, CCU.constraint_name src_constraint, CCU.column_name src_col, KCU.table_name target_table, KCU.column_name target_col ");
        query.AppendLine("from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU, INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC, INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU ");
        query.AppendLine($"Where CCU.table_name = '{_tableName}' AND CCU.column_name = '{_name}' AND CCU.constraint_name = RC.constraint_name And KCU.constraint_name = RC.unique_constraint_name ");
        query.AppendLine("order by CCU.table_name, CCU.constraint_name ");

        cmd.CommandText = query.ToString();
        IDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          IsForeignKey = true;
          TargetTable = reader["target_table"].ToString();
          TargetColumn = reader["target_col"].ToString();
        }
        reader.Close();
        reader = null;



        }
      }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    public void Save(StreamWriter file)
    {
      string nullAbleSign = (IsNullAble && _type != "string") ? "?" : "";

      WriteFile(file,"/// <summary>");
      WriteFile(file, $"/// {_name} property");
      WriteFile(file,"/// </summary>");
      WriteFile(file, $"public {_type}{nullAbleSign} {_name} {"{"} get; set; {"}"}");



      //ToDo : add Foreign Key Annotation and link to the remote table.
      if (IsForeignKey)
      {
        string foreignClass = ConvertToClassName(TargetTable);

        WriteFile(file, "/// <summary>");
        WriteFile(file, $"/// the actual {foreignClass} class");
        WriteFile(file, "/// </summary>");
        WriteFile(file, $"[ForeignKey(\"{_name}\")]");
        WriteFile(file, $"public virtual {foreignClass} {foreignClass} {"{"} get; set; {"}"}");
      }
    }

    #endregion

    #region Private routines

    private void WriteFile(StreamWriter file, string line)
    {
      file.WriteLine($"    {line}");
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
