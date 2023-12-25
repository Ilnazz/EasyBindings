using EasyBindings.Interfaces;

namespace EasyBindings.Examples;

class PersonObserver : IDisposable
{
    public void Observe(INotifyStateChanged changeable) =>
        TriggerBinder.OnPropertyChanged(this, changeable, o => o.State, () => Console.WriteLine($"{changeable}'s state was changed"));

    public void Dispose() => TriggerBinder.Unbind(this);
}