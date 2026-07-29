
using System.Diagnostics;
using System.Formats.Asn1;

using var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));

Console.WriteLine("sequential execution");

var sequentialWatch = Stopwatch.StartNew();

try
{
    await DownloadReportAsync(cts.Token);
    await DownloadInvoiceAsync();
    await DownloadSummryAsync();
}

catch (OperationCanceledException)
{
    Console.WriteLine("Sequential oparation was cancelled");
}

sequentialWatch.Stop();
Console.WriteLine($"sequential time: {sequentialWatch.ElapsedMilliseconds}ms\n");
Console.WriteLine("parallel execution:");

var parallelWatch= Stopwatch.StartNew();

try
{
    await Task.WhenAll(
        DownloadReportAsync(cts.Token),
        DownloadInvoiceAsync(),
        DownloadSummryAsync()
    );
}

catch (OperationCanceledException)
{
    Console.WriteLine($"parallel oparation was cancelled");
}

parallelWatch.Stop();
Console.WriteLine($"parallel time: {parallelWatch.ElapsedMilliseconds} ms");

static async Task DownloadReportAsync(CancellationToken token)
{
    Console.WriteLine("starting download : report.pdf");
    var delay= Random.Shared.Next(1000,3000);
    await Task.Delay(delay, token);

    Console.WriteLine("finished dwonload: report.pdf");
   
}

static async Task DownloadInvoiceAsync()
{
    Console.WriteLine("Starting download: invoice.pdf" );

    var delay= Random.Shared.Next(1000, 3000);
    await Task.Delay(delay);

    Console.WriteLine($"Finished download: invoice.pdf");
    
}

static async Task DownloadSummryAsync()
{
    Console.WriteLine("starting dwonload: summry.pdf");

    var delay= Random.Shared.Next(1000,3000);
    await Task.Delay(delay);

    Console.WriteLine("finishd download: summary.docx");
}