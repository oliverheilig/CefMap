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
            CefSettings settings = new CefSettings();

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            Cef.EnableHighDPISupport();          
           
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Map.html"));
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
     }
}
