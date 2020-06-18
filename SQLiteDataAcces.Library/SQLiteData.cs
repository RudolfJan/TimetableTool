using Dapper;
using Logging.Library;
using System;
using System.Collections.Generic;
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

		public static int InitDatabase(string connectionString, string databasePath)
			{
			_connectionString = connectionString;
			_databasePath = databasePath;
			return TableFactory();
			}
		#endregion

		#region Methods

		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <returns>string.</returns>
		public static string GetConnectionString()
			{
			return _connectionString;
			}
		/// <summary>
		/// Creates the database.
		/// </summary>
		protected static int CreateDatabase()
			{
			try
				{
				if (!File.Exists(_databasePath))
					{
					string Dir = Path.GetDirectoryName(_databasePath);
					if (Dir != null)
						{
						Directory.CreateDirectory(Dir);
						}
					SQLiteConnection.CreateFile(_databasePath);
					Log.Trace($"No database found. Created new database{_databasePath}", LogEventType.Event);
					CreateTable("SQL\\CreateVersion.sql");
					return 0; // created a database
					}
				return 1; //database already exists
				}
			catch (Exception ex)
				{
				Log.Trace($"Exception during creating Asset database{_databasePath}", ex, LogEventType.Error);
				throw ex;
				}
			}

		/// <summary>
		/// Creates the table.
		/// </summary>
		/// <param name="command">The command.</param>
		public static void CreateTable(string command)
			{
			try
				{
				string reader = File.ReadAllText(command);
				using IDbConnection DbConnection = new SQLiteConnection(_connectionString);
					{
					int result = DbConnection.Execute(reader);
					}
				}
			catch (SQLiteException sqLiteException)
				{
				Log.Trace($"Exception during create database table command {command}", sqLiteException, LogEventType.Error);
				throw sqLiteException;
				}
			catch (Exception ex)
				{
				Log.Trace($"Exception during create database table command {command}", ex, LogEventType.Error);
				throw ex;
				}
			}

		/// <summary>
		/// Tables the factory.
		/// </summary>
		public static int TableFactory()
			{
			try
				{
				// Make sure database exists
				var output = CreateDatabase();

				// Create tables and views for new database.
				if (output == 0)
					{
					CreateTable("SQL\\TimeTableDb.sql");
					}

				return output;
				}
			catch (Exception e)
				{
				Log.Trace($"Exception during initialization of database", e, LogEventType.Error);
				throw;
				}
			}

		/// <summary>
		/// Clears the table.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		public static void ClearTable(string tableName)
			{
			try
				{
				using IDbConnection DbConnection = new SQLiteConnection(GetConnectionString());
				DbConnection.Execute($"DELETE FROM {tableName}");
				}
			catch (Exception ex)
				{
				Log.Trace($"Cannot clear table {tableName}", ex, LogEventType.Error);
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
				Log.Trace($"Cannot execute query {sqlStatement}", e, LogEventType.Error);
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
			int output;
			try
				{
				using (IDbConnection connection = new SQLiteConnection(connectionString))
					{
					output = connection.Query<int>(sqlStatement, parameters).FirstOrDefault();
					}
				}
			catch (Exception e)
				{
				Log.Trace($"Cannot save data in database using {sqlStatement}", e, LogEventType.Error);
				throw;
				}
			return output;
			}
		#endregion
		}
	}
