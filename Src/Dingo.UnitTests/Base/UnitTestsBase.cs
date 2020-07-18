using AutoFixture;
using AutoFixture.AutoMoq;
using Dingo.Core.Extensions;
using Moq;
using System.Collections.Generic;

namespace Dingo.UnitTests.Base
{
	public class UnitTestsBase
	{
		protected IFixture CreateFixture()
		{
			return new Fixture().Customize(new AutoMoqCustomization());
		}
		
		protected IFixture CreateFixture<T1>(Mock<T1> T1Value) 
			where T1 : class
		{
			var fixture = new Fixture().Customize(new AutoMoqCustomization());
			
			fixture.Register(() => T1Value.Object);

			return fixture;
		}
		
		protected IFixture CreateFixture<T1, T2>(Mock<T1> T1Value, Mock<T2> T2Value) 
			where T1 : class 
			where T2 : class
		{
			var fixture = new Fixture().Customize(new AutoMoqCustomization());
			
			fixture.Register(() => T1Value.Object);
			fixture.Register(() => T2Value.Object);

			return fixture;
		}
		
		protected IFixture CreateFixture<T1, T2, T3>(Mock<T1> T1Value, Mock<T2> T2Value, Mock<T3> T3Value) 
			where T1 : class 
			where T2 : class
			where T3 : class
		{
			var fixture = new Fixture().Customize(new AutoMoqCustomization());
			
			fixture.Register(() => T1Value.Object);
			fixture.Register(() => T2Value.Object);
			fixture.Register(() => T3Value.Object);

			return fixture;
		}
		
		protected IFixture CreateFixture<T1, T2, T3, T4>(Mock<T1> T1Value, Mock<T2> T2Value, Mock<T3> T3Value, Mock<T4> T4Value) 
			where T1 : class 
			where T2 : class
			where T3 : class
			where T4 : class
		{
			var fixture = new Fixture().Customize(new AutoMoqCustomization());
			
			fixture.Register(() => T1Value.Object);
			fixture.Register(() => T2Value.Object);
			fixture.Register(() => T3Value.Object);
			fixture.Register(() => T4Value.Object);

			return fixture;
		}
		
		protected IList<int> CreateIntArray(int length)
		{
			if (length < 0)
			{
				length = length.Negate();
			}
			
			var array = new int[length];
			
			for (var i = 0; i < length; i++)
			{
				array[i] = i;
			}

			return array;
		}
	}
}
