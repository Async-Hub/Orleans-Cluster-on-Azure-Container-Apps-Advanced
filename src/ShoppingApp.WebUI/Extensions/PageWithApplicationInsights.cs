using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace ShoppingApp.WebUI.Extensions;

public class PageWithApplicationInsights : ComponentBase
{
    [Inject]
    private TelemetryClient TelemetryClient { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    private Stopwatch? _sw;

    protected override void OnInitialized()
    {
        _sw = Stopwatch.StartNew();
        base.OnInitialized();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (_sw == null) return;

        _sw.Stop();
        TrackRequest(_sw);
    }

    private void TrackRequest(Stopwatch sw)
    {
        var dateTimeOffset = DateTimeOffset.UtcNow.AddMicroseconds(-sw.ElapsedMilliseconds);
        TelemetryClient.TrackRequest(NavigationManager.Uri,
            dateTimeOffset, sw.Elapsed, "200", true);
    }

    //private void TrackEvent(Stopwatch sw)
    //{
    //    var metrics = new Dictionary<string, double>
    //        {{"processingTime", sw.Elapsed.TotalMilliseconds}};

    //    var properties = new Dictionary<string, string>
    //        {{"pageName", NavigationManager.Uri}};

    //    TelemetryClient.TrackEvent("BlazorPageProcessed", properties, metrics);
    //}
}