using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CefMap
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CefRuntime.SubscribeAnyCpuAssemblyResolver();

            CefSettings settings = new CefSettings
            {
            };

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            Cef.EnableHighDPISupport();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
