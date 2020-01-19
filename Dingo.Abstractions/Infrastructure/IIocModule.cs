﻿using System;

 namespace Dingo.Abstractions.Infrastructure
{
	public interface IIocModule
	{
		IServiceProvider Build();
	}
}
