using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings.Interfaces;
using EasyBindings.Tests.Controls;

namespace EasyBindings.Examples;

public class Program
{
    public static void Main()
    {
        var programInstance = new Program();

        programInstance.IChangeableExample();
    }

    void TriggerBindingExample()
    {

    }

    void PropertyBindingExample()
    {

    }

    void CommandBindingExample()
    {
        // Create the command to greet user with the given name
        var greetCommand = new RelayCommand<string>
        (
            execute: userName => Console.WriteLine($"Hello, {userName}!"),
            canExecute: userName => string.IsNullOrWhiteSpace(userName) == false
        );

        // Create the text input to enter name
        var nameTextInput = new TextInput();
        // Create the trigger that will notify command execution ability is changed
        TriggerBindingService.RegisterPropertyChanged(this, nameTextInput, o => o.Text, greetCommand.NotifyCanExecuteChanged);

        // Create the button that will execute a command
        var greetButton = new Button();

        // Bind the command to the button
        CommandBindingService.Register(this, greetButton, greetCommand, () => nameTextInput.Text);

        // Change textInput's text
        nameTextInput.Text = "Ilnaz";
        greetButton.Press(); // Console output: Button was pressed.\nHello, Ilnaz!

        nameTextInput.Text = string.Empty;
        greetButton.Press(); // Console output: Button can't be pressed.
    }

    void IChangeableExample()
    {
        // Create the new person object
        var person = new Person();

        // Bind the trigger to person's state property changed event
        TriggerBindingService.RegisterPropertyChanged(this, person, o => o.State, () =>
            Console.WriteLine($"Person's state was changed: Name: {person.Name}, Age: {person.Age}."));

        // Change person's properties
        person.Name = "Ilnaz"; // Console output: Person's state was changed: Name: Ilnaz, Age: 0.
        person.Age = 20; // Console output: Person's state was changed: Name: Ilnaz, Age: 20.

        // Don't forget to unbind trigger
        TriggerBindingService.UnregisterPropertyChanged(this, person);
    }

    void IUnsubscribeExample()
    {
        // On what example can I demonstrate IUnsubscribe using?
        // You can refer to ResearchProject - one of options
        // It will be good to refer to ResearchProject to show whole using this library
        // Plus: add to readme project goals and about project
    }
}

partial class Person : ObservableObject, IChangeable
{
    public object? State { get; }

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private string _name = string.Empty;

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private int _age;
}