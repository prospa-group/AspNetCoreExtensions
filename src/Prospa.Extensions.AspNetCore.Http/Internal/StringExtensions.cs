using System.Diagnostics;

// ReSharper disable CheckNamespace
namespace System
    // ReSharper restore CheckNamespace
{
    internal static class StringExtensions
    {
        [DebuggerStepThrough]
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static bool IsPresent(this string value)
        {
            return !value.IsMissing();
        }
    }
}
