using System.Data;
using System.Data.Common;

namespace Dingo.UnitTests.Helpers;

public class DummyConnection : DbConnection
{
	private readonly IDbConnection _connection;
	private string _connectionString;

	public DummyConnection(IDbConnection connection)
	{
		_connection = connection;
	}

	protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
	{
		return _connection.BeginTransaction() as DbTransaction;
	}

	public override void ChangeDatabase(string databaseName)
	{
		_connection.ChangeDatabase(databaseName);
	}

	public override void Close()
	{
		_connection.Close();
	}

	public override void Open()
	{
		_connection.Open();
	}

	public override string ConnectionString
	{
		get => _connection.ConnectionString;
		set => _connectionString = value;
	}

	public override string Database => _connection.Database;
	public override ConnectionState State => _connection.State;
	public override string DataSource => string.Empty;
	public override string ServerVersion => string.Empty;

	protected override DbCommand CreateDbCommand()
	{
		throw new NotImplementedException();
	}
}
