﻿using Dingo.Core.Constants;
using Dingo.Core.Extensions;
using Dingo.Core.Serializers;
using System;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	public class InternalSerializerFactory : IInternalSerializerFactory
	{
		/// <inheritdoc />
		public IInternalSerializer CreateInternalSerializer(string filename)
		{
			var fileExtension = filename
				.Split('.')
				.GetItem(^1);

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