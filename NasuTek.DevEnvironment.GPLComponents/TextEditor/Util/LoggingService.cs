// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision$</version>
// </file>

using System;

namespace NasuTek.DevEnvironment.Extendability.Workbench.TextEditor.Util
{
	/// <summary>
	/// Central location for logging calls in the text editor.
	/// </summary>
	static class LoggingService
	{
		public static void Debug(string text)
		{
			#if DEBUG
			Console.WriteLine(text);
			#endif
		}
	}
}