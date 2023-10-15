using EasyBindings.Interfaces;

namespace EasyBindings.Tests.Controls;

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