
namespace ModelBuilder.Database 
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Data;
  using System.Data.SqlClient;

  #endregion

  /// <summary>
  /// ADOConnection object
  /// </summary>
  public class ADOConnection : IADOConnection
  {
    #region Locals
    private IDbConnection _connection = null;
    private IDbTransaction _transaction = null;
    #endregion


    #region Constructor

    public ADOConnection(IDbConnection connection)
    {
      _connection = connection;
      _transaction = _connection.BeginTransaction();
    }

    #endregion

    public void Dispose()
    {
      if (_transaction != null)
      {
        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
      }

      if (_connection != null)
      {
        if (_connection.State != ConnectionState.Closed )
        {
          _connection.Close();
        }
        _connection.Dispose();
        _connection = null;
      }
    }



    public static IADOConnection Create(string connectionString)
    {
      SqlConnection connection = new SqlConnection(connectionString);
      connection.Open();
      return new ADOConnection(connection);
    }



    public IDbCommand CreateCommand()
    {
      IDbCommand cmd = _connection.CreateCommand();
      cmd.Transaction = _transaction;
      return cmd;
    }

    public void SaveChanges()
    {
      if (_transaction != null)
        throw new InvalidOperationException("Transaction already closed.");
      _transaction.Commit();
      _transaction = null;

    }

  }
}
