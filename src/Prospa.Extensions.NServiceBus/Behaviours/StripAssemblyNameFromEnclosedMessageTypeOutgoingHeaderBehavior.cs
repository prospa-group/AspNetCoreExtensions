using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace Prospa.Extensions.NServiceBus.Behaviours
{
    public class StripAssemblyNameFromEnclosedMessageTypeOutgoingHeaderBehavior : Behavior<IOutgoingPhysicalMessageContext>
    {
        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            var headers = context.Headers;

            var currentType = headers["NServiceBus.EnclosedMessageTypes"];
            var newType = currentType.Substring(0, currentType.IndexOf(','));

            headers["NServiceBus.EnclosedMessageTypes"] = newType;

            return next();
        }
    }
}