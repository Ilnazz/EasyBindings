using System;
using System.Runtime.Serialization;

namespace EasyBindings;

/// <summary>
/// The exception that is thrown when TODO: ...
/// </summary>
[Serializable]
public class BindingException : Exception
{
    public BindingException() : base() { }

    public BindingException(string? message) : base(message) {}

    public BindingException(string? message, Exception innerException) : base(message, innerException) { }

    protected BindingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}