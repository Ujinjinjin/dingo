﻿using System;

 namespace Dingo.Abstractions
{
	public interface IContainer
	{
		IServiceProvider Build();
	}
}
