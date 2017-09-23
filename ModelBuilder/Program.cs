
namespace ModelBuilder
{
  #region Directives
  using System;
  #endregion


  class Program
  {
    static void Main(string[] args)
    {
      InputArgumentParser parser = new InputArgumentParser();

      if (parser.ParseInputArgs (args))
      {
        Host host = new Host( parser.DatabaseName, parser.ExportFolderName, parser.Namespace  );
        host.Execute();
      }
      if (!parser.Silent)
      {
        Console.WriteLine("Press any key to continue..");

        Console.ReadKey();
      }

    }
  }
}
