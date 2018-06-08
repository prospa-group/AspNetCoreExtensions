using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Api.Routing;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class SampleMetricsController : ControllerBase
    {
        private static readonly Random Rnd = new Random();

        private readonly IMetrics _metrics;

        public SampleMetricsController(IMetrics metrics) { _metrics = metrics; }

        [HttpGet("timer")]
        public async Task<ActionResult> GetTimer()
        {
            using (_metrics.Measure.Timer.Time(MetricsRegistry.RandomTimer))
            {
                await Task.Delay(Rnd.Next(300), HttpContext.RequestAborted);
            }

            return Ok();
        }

        [HttpGet("counter")]
        public ActionResult GetCounter()
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry.RandomCount, Rnd.Next(10));

            return Ok();
        }

        [HttpGet("meter")]
        public ActionResult GetMeter()
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.RandomRate, Rnd.Next(100));

            return Ok();
        }

        [HttpGet("histogram")]
        public ActionResult GetHistogram()
        {
            foreach (var i in Enumerable.Range(1, 3))
            {
                var tags = new MetricTags($"key{i}", $"value{i}");

                _metrics.Measure.Histogram.Update(MetricsRegistry.RandomHistogram, tags, Rnd.Next(1, 100));
                _metrics.Measure.Histogram.Update(MetricsRegistry.RandomHistogram, tags, Rnd.Next(1, 100));
                _metrics.Measure.Histogram.Update(MetricsRegistry.RandomHistogram, tags, Rnd.Next(1, 100));
            }

            return Ok();
        }

        [HttpGet("apdex")]
        public async Task<ActionResult> GetApdex()
        {
            using (_metrics.Measure.Apdex.Track(MetricsRegistry.RandomApdex))
            {
                await Task.Delay(Rnd.Next(300), HttpContext.RequestAborted);
            }

            return Ok();
        }
    }
}