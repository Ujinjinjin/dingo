﻿using System;

 namespace Cliff.Infrastructure
{
	public interface IIocModule
	{
		IServiceProvider Build();
	}
}