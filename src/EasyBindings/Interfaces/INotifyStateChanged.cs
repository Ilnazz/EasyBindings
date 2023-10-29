using System.ComponentModel;

namespace EasyBindings.Interfaces;

/// <summary>
/// Notifies clients that a state has changed.
/// </summary>
public interface INotifyStateChanged : INotifyPropertyChanged
{
    /// <summary>
    /// The property that will be passed to the <see cref='PropertyChangedEventArgs'/> constructor.
    /// </summary>
    object? State { get; }
}