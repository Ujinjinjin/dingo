namespace Dingo.UnitTests;

public struct TestStruct
{
	public string? Property1 { get; set; }
	public string? Property2 { get; set; }

	public TestStruct(string? property1, string? property2)
	{
		Property1 = property1;
		Property2 = property2;
	}
}
