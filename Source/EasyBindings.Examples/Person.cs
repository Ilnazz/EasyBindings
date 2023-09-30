using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;

namespace EasyBindings.Examples;

partial class Person : ObservableObject, IChangeable
{
    public object? State { get; }

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private string _name = string.Empty;

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private int _age;

    public override string ToString() => $"Person {{ Name = {Name}, Age = {Age} }}";
}
