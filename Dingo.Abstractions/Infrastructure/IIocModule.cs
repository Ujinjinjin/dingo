﻿using System;

 namespace Dingo.Abstractions
{
	public interface IIocModule
	{
		IServiceProvider Build();
	}
}
