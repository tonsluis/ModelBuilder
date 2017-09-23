
namespace ModelBuilder.Database
{
  #region Directives
  // Standard .NET Directives
  using System;
  using System.Collections.Generic;
  #endregion

  /// <summary>
  /// ADOFactory object
  /// </summary>
  public class ADOFactory
  {
    #region Locals
    private static Func<IADOConnection> _defaultCreateMethod = null;
    private static Dictionary<string, Func<IADOConnection>> _createMethods;
    #endregion

    /// <summary>
    /// Calls the default Create method.
    /// </summary>
    /// <returns></returns>
    public static IADOConnection Create()
    {
      if(_defaultCreateMethod == null)
      {
        throw new InvalidOperationException("You must call SetCreateMethod() First.");
      }
      return _defaultCreateMethod();
    }


    /// <summary>
    /// Creates a named ADOConnection
    /// </summary>
    /// <param name="key">name</param>
    /// <returns>IADOConnection object</returns>
    public static IADOConnection Create(string key)
    {
      Func<IADOConnection> returnValue = null;

      if (_createMethods == null) throw new NullReferenceException("You must call SetCreateMethod() first.");

      if(_createMethods.ContainsKey(key.ToUpper()))
      {
        returnValue = _createMethods[key.ToUpper()];
        if (returnValue == null)
          throw new InvalidOperationException("You must call SetCreateMethod() first."); 
      }
      else
      {
        throw new ArgumentException($"Key {key} not found in the Create Methods.");
      }
      return returnValue();
    }


    public static void SetCreateMethod(string key, Func<IADOConnection> createMethod, bool setAsDefault = false)
    {
      #region Parameter evaluation
      if (createMethod == null) throw new NullReferenceException(nameof(createMethod));
      #endregion

      if (setAsDefault)
      {
        _defaultCreateMethod = createMethod;
      }
      if (_createMethods == null) _createMethods = new Dictionary<string, Func<IADOConnection>>();
      if(_createMethods.ContainsKey(key.ToUpper()))
      {
        _createMethods.Add(key.ToUpper(), createMethod);
      }
    }


  }
}
