using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using GeocodingClient.Api;
using GeocodingClient.Model;

namespace CefMap
{
    public partial class Form1 : Form
    {
        private static readonly string apiKey = ""; // Get your free api key at https://developer.myptv.com/
        private static LocationsApi locationsApi; 

        public Form1()
        {
            InitializeComponent();

            InitializeChromium();

            if (string.IsNullOrEmpty(apiKey))
                MessageBox.Show("You need an api key! Get your free key at https://developer.myptv.com/");

            locationsApi = new LocationsApi(new GeocodingClient.Client.Configuration
            {
                ApiKey = new Dictionary<string, string>
                {
                    ["apiKey"] = apiKey
                }
            });
        }

        private ChromiumWebBrowser browser;

        public void InitializeChromium()
        {
            // Create a browser component
            browser = new ChromiumWebBrowser(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                $"Map.html?apiKey={apiKey}"));

            // Add it to the form and fill it to the form window.
            this.mapPanel.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            browser.JavascriptMessageReceived += OnReceiveNotification;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = locationsApi.SearchLocationsByText(textBox1.Text);

            SetLocations(result.Locations);
        }

        private void OnReceiveNotification(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e == null)
                return;

            dynamic message = e.Message;
            switch (message.Event)
            {
                case "click":
                    // need to warp longitude (outside -180/+180)
                    var result = locationsApi.SearchLocationsByPosition(message.Lat, (message.Lng % 360 + 540) % 360 - 180);

                    // have to invoke it on the WinForms thread
                    if(InvokeRequired) 
                        Invoke(new Action(() => SetLocations(result.Locations)));
                    break;
            }
        }

        private void SetLocations(List<Location> locations)
        {
            dataGridView1.DataSource = locations;

//            browser.ShowDevTools();

            browser.GetMainFrame().ExecuteJavaScriptAsync(FormattableString.Invariant(
                $@"setLocations('{JsonSerializer.Serialize(locations)}')"));
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                button1_Click(sender, null);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
        }
    }
}
