
namespace ModelBuilder.Database
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Data;
  #endregion

  /// <summary>
  /// IADOConnection object
  /// </summary>
  public interface IADOConnection : IDisposable
  {


    /// <summary>
    /// Creates an ADO Command object
    /// </summary>
    /// <returns></returns>
    IDbCommand CreateCommand();


    void SaveChanges();

  }
}
