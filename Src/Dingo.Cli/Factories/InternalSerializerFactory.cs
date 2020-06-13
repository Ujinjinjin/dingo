using Dingo.Cli.Constants;
using Dingo.Cli.Extensions;
using Dingo.Cli.Serializers;
using System;

namespace Dingo.Cli.Factories
{
	public class InternalSerializerFactory : IInternalSerializerFactory
	{
		public IInternalSerializer CreateInternalSerializer(string filename)
		{
			var fileExtension = filename
				.Split('.')
				.GetItem(-1);
			
			switch (fileExtension)
			{
				case FileExtension.Json:
					return new JsonInternalSerializer();
				case FileExtension.Yaml:
				case FileExtension.Yml:
					return new YamlInternalSerializer();
				default:
					throw new ArgumentOutOfRangeException(filename);
			}
		}
	}
}