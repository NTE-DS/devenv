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
using System.Linq;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extendability.Workbench;

namespace NasuTek.DevEnvironment.Documents
{
    public partial class WebBrowserDocument : DevEnvPane
    {
        public WebBrowserDocument()
        {
            InitializeComponent();
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        public void Navigate(string url)
        {
            Navigate(new Uri(url));
        }

        public void Navigate(Uri uri)
        {
            webBrowser1.Navigate(uri);
        }

        public string Url {
            get { return webBrowser1.Url.ToString(); }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
            Text = String.IsNullOrEmpty(webBrowser1.DocumentTitle) ? webBrowser1.Url.ToString() : webBrowser1.DocumentTitle;
            toolStripSpringTextBox1.Text = webBrowser1.Url.ToString();
        }

        public void Back() { webBrowser1.GoBack(); }
        public void Forward() { webBrowser1.GoForward(); }
        public void Stop() { webBrowser1.Stop(); }
        public void Refresh() { webBrowser1.Refresh(); }

        private int currentFont = 2;
        private int[] size = new int[] { 80, 90, 100, 200, 300 };

        public void FontSize() {
            if (currentFont != 4)
                currentFont++;
            else
                currentFont = 0;
            
            SetZoom(size[currentFont]);
        }

        private const int OLECMDID_ZOOM = 63;
        private const int OLECMDEXECOPT_DONTPROMPTUSER = 2;

        private void SetZoom(object zoom) {
            dynamic obj = webBrowser1.ActiveXInstance;

            obj.ExecWB(OLECMDID_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, zoom, IntPtr.Zero);
        }

        public void Print() {
            webBrowser1.ShowPrintDialog();
        }

        public void Copy() {
            
        }

        public string DocumentText {
            get { return webBrowser1.DocumentText; }
        }

        public Uri DocumentUri {
            get { return webBrowser1.Url; }
        }

        private void toolStripSpringTextBox1_KeyDown(object sender, KeyEventArgs e) {
        }

        private void toolStripSpringTextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != '\r') return;

            webBrowser1.Navigate(toolStripSpringTextBox1.Text);
            e.Handled = true;
        }
    }
}
