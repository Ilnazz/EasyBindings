# EasyBindings

## In Russian

### Что это?
EasyBindings - крохотная C# библиотека, которая позволяет осуществлять привязки триггеров, команд и свойств, как в WPF.

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
или используя консоль менеджера пакетов NuGet:
```sh
Install-Package EasyBindings
```

### Примеры использования


Вы можете найти больше примеров использования в проекте EasyBindings.Tests.

## In English

### What is it?
EasyBindings is a tiny C# library that allows you to bind triggers, commands and properties, as in WPF.

### What is it for?
EasyBindings makes it possible to use a WPF-specific mechanism for binding properties, triggers and commands outside of any MVVM framework.

### What led to the creation of EasyBindings?
When I started developing a near-game project on the Godot engine and having experience in developing WPF projects, I really missed having a binding mechanism.
I was looking for a library that would allow me to do similar things, but I didn't find it; so I decided to create one.

### Installation
You can install EasyBindings using dotnet command-line tool:
```sh
dotnet add package EasyBindings
```
or using NuGet package manager console:
```sh
Install-Package EasyBindings
```

### Examples
Classes used in examples:

```csharp
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

1. Bind trigger to property changed event:

```csharp
// Create the observable object
var person = new Person();

// Create the trigger that will display person's new name
TriggerBindingService.OnPropertyChanged(this, person, o => o.Name, newName =>
    Console.WriteLine($"Person's new name: {newName}"));

// Change person's name
person.Name = "Ilnaz"; // Console output: Person's new name: Ilnaz
```

You can find more examples in EasyBindings.Tests project