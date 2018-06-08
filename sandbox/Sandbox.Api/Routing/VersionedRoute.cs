using System;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Api.Routing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class VersionedRoute : RouteAttribute
    {
        private const string Prefix = "v{version:apiVersion}/";

        public VersionedRoute(string template)
            : base($"{Prefix}{template}")
        {
        }
    }
}