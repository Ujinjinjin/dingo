namespace Dingo.Core.Utils;

internal readonly struct Timestamp
{
	private readonly DateTime _dateTime;
	private const string Format = "yyyyMMddHHmmss";

	private Timestamp(DateTime dateTime)
	{
		_dateTime = dateTime;
	}

	public static Timestamp New()
	{
		return new Timestamp(DateTime.UtcNow);
	}

	public override string ToString()
	{
		return _dateTime.ToString(Format);
	}
}
