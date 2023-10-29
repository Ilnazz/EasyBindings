namespace EasyBindings.Interfaces;

/// <summary>
/// Provides a mechanism for unsubscribe object from all events it is subscribed to.
/// </summary>
public interface IUnsubscribe
{
    /// <summary>
    /// Performs unsubscribing object from all events to which it is subscribed.
    /// </summary>
    void Unsubscribe();
}