namespace EasyBindings.Interfaces;

/// <summary>
/// Represents an object that can change state
/// and notifies of its state change by generating
/// the PropertyChanged event (by implementing INotifyPropertyChanged interface)
/// with propertyName argument "State".
/// </summary>
public interface IChangeable
{
    /// <summary>
    /// A dummy property needed only to notify that the state of an object has changed by generating the PropertyChanged event.
    /// </summary>
    object? State { get; }
}
