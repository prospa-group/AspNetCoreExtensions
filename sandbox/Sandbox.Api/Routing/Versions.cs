using System;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Api.Routing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
#pragma warning disable SA1402 // File may only contain a single class
    public sealed class V1Attribute : ApiVersionAttribute
    {
        public V1Attribute()
            : base("1") { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class V2Attribute : ApiVersionAttribute
    {
        public V2Attribute()
            : base("2") { }
    }
#pragma warning restore SA1402 // File may only contain a single class
}
