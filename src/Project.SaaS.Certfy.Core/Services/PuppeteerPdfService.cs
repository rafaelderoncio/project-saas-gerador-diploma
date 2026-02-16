using Project.SaaS.Certfy.Core.Services.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Project.SaaS.Certfy.Core.Services;

public class PuppeteerPdfService : IPdfService, IAsyncDisposable
{
    private IBrowser _browser;
    private readonly SemaphoreSlim _browserLock = new(1, 1);
    private readonly LaunchOptions _browserOptions = new()
    {
            Headless = true,
            Args = [ 
                "--no-sandbox", 
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage", // p/ ambientes Docker/Linux
                "--font-render-hinting=none" 
            ]
        };

    private async Task EnsureBrowserAsync()
    {
        if (_browser is not null && !_browser.IsClosed && _browser.Process.HasExited == false)
            return;

        await _browserLock.WaitAsync();
        try
        {
            if (_browser is not null && !_browser.IsClosed && _browser.Process.HasExited == false)
                return;

            var fetcher = new BrowserFetcher();
            await fetcher.DownloadAsync();
            _browser = await Puppeteer.LaunchAsync(_browserOptions);
        }
        finally
        {
            _browserLock.Release();
        }
    }

    public async Task<byte[]> GenerateAsync(string template)
    {
        await EnsureBrowserAsync();

        try
        {
            await using var page = await _browser.NewPageAsync();

            await page.SetContentAsync(template, new NavigationOptions 
            { 
                WaitUntil = [WaitUntilNavigation.Networkidle0] 
            });

            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                Landscape = true,
                MarginOptions = new MarginOptions { Top = "0", Right = "0", Bottom = "0", Left = "0" }
            };

            return await page.PdfDataAsync(pdfOptions);
        }
        catch (Exception ex)
        {
            if (ex is TargetClosedException)
            {
                _browser = null;
            }
            throw new Exception("Falha ao gerar PDF via Puppeteer.", ex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
            await _browser.DisposeAsync();
            _browser = null;
        }
        _browserLock.Dispose();
    }
}