/***************************************************************************************************
 * NasuTek Developer Studio Development Environment Core DLL
 * Copyright (C) 2005-2014 NasuTek Enterprises
 * Window Docking Portions Copyright (C) 2007-2012 Weifen Luo (email: weifenluo@yahoo.com)
 * Addin Engine Portions Copyright (C) 2001-2012 AlphaSierraPapa for the SharpDevelop Team
 *
 * This library is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Library General Public License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public License along with this 
 * library; if not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NasuTek.DevEnvironment.Extensibility.Addins.Services
{
	/// <summary>
	/// Maintains a list of services that can be shutdown in the reverse order of their initialization.
	/// Maintains references to the core service implementations.
	/// </summary>
	public abstract class ServiceManager : IServiceProvider
	{
		volatile static ServiceManager instance = new DefaultServiceManager();
		
		/// <summary>
		/// Gets the static ServiceManager instance.
		/// </summary>
		public static ServiceManager Instance {
			get { return instance; }
			set {
				if (value == null)
					throw new ArgumentNullException();
				instance = value;
			}
		}
		
		/// <summary>
		/// Gets a service. Returns null if service is not found.
		/// </summary>
		public abstract object GetService(Type serviceType);
		
		/// <summary>
		/// Gets a service. Returns null if service is not found.
		/// </summary>
		public T GetService<T>() where T : class
		{
			return GetService(typeof(T)) as T;
		}
		
		/// <summary>
		/// Gets a service. Throws an exception if service is not found.
		/// </summary>
		public object GetRequiredService(Type serviceType)
		{
			object service = GetService(serviceType);
			if (service == null)
				throw new ServiceNotFoundException();
			return service;
		}
		
		/// <summary>
		/// Gets a service. Throws an exception if service is not found.
		/// </summary>
		public T GetRequiredService<T>() where T : class
		{
			return (T)GetRequiredService(typeof(T));
		}
		
		/// <summary>
		/// Gets the logging service.
		/// </summary>
		public virtual ILoggingService LoggingService {
			get { return (ILoggingService)GetRequiredService(typeof(ILoggingService)); }
		}
		
		/// <summary>
		/// Gets the message service.
		/// </summary>
		public virtual IMessageService MessageService {
			get { return (IMessageService)GetRequiredService(typeof(IMessageService)); }
		}
	}
	
	sealed class DefaultServiceManager : ServiceManager
	{
		static ILoggingService loggingService = new TextWriterLoggingService(new DebugTextWriter());
		static IMessageService messageService = new TextWriterMessageService(Console.Out);
		
		public override ILoggingService LoggingService {
			get { return loggingService; }
		}
		
		public override IMessageService MessageService {
			get { return messageService; }
		}
		
		public override object GetService(Type serviceType)
		{
			if (serviceType == typeof(ILoggingService))
				return loggingService;
			else if (serviceType == typeof(IMessageService))
				return messageService;
			else
				return null;
		}
	}
}
