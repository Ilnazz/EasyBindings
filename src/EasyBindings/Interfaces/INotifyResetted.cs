using System.ComponentModel;

namespace EasyBindings.Interfaces;

/// <summary>
/// Notifies clients that a state has resetted.
/// </summary>
public interface INotifyResetted : INotifyPropertyChanged
{
    /// <summary>
    /// The property that will be passed to the <see cref='PropertyChangedEventArgs'/> constructor.
    /// </summary>
    object? Resetted { get; }
}
