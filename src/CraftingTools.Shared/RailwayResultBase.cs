namespace CraftingTools.Shared;

/// <summary>
/// Railway programming result base class.
/// </summary>
public abstract class RailwayResultBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the status parameter is <c>Unknown</c>.</exception>
    protected RailwayResultBase(RailwayResultStatus status, Error error)
    {
        this.Status = status == RailwayResultStatus.Unknown ? throw new ArgumentOutOfRangeException(nameof(status)) : status;
        this.Error = error;
    }
    
    /// <summary>
    /// Status of the result.
    /// </summary>
    public RailwayResultStatus Status { get; }
    
    /// <summary>
    /// Helper property for checking the success status.
    /// </summary>
    public bool IsSuccess => this.Status == RailwayResultStatus.Success;

    /// <summary>
    /// Helper property for checking the failure status.
    /// </summary>
    public bool IsFailure => this.Status == RailwayResultStatus.Failure;
    
    /// <summary>
    /// Gets the error instance.
    /// </summary>
    public Error Error { get; }
}