using EasyBindings.Interfaces;

namespace EasyBindings.Examples;

class PersonObserver : IUnsubscribe
{
    public void Observe(IChangeable changeable) =>
        TriggerBinder.OnPropertyChanged(this, changeable, o => o.State, () => Console.WriteLine($"{changeable}'s state was changed"));

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}