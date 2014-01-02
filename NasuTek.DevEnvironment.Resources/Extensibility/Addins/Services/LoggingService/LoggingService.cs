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
using NasuTek.DevEnvironment.Extensibility.Addins.Services;

namespace NasuTek.DevEnvironment.Extensibility.Addins
{
	/// <summary>
	/// Class for easy logging.
	/// </summary>
	public static class LoggingService
	{
		public static void Debug(object message)
		{
			ServiceManager.Instance.LoggingService.Debug(message);
		}
		
		public static void DebugFormatted(string format, params object[] args)
		{
			ServiceManager.Instance.LoggingService.DebugFormatted(format, args);
		}
		
		public static void Info(object message)
		{
			ServiceManager.Instance.LoggingService.Info(message);
		}
		
		public static void InfoFormatted(string format, params object[] args)
		{
			ServiceManager.Instance.LoggingService.InfoFormatted(format, args);
		}
		
		public static void Warn(object message)
		{
			ServiceManager.Instance.LoggingService.Warn(message);
		}
		
		public static void Warn(object message, Exception exception)
		{
			ServiceManager.Instance.LoggingService.Warn(message, exception);
		}
		
		public static void WarnFormatted(string format, params object[] args)
		{
			ServiceManager.Instance.LoggingService.WarnFormatted(format, args);
		}
		
		public static void Error(object message)
		{
			ServiceManager.Instance.LoggingService.Error(message);
		}
		
		public static void Error(object message, Exception exception)
		{
			ServiceManager.Instance.LoggingService.Error(message, exception);
		}
		
		public static void ErrorFormatted(string format, params object[] args)
		{
			ServiceManager.Instance.LoggingService.ErrorFormatted(format, args);
		}
		
		public static void Fatal(object message)
		{
			ServiceManager.Instance.LoggingService.Fatal(message);
		}
		
		public static void Fatal(object message, Exception exception)
		{
			ServiceManager.Instance.LoggingService.Fatal(message, exception);
		}
		
		public static void FatalFormatted(string format, params object[] args)
		{
			ServiceManager.Instance.LoggingService.FatalFormatted(format, args);
		}
		
		public static bool IsDebugEnabled {
			get {
				return ServiceManager.Instance.LoggingService.IsDebugEnabled;
			}
		}
		
		public static bool IsInfoEnabled {
			get {
				return ServiceManager.Instance.LoggingService.IsInfoEnabled;
			}
		}
		
		public static bool IsWarnEnabled {
			get {
				return ServiceManager.Instance.LoggingService.IsWarnEnabled;
			}
		}
		
		public static bool IsErrorEnabled {
			get {
				return ServiceManager.Instance.LoggingService.IsErrorEnabled;
			}
		}
		
		public static bool IsFatalEnabled {
			get {
				return ServiceManager.Instance.LoggingService.IsFatalEnabled;
			}
		}
	}
}
