using System.Diagnostics.CodeAnalysis;

namespace BindingSharp.Core;

public class BindingException : Exception
{
    public object Object { [ExcludeFromCodeCoverage] get; }

    public BindingException(object obj, string message) : base(message)
    {
        Object = obj;
    }
}

