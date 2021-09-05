using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace CefMap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitializeChromium();
        }

        private ChromiumWebBrowser chromeBrowser;

        public void InitializeChromium()
        {           
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Map.html"));
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            //Wait for the MainFrame to finish loading
            chromeBrowser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    double latitude = 8.4044;
                    double longitude = 49.01405;

                    args.Frame.ExecuteJavaScriptAsync(FormattableString.Invariant($@"setMarker({latitude}, {longitude});"));
                }
            };
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            chromeBrowser.Dispose();
            Cef.Shutdown();
        }
     }
}
