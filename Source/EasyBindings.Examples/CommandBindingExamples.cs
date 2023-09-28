using CommunityToolkit.Mvvm.Input;
using EasyBindings.Tests.Controls;

namespace EasyBindings.Examples;

public class CommandBindingExamples
{
    public void CommandWithoutParameter()
    {
        var greetCommand = new RelayCommand(() => Console.WriteLine("Hello, user!"));
        var button = new Button();

        CommandBindingService.Register(this, button, greetCommand);

        button.Press();
    }

    public void CommandWithParameter()
    {
        var greetCommand = new RelayCommand<string>
        (
            userName => Console.WriteLine($"Hello, {userName}!"),
            userName => string.IsNullOrWhiteSpace(userName) == false
        );

        var button = new Button();

        var userName = "user";

        CommandBindingService.Register(this, button, greetCommand, () => userName);

        button.Press();
        userName = "Ilnaz";

        button.Press();

        userName = null;
        greetCommand.NotifyCanExecuteChanged();

        button.Press();
    }

    public void GreetingUser()
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

        nameTextInput.Text = "Ilnaz";
        greetButton.Press();

        nameTextInput.Text = string.Empty;
        greetButton.Press();
    }
}