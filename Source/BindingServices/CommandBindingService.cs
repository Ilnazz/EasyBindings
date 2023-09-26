using BindingServices.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BindingServices;

public static class CommandBindingService
{
    private class CommandBinding
    {
        public object Context { get; init; }

        public ICommandExecutor CommandExecutor { get; init; }

        public Action CommandExecutionRequestedEventHandler { get; init; }

        public ICommand Command { get; init; }
        
        public EventHandler CommanCanExecuteChangedEventHandler { get; init; }

        public CommandBinding
        (
            object context, ICommandExecutor commandExecutor,
            Action commandExecutionRequestedEventHandler,
            ICommand command, EventHandler commanCanExecuteChangedEventHandler)
        {
            Context = context;
            CommandExecutor = commandExecutor;
            CommandExecutionRequestedEventHandler = commandExecutionRequestedEventHandler;
            Command = command;
            CommanCanExecuteChangedEventHandler = commanCanExecuteChangedEventHandler;
        }
    }

    private static readonly IList<CommandBinding> _commandBindings = new List<CommandBinding>();

    public static void Register(object context, ICommandExecutor commandExecutor, ICommand command)
    {
        CheckRegistrationArgs(context, commandExecutor, command);
        CheckCanRegister(commandExecutor);

        RegisterCommandBinding(context,
            commandExecutor, () => command.Execute(null),
            command, (object? _, EventArgs _) => commandExecutor.CanExecuteCommand = command.CanExecute(null));

        commandExecutor.CanExecuteCommand = command.CanExecute(null);
    }

    public static void Register(object context, ICommandExecutor commandExecutor, ICommand command, Func<object> commandParameterGetter)
    {
        CheckRegistrationArgs(context, commandExecutor, command);
        ArgumentNullException.ThrowIfNull(commandParameterGetter, nameof(commandParameterGetter));
        CheckCanRegister(commandExecutor);

        RegisterCommandBinding(context,
            commandExecutor, () => command.Execute(commandParameterGetter()),
            command, (object? _, EventArgs _) => commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter()));

        commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter());
    }

    public static void Register<T>(object context, ICommandExecutor commandExecutor, IRelayCommand<T> command, Func<T> commandParameterGetter)
    {
        CheckRegistrationArgs(context, commandExecutor, command);
        ArgumentNullException.ThrowIfNull(commandParameterGetter, nameof(commandParameterGetter));
        CheckCanRegister(commandExecutor);

        RegisterCommandBinding(context,
            commandExecutor, () => command.Execute(commandParameterGetter()),
            command, (object? _, EventArgs _) => commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter()));

        commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter());
    }

    public static void Unregister(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _commandBindings.Where(cb => cb.Context == context).ToList().ForEach(Unregister);
    }

    public static void Unregister(ICommandExecutor commandExecutor)
    {
        ArgumentNullException.ThrowIfNull(commandExecutor, nameof(commandExecutor));

        var commandBinding = _commandBindings.FirstOrDefault(cb => cb.CommandExecutor == commandExecutor);
        if (commandBinding is not null)
            Unregister(commandBinding);
    }

    private static void Unregister(CommandBinding commandBinding)
    {
        commandBinding.CommandExecutor.CommandExecutionRequested -= commandBinding.CommandExecutionRequestedEventHandler;
        commandBinding.Command.CanExecuteChanged -= commandBinding.CommanCanExecuteChangedEventHandler;
        _commandBindings.Remove(commandBinding);
    }

    private static void RegisterCommandBinding
    (
        object context,
        ICommandExecutor commandExecutor, Action commandExecutionRequestedEventHandler,
        ICommand command, EventHandler commandCanExecuteChangedEventHandler)
    {
        commandExecutor.CommandExecutionRequested += commandExecutionRequestedEventHandler;
        command.CanExecuteChanged += commandCanExecuteChangedEventHandler;

        _commandBindings.Add(new CommandBinding
        (
            context,
            commandExecutor, commandExecutionRequestedEventHandler,
            command, commandCanExecuteChangedEventHandler
        ));
    }

    private static void CheckRegistrationArgs(object context, object commandExecutor, object command)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(commandExecutor, nameof(commandExecutor));
        ArgumentNullException.ThrowIfNull(command, nameof(command));
    }

    private static void CheckCanRegister(ICommandExecutor commandExecutor)
    {
        if (_commandBindings.Any(cb => cb.CommandExecutor == commandExecutor))
            throw new Exception($"{commandExecutor} is already bound with command");
    }
}
