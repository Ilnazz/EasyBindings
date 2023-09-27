using EasyBindings.Interfaces;

namespace EasyBindings.Test.Controls;

public class Button : ICommandExecutor
{
    public bool CanExecuteCommand { get; set; }

    public event Action? CommandExecutionRequested;

    public void Press()
    {
        if (CanExecuteCommand)
        {
            Console.WriteLine("Button can't be pressed.");
            return;
        }

        Console.WriteLine("Button pressed.");
        CommandExecutionRequested?.Invoke();
    }
}