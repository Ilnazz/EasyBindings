using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings.Interfaces;
using EasyBindings.Tests.Controls;
using System.Collections.ObjectModel;

namespace EasyBindings.Examples;

public class Program
{
    public static void Main()
    {
        var programInstance = new Program();

        programInstance.PropertyBindingExample();
    }

    void PropertyChangedTriggerBindingExample()
    {
        // Create the observable object
        var person = new Person();

        // Create the trigger that will display person's new name
        TriggerBindingService.OnPropertyChanged(this, person, o => o.Name, newName =>
            Console.WriteLine($"Person's new name: {newName}"));

        // Change person's name
        person.Name = "Ilnaz"; // Console output: Person's new name: Ilnaz
    }

    void PropertyChangingTriggerBindingExample()
    {
        // Create the observable object with initial property value
        var person = new Person
        {
            Name = "Alfred"
        };

        // Create the trigger that will display person's name before it will be changed
        TriggerBindingService.OnPropertyChanging(this, person, o => o.Name, currentName =>
            Console.WriteLine($"Person's name will be changed; current name: {currentName}"));

        // Change person's name
        person.Name = "Ilnaz"; // Console output: Person's name will be changed; current name: Alfred
    }

    void CollectionChangedTriggerBindingExample()
    {
        // Create the observable collection
        var numbers = new ObservableCollection<int>();
        // Create the variable that stores sum of the numbers
        var numbersSum = 0;

        // Create the trigger that will update numberSum variable when numbers collection will change
        TriggerBindingService.OnCollectionChanged(this, numbers, () => numbersSum = numbers.Sum());

        // Add some numbers to collection
        numbers.Add(10); // numbersSum = 10
        numbers.Add(20); // numbersSum = 30
        numbers.Add(-30); // numbersSum = 0
    }

    void PropertyBindingExample()
    {
        // Create the observable object
        var person = new Person();

        // Create dummy text inputs to input text
        var nameTextInput = new TextInput();
        var ageTextInput = new TextInput();

        // Bind person.Name to nameTextInput.Text
        PropertyBindingService.BindOneWay(this, person, t => t.Name, nameTextInput, s => s.Text);
        // Bind person.Name to ageTextInput.Text using value converter
        PropertyBindingService.BindOneWay(this, person, t => t.Age, ageTextInput, s => s.Text,
            ageText => int.TryParse(ageText, out var age) ? age : 0);

        // Enter text
        nameTextInput.Text = "Ilnaz"; // person.Name also will be set to Ilnaz
        ageTextInput.Text = "20"; // person.Age also will be set to 20
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
        TriggerBindingService.OnPropertyChanged(this, nameTextInput, o => o.Text, greetCommand.NotifyCanExecuteChanged);

        // Create the button that will execute a command
        var greetButton = new Button();

        // Bind the command to the button
        CommandBindingService.Bind(this, greetButton, greetCommand, () => nameTextInput.Text);

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
        TriggerBindingService.OnPropertyChanged(this, person, o => o.State, () =>
            Console.WriteLine($"Person's state was changed: Name: {person.Name}, Age: {person.Age}."));

        // Change person's properties
        person.Name = "Ilnaz"; // Console output: Person's state was changed: Name: Ilnaz, Age: 0.
        person.Age = 20; // Console output: Person's state was changed: Name: Ilnaz, Age: 20.

        // Don't forget to unbind trigger
        TriggerBindingService.UnbindPropertyChanged(this, person);
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