# EasyBindings

## In Russian

### Что это?
EasyBindings - крохотная C# библиотека, которая позволяет осуществлять привязки триггеров, свойств и команд, как в WPF.

### Для чего это нужно?
EasyBindings даёт возможность использовать специфичный для WPF механизм привязок свойств, триггеров и команд за пределами какого-либо MVVM фреймворка.

### Что привело к созданию EasyBindings?
Когда я начал разрабатывать около-игровой проект на движке Godot и имея опыт разработки WPF-проектов, мне очень не хватало наличия механизма привязок.
Я искал библиотеку, которая позволила бы делать подобные вещи, но не нашёл; поэтому я решил её создать.

### Установка
Вы можете установить EasyBindings через командную строку, используя инструмент dotnet:
```sh
dotnet add package EasyBindings
```
или используя консоль менеджера пакетов [NuGet](https://www.nuget.org/packages/EasyBindings):
```sh
PM> Install-Package EasyBindings
```

## In English

### What is it?
EasyBindings is a tiny C# library that allows you perform trigger, property and command bindings, as in WPF.

### What is it for?
EasyBindings makes it possible to use a WPF-specific mechanism for binding properties, triggers and commands outside of any MVVM framework.

### What led to the creation of EasyBindings?
When I started developing a near-game project on the Godot engine and having experience in developing WPF projects, I really missed having a binding mechanism.
I was looking for a library that would allow me to do similar things, but I didn't find it; so I decided to create one.

### Installation
You can install EasyBindings using dotnet command-line tool:
```sh
> dotnet add package EasyBindings
```
or using [NuGet](https://www.nuget.org/packages/EasyBindings) package manager console:
```sh
PM> Install-Package EasyBindings
```

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

#### Creating trigger bindings
- Bind trigger to property changed event:
```csharp
// Create the observable object
var person = new Person();

// Create the trigger that will display person's new name
TriggerBindingService.OnPropertyChanged(this, person, o => o.Name, newName =>
    Console.WriteLine($"Person's new name: {newName}"));

// Change person's name
person.Name = "Ilnaz"; // Console output: Person's new name: Ilnaz
```

- Bind trigger to property changing event:
```csharp
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
```

- Bind trigger to collection changed event:
```csharp
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
```

#### Creating property binding
```csharp
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
```

#### Using IChangeable interface
```csharp
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
```

#### Using IUnsubscribe interface
```csharp
...
```

You can find more examples in EasyBindings.Tests project.