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
using System.IO;

namespace NasuTek.DevEnvironment.Extensibility.Addins.Services
{
	/// <summary>
	/// LoggingService implementation that logs into a TextWriter.
	/// </summary>
	public class TextWriterLoggingService : ILoggingService
	{
		readonly TextWriter writer;
		
		public TextWriterLoggingService(TextWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			this.writer = writer;
			this.IsFatalEnabled = true;
			this.IsErrorEnabled = true;
			this.IsWarnEnabled = true;
			this.IsInfoEnabled = true;
			this.IsDebugEnabled = true;
		}
		
		void Write(object message, Exception exception)
		{
			if (message != null) {
				writer.WriteLine(message.ToString());
			}
			if (exception != null) {
				writer.WriteLine(exception.ToString());
			}
		}
		
		public bool IsDebugEnabled { get; set; }
		public bool IsInfoEnabled { get; set; }
		public bool IsWarnEnabled { get; set; }
		public bool IsErrorEnabled { get; set; }
		public bool IsFatalEnabled { get; set; }
		
		public void Debug(object message)
		{
			if (IsDebugEnabled) {
				Write(message, null);
			}
		}
		
		public void DebugFormatted(string format, params object[] args)
		{
			Debug(string.Format(format, args));
		}
		
		public void Info(object message)
		{
			if (IsInfoEnabled) {
				Write(message, null);
			}
		}
		
		public void InfoFormatted(string format, params object[] args)
		{
			Info(string.Format(format, args));
		}
		
		public void Warn(object message)
		{
			Warn(message, null);
		}
		
		public void Warn(object message, Exception exception)
		{
			if (IsWarnEnabled) {
				Write(message, exception);
			}
		}
		
		public void WarnFormatted(string format, params object[] args)
		{
			Warn(string.Format(format, args));
		}
		
		public void Error(object message)
		{
			Error(message, null);
		}
		
		public void Error(object message, Exception exception)
		{
			if (IsErrorEnabled) {
				Write(message, exception);
			}
		}
		
		public void ErrorFormatted(string format, params object[] args)
		{
			Error(string.Format(format, args));
		}
		
		public void Fatal(object message)
		{
			Fatal(message, null);
		}
		
		public void Fatal(object message, Exception exception)
		{
			if (IsFatalEnabled) {
				Write(message, exception);
			}
		}
		
		public void FatalFormatted(string format, params object[] args)
		{
			Fatal(string.Format(format, args));
		}
	}
}
