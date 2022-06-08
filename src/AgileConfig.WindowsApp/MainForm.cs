using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace AgileConfig.WindowsApp
{
    public partial class MainForm : Form
    {
        private readonly CoreSettings _coreSettings;

        public MainForm(CoreSettings coreSettings)
        {
            InitializeComponent();
            this._coreSettings = coreSettings;
        }



        public async Task LoadUIAsync()
        {
            await Task.CompletedTask;
            void action()
            {
                string uri = _coreSettings?.Uri?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(uri))
                    return;
                this.webView2.CoreWebView2.Navigate(uri);
            };
            this.webView2.Invoke(action);
        }
        public async Task LoadWebView2CoreAsync()
        {
            if (this.webView2 is null)
            {
                return;
            }
            if (this.webView2.CoreWebView2 is not null)
            {
                return;
            }
            try
            {
                //download webview2 runtime from https://go.microsoft.com/fwlink/p/?LinkId=2124703
                //CoreWebView2Environment? env = await CoreWebView2Environment.CreateAsync(userDataFolder: @"./WebView2Data");
                await this.webView2.EnsureCoreWebView2Async();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private async void MainForm_ShownAsync(object sender, EventArgs e)
        {
            await LoadWebView2CoreAsync();

            this.webView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            CoreWebView2Settings settings = this.webView2.CoreWebView2.Settings;
            settings.AreDevToolsEnabled = true;

            await LoadUIAsync();
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            return;
        }
    }
}
