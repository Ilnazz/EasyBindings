using System.ComponentModel;

namespace EasyBindings.Interfaces;

/// <summary>
/// Notifies clients that a state has resetted.
/// </summary>
public interface IResettable : INotifyPropertyChanged
{
    /// <summary>
    /// The property that will be passed to the <see cref='PropertyChangedEventArgs'/> constructor.
    /// </summary>
    public object? Resetted { get; }
}
