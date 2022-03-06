namespace CraftingTools.Shared;

/// <summary>
/// Immutable container class for error information.
/// </summary>
public sealed class Error
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <remarks>
    /// Scoped as internal to prevent callers outside the assembly from
    /// directly instantiating instance.  All instantiation should be handle
    /// from the extension methods in <see cref="ErrorExtensions"/>.
    /// </remarks>
    internal Error(string? message, Exception? exception)
    {
        this._message = message;
        this.Exception = exception;
    }
    
    /// <summary>
    /// Gets the message associated with the error.  If the message
    /// field is not null or empty, the message field is returned.  If
    /// the exception is not null, the exception message is returned.  Otherwise
    /// an empty string is returned. 
    /// </summary>
    public string Message =>
        string.IsNullOrEmpty(this._message)
            ? this.Exception is null
                ? string.Empty
                : this.Exception.Message
            : this._message;

    /// <summary>
    /// The exception associated with the error.
    /// </summary>
    public Exception? Exception { get; }

    public static readonly Error Empty = new(string.Empty, null);
    
    private readonly string? _message;
}