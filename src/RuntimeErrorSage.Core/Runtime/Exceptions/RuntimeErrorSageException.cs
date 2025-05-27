using System;
using System.Runtime.Serialization;

namespace RuntimeErrorSage.Core.Runtime.Exceptions;

/// <summary>
/// Represents errors that occur during RuntimeErrorSage operations.
/// </summary>
[Serializable]
public class RuntimeErrorSageException : Exception
{
    public string ErrorCode { get; }
    public object Details { get; }
    public RuntimeErrorSageException() : base() { }
    public RuntimeErrorSageException(string message) : base(message) { }
    public RuntimeErrorSageException(string message, string errorCode) : base(message) { ErrorCode = errorCode; }
    public RuntimeErrorSageException(string message, Exception innerException) : base(message, innerException) { }
    public RuntimeErrorSageException(string message, string errorCode, Exception innerException) : base(message, innerException) { ErrorCode = errorCode; }
    public RuntimeErrorSageException(string message, string errorCode, object details) : base(message) { ErrorCode = errorCode; Details = details; }
    protected RuntimeErrorSageException(SerializationInfo info, StreamingContext context) : base(info, context) { ErrorCode = info.GetString(nameof(ErrorCode)); Details = info.GetValue(nameof(Details), typeof(object)); }
    public override void GetObjectData(SerializationInfo info, StreamingContext context) { base.GetObjectData(info, context); info.AddValue(nameof(ErrorCode), ErrorCode); info.AddValue(nameof(Details), Details); }
} 