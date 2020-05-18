using System;
using Microsoft.AspNetCore.Mvc;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Routing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
#pragma warning disable SA1402 // File may only contain a single class
#pragma warning disable SA1649 // File must match class name
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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class V3Attribute : ApiVersionAttribute
    {
        public V3Attribute()
            : base("3") { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class V4Attribute : ApiVersionAttribute
    {
        public V4Attribute()
            : base("4") { }
    }
#pragma warning restore SA1649 // File must match class name
#pragma warning restore SA1402 // File may only contain a single class
}
