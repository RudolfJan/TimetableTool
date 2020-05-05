using Dapper;
using Logging.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace SQLiteDataAccess.Library
  {
  public class SQLiteData
    {
       #region Properties

    /// <summary>
    /// Gets or sets the database path.
    /// </summary>
    /// <value>The database path.</value>
    public static string _databasePath;

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <value>The connection string.</value>
    private static string _connectionString; 


    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
    /// </summary>
    //public AssetDatabaseAccess(string connectionString, string databasePath)
    //  {
    //  _connectionString = connectionString;
    //  _databasePath = databasePath;
    //  TableFactory();
    //  }


    public static void InitDatabase(string connectionString, string databasePath, bool useDemoData)
      {
      _connectionString = connectionString;
       _databasePath = databasePath;
      TableFactory(useDemoData);
      }
    #endregion

    #region Methods

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <returns>String.</returns>
    public static String GetConnectionString()
      {
      return _connectionString;
      }
    /// <summary>
    /// Creates the database.
    /// </summary>
    protected static void CreateDatabase()
      {
      try
        {
        if (!File.Exists(_databasePath))
          {
          String Dir = Path.GetDirectoryName(_databasePath);
          if (Dir != null)
            {
            Directory.CreateDirectory(Dir);
            }
          SQLiteConnection.CreateFile(_databasePath);
          Log.Trace($"No database found. Created new database{_databasePath}", LogEventType.Event);
          }
        }
      catch (Exception ex)
        {
        Log.Trace($"Exception during creating Asset database{_databasePath}",ex,LogEventType.Error);
        throw ex;
        }
      }

    /// <summary>
    /// Creates the table.
    /// </summary>
    /// <param name="command">The command.</param>
    protected static void CreateTable(String command)
      {
      try
        {
        String reader = File.ReadAllText(command);
        using IDbConnection DbConnection = new SQLiteConnection(_connectionString);
          {
          Int32 result = DbConnection.Execute(reader);
          }
        }
      catch (SQLiteException sqLiteException)
        {
        Log.Trace($"Exception during create database table command {command}",sqLiteException,LogEventType.Error);
        throw sqLiteException;
        }
      catch (Exception ex)
        {
        Log.Trace($"Exception during create database table command {command}",ex,LogEventType.Error);
        throw ex;
        }
      }

    /// <summary>
    /// Tables the factory.
    /// </summary>
    public static void TableFactory(bool UseDemoData)
      {
      try
        {
        // Make sure database exists
        CreateDatabase();

        // TableCreation
        CreateTable("SQL\\TimeTableDb.sql");
        
        // ViewCreation
        CreateTable("SQL\\CreateFullTimeEvents.sql");
        // Index creation

        // Create testdata
      if(UseDemoData)
          {
          CreateTable("SQL\\TimeTableData.sql");
          }

        }
      catch (Exception e)
        {
        Log.Trace($"Exception during initialization of database",e,LogEventType.Error);
        throw;
        }
      }

    /// <summary>
    /// Clears the table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public static void ClearTable(String tableName)
      {
      try
        {
        using IDbConnection DbConnection = new SQLiteConnection(GetConnectionString());
        DbConnection.Execute($"DELETE FROM {tableName}");
        }
      catch (Exception ex)
        {
        Log.Trace($"Cannot clear table {tableName}",ex,LogEventType.Error);
        throw ex;
        }
      }

    /// <summary>
    /// Loads the data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>List&lt;T&gt;.</returns>
    public static List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
      {
      try
        {
        using (IDbConnection connection = new SQLiteConnection(connectionString))
          {
          List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
          return rows;
          }
        }
      catch (Exception e)
        {
        Log.Trace($"Cannot execute query {sqlStatement}",e,LogEventType.Error);
        throw;
        }

      }

    /// <summary>
    /// Saves the data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="connectionString">The connection string.</param>
    public static int SaveData<T>(string sqlStatement, T parameters, string connectionString)
      {
      var output = -1;
      try
        {
        using (IDbConnection connection = new SQLiteConnection(connectionString))
          {
          output=connection.Execute(sqlStatement, parameters);
          }
        }
      catch (Exception e)
        {
        Log.Trace($"Cannot save data in database using {sqlStatement}",e,LogEventType.Error);
        throw;
        }
      return output;
      }
    #endregion

    #region exportandimport

    public string ExportTableDefinition(string tableName,string connectionString)
      {
      string output = string.Empty;
      var sqlStatement = "";
      try
        {
        using (IDbConnection connection = new SQLiteConnection(connectionString))
          {
          connection.Execute(sqlStatement);
          }
        }
      catch (Exception e)
        {
        Log.Trace($"Cannot save data in database using {sqlStatement}",e,LogEventType.Error);
        throw;
        }

      return "";
      }

    #endregion
    }
  }
