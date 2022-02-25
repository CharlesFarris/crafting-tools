using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CraftingTools.Shared;

/// <summary>
/// Railway programming result base class.
/// </summary>
public abstract class ResultBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the status parameter is <c>Unknown</c>.</exception>
    protected ResultBase(ResultStatus status)
    {
        this.Status = status == ResultStatus.Unknown ? throw new ArgumentOutOfRangeException(nameof(status)) : status;
    }
    
    /// <summary>
    /// Status of the result.
    /// </summary>
    public ResultStatus Status { get; }
    
    /// <summary>
    /// Helper property for checking the success status.
    /// </summary>
    public bool IsSuccess => this.Status == ResultStatus.Success;

    /// <summary>
    /// Helper property for checking the failure status.
    /// </summary>
    public bool IsFailure => this.Status == ResultStatus.Failure;
}