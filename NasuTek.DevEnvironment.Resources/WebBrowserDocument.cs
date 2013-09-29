/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***************************************************************************************************/

using System.Text.RegularExpressions;
using NasuTek.DevEnvironment.Resources.Addins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Resources
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
