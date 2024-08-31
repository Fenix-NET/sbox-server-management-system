using System.Diagnostics.Metrics;

namespace Infrastructure.Monitoring;

public static class PrometheusMetrics
{
    private static readonly Counter ProcessedOrderCounter = PrometheusMetrics
        .CreateCounter("processed_order_total", "Total number of processed orders");

    public static void IncrementProcessedOrders()
    {
        ProcessedOrderCounter.Inc();
    }
}