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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using NasuTek.DevEnvironment.Resources.Docking.Win32;

namespace NasuTek.DevEnvironment.Resources.Docking
{
    internal static class NativeMethods
    {
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DragDetect(IntPtr hWnd, Point pt);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, FlagsSetWindowPos flags);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowLong(IntPtr hWnd, int Index);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
        //*********************************
        // FxCop bug, suppress the message
        //*********************************
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
		public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(Win32.HookType code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr hhook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);
	}
}