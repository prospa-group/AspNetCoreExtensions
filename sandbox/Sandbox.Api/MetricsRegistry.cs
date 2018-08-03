using App.Metrics;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer;

namespace Sandbox.Api
{
    public static class MetricsRegistry
    {
        public const string ContextName = "Sandox.Api";

        public static readonly TimerOptions RandomTimer = new TimerOptions
        {
            Context = ContextName,
            Name = nameof(RandomTimer),
            Reservoir = () => new DefaultAlgorithmRReservoir()
        };

        public static readonly CounterOptions RandomCount = new CounterOptions
        {
            Context = ContextName,
            Name = nameof(RandomCount),
            ResetOnReporting = false,
            MeasurementUnit = Unit.Calls
        };

        public static readonly MeterOptions RandomRate = new MeterOptions
        {
            Context = ContextName,
            Name = nameof(RandomRate)
        };

        public static readonly HistogramOptions RandomHistogram = new HistogramOptions
        {
            Context = ContextName,
            Name = nameof(RandomHistogram),
            Reservoir = () => new DefaultSlidingWindowReservoir()
        };

        public static readonly ApdexOptions RandomApdex = new ApdexOptions
        {
            Context = ContextName,
            Name = nameof(RandomApdex),
            Reservoir = () => new DefaultForwardDecayingReservoir()
        };
    }
}