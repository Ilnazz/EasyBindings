# EasyBindings

## In English

### UPD:
At the time of the development of this library, I was unaware of the existence of libraries such as [Rx.NET](https://github.com/dotnet/reactive) and [ReactiveUI](https://github.com/reactiveui/ReactiveUI), which allow you to do much more (and much better) than EasyBindings.
Due to the existence of the mentioned libraries, this library becomes a public archive.

### What is it?
EasyBindings is a tiny C# library that allows you perform trigger, property and command bindings, as in WPF.

### What is it for?
EasyBindings makes it possible to use a WPF-specific mechanism for binding properties, triggers and commands outside of any MVVM framework.

### What led to the creation of EasyBindings?
When I started developing a near-game project on the Godot engine and having experience in developing WPF projects, I really missed having a binding mechanism.
I was looking for a library that would allow me to do similar things, but I didn't find it; so I decided to create one.

### Additional
- The library code is documented and tested.
- EasyBindings uses the official Community Toolkit.Mvvm library from Microsoft,
which provides tools for developing applications using the MVVM pattern.

### Examples
#### Classes used in examples
- TextInput
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

public partial class TextInput : ObservableObject
{
    [ObservableProperty]
    private string _text = string.Empty;
}
```

- Button
```csharp
using System;
using EasyBindings.Interfaces;

public class Button : ICommandExecutor
{
    public bool CanExecuteCommand { get; set; }

    public event Action? CommandExecutionRequested;

    public void Press()
    {
        if (CanExecuteCommand == false)
        {
            Console.WriteLine("Button can't be pressed.");
            return;
        }

        Console.WriteLine("Button was pressed.");
        CommandExecutionRequested?.Invoke();
    }
}
```

- Person
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

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
```

- PersonObserver
```csharp
using EasyBindings.Interfaces;

class PersonObserver : IUnsubscribe
{
    public void Observe(IChangeable changeable) =>
        TriggerBinder.OnPropertyChanged(this, changeable, o => o.State, () => Console.WriteLine($"{changeable}'s state was changed"));

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}
```

#### Creating trigger bindings
- Bind trigger to property changed event:
```csharp
// Create the observable object
var person = new Person();

// Create dummy text inputs to input text
var nameTextInput = new TextInput();
var ageTextInput = new TextInput();

// Bind person.Name to nameTextInput.Text
PropertyBinder.BindOneWay(this, person, t => t.Name, nameTextInput, s => s.Text);
// Bind person.Name to ageTextInput.Text using value converter
PropertyBinder.BindOneWay(this, person, t => t.Age, ageTextInput, s => s.Text,
    ageText => int.TryParse(ageText, out var age) ? age : 0);

// Enter text
nameTextInput.Text = "Ilnaz"; // person.Name also will be set to Ilnaz
ageTextInput.Text = "20"; // person.Age also will be set to 20
```

- Bind trigger to property changing event:
```csharp
// Create the observable object with initial property value
var person = new Person
{
    Name = "Alfred"
};

// Create the trigger that will display person's name before it will be changed
TriggerBinder.OnPropertyChanging(this, person, o => o.Name, currentName =>
    Console.WriteLine($"Person's name will be changed; current name: {currentName}"));

// Change person's name
person.Name = "Ilnaz"; // Console output: Person's name will be changed; current name: Alfred
```

- Bind trigger to collection changed event:
```csharp
// Create the observable collection
var numbers = new ObservableCollection<int>();
// Create the variable that stores sum of the numbers
var numbersSum = 0;

// Create the trigger that will update numberSum variable when numbers collection will change
TriggerBinder.OnCollectionChanged(this, numbers, () => numbersSum = numbers.Sum());

// Add some numbers to collection
numbers.Add(10); // numbersSum = 10
numbers.Add(20); // numbersSum = 30
numbers.Add(-30); // numbersSum = 0
```

#### Creating property binding
```csharp
// Create the observable object
var person = new Person();

// Create dummy text inputs to input text
var nameTextInput = new TextInput();
var ageTextInput = new TextInput();

// Bind person.Name to nameTextInput.Text
PropertyBinder.BindOneWay(this, person, t => t.Name, nameTextInput, s => s.Text);
// Bind person.Name to ageTextInput.Text using value converter
PropertyBinder.BindOneWay(this, person, t => t.Age, ageTextInput, s => s.Text,
    ageText => int.TryParse(ageText, out var age) ? age : 0);

// Enter text
nameTextInput.Text = "Ilnaz"; // person.Name also will be set to Ilnaz
ageTextInput.Text = "20"; // person.Age also will be set to 20
```


#### Creating command binding
```csharp
// Create the command to greet user with the given name
var greetCommand = new RelayCommand<string>
(
    execute: userName => Console.WriteLine($"Hello, {userName}!"),
    canExecute: userName => string.IsNullOrWhiteSpace(userName) == false
);

// Create the text input to enter name
var nameTextInput = new TextInput();
// Create the trigger that will notify command execution ability is changed
TriggerBinder.OnPropertyChanged(this, nameTextInput, o => o.Text, greetCommand.NotifyCanExecuteChanged);

// Create the button that will execute a command
var greetButton = new Button();

// Bind the command to the button
CommandBinder.Bind(this, greetButton, greetCommand, () => nameTextInput.Text);

// Change textInput's text
nameTextInput.Text = "Ilnaz";
greetButton.Press(); // Console output: Button was pressed.\nHello, Ilnaz!

nameTextInput.Text = string.Empty;
greetButton.Press(); // Console output: Button can't be pressed.
```

#### Using IChangeable interface
```csharp
// Create the new person object
var person = new Person();

// Bind the trigger to person's state property changed event
TriggerBinder.OnPropertyChanged(this, person, o => o.State, () =>
    Console.WriteLine($"Person's state was changed: Name: {person.Name}, Age: {person.Age}."));

// Change person's properties
person.Name = "Ilnaz"; // Console output: Person's state was changed: Name: Ilnaz, Age: 0.
person.Age = 20; // Console output: Person's state was changed: Name: Ilnaz, Age: 20.

// Don't forget to unbind trigger
TriggerBinder.UnbindPropertyChanged(this, person);
```

#### Using IUnsubscribe interface
```csharp
// Create two person objects
Person person1 = new(),
        person2 = new();

// Create the person observer object
var personObserver = new PersonObserver();
// Start observing persons
personObserver.Observe(person1);
personObserver.Observe(person2);

// Change persons properties
person1.Name = "Ilnaz"; // Console output: Person { Name = Ilnaz, Age = 0 }'s state was changed
person1.Age = 20; // Console output: Person { Name = Ilnaz, Age = 20 }'s state was changed

person2.Name = "Alfred"; // Console output: Person { Name = Alfred, Age = 0 }'s state was changed

// When you are not more need in object state changed notifier object,
// you should call unsubscribe method to clean up subscriptions
personObserver.Unsubscribe();
```

You can find more examples in EasyBindings.Tests project.
