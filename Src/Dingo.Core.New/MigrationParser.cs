using Dingo.Core.Exceptions;
using System.Text;
using System.Text.RegularExpressions;
using Trico.Configuration;

namespace Dingo.Core;

internal class MigrationParser
{
	private readonly Regex _delimiter;

	public MigrationParser(IConfiguration configuration)
	{
		_delimiter = new Regex(configuration.Get("migration-delimiter"));
	}

	public Migration Parse(string sql)
	{
		if (string.IsNullOrWhiteSpace(sql))
		{
			return Migration.Empty;
		}

		var lines = sql.ReplaceLineEndings().Split(Environment.NewLine);
		var (up, down) = GetMigrationCommands(lines);

		return new Migration(up, down);
	}

	private (string? up, string? down) GetMigrationCommands(IEnumerable<string> lines)
	{
		var commands = new string?[2];
		var i = 0;
		var sb = new StringBuilder();

		foreach (var s in lines)
		{
			if (_delimiter.IsMatch(s))
			{
				commands[i++] = sb.ToString().TrimEnd();
				sb.Clear();

				if (i == commands.Length)
				{
					throw new MigrationParsingException();
				}

				continue;
			}

			sb.Append(s);
			sb.Append(Environment.NewLine);
		}

		if (!string.IsNullOrWhiteSpace(sb.ToString()))
		{
			commands[i] = sb.ToString().TrimEnd();
		}

		return (commands[0], commands[1]);
	}
}
