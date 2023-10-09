using EasyBindings.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EasyBindings;

/// <summary>
/// Allows you to bind a <see cref="ICommand"/>s to a <see cref="ICommandExecutor"/>s.
/// </summary>
public static class CommandBinder
{
    private static readonly IList<CommandBinding> _commandBindings = new List<CommandBinding>();

    #region Public methods
    #region Binding methods
    /// <summary>
    /// Binds <paramref name="command"/> to a <paramref name="commandExecutor"/> in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="commandExecutor">The command executor that will execute the command.</param>
    /// <param name="command">The command to bind.</param>
    public static void Bind(object context, ICommandExecutor commandExecutor, ICommand command)
    {
        CheckBindingArgs(context, commandExecutor, command);

        BindInternal(context,
            commandExecutor, () => command.Execute(null),
            command, (object? _, EventArgs _) => commandExecutor.CanExecuteCommand = command.CanExecute(null));

        commandExecutor.CanExecuteCommand = command.CanExecute(null);
    }

    /// <summary>
    /// Binds <paramref name="command"/> to a <paramref name="commandExecutor"/> in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="commandExecutor">The command executor that will execute the command.</param>
    /// <param name="command">The command to bind.</param>
    /// <param name="commandParameterGetter">A function that returns the current value of the parameter to be passed to the command.</param>
    public static void Bind(object context, ICommandExecutor commandExecutor, ICommand command, Func<object> commandParameterGetter)
    {
        CheckBindingArgs(context, commandExecutor, command);
        ArgumentNullException.ThrowIfNull(commandParameterGetter, nameof(commandParameterGetter));

        BindInternal(context,
            commandExecutor, () => command.Execute(commandParameterGetter()),
            command, (object? _, EventArgs _) => commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter()));

        commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter());
    }

    /// <summary>
    /// Binds <paramref name="command"/> to a <paramref name="commandExecutor"/> in a given context.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="commandExecutor">The command executor that will execute the command.</param>
    /// <param name="command">The command to bind.</param>
    /// <param name="commandParameterGetter">A function that returns the current value of the parameter to be passed to the command.</param>
    public static void Bind<T>(object context, ICommandExecutor commandExecutor, IRelayCommand<T> command, Func<T> commandParameterGetter)
    {
        CheckBindingArgs(context, commandExecutor, command);
        ArgumentNullException.ThrowIfNull(commandParameterGetter, nameof(commandParameterGetter));

        BindInternal(context,
            commandExecutor, () => command.Execute(commandParameterGetter()),
            command, (object? _, EventArgs _) => commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter()));

        commandExecutor.CanExecuteCommand = command.CanExecute(commandParameterGetter());
    }
    #endregion

    #region Unbinding
    /// <summary>
    /// Unbinds <see cref="ICommand"/>s from the corresponding <see cref="ICommandExecutor"/>s in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void Unbind(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _commandBindings.Where(cb => cb.Context == context).ToList().ForEach(Unbind);
    }

    /// <summary>
    /// Unbinds a <see cref="ICommand"/> from <paramref name="commandExecutor"/> in a given context, if there is a binding.
    /// </summary>
    /// <param name="commandExecutor">The command executor the command was bound to. </param>
    public static void Unbind(object context, ICommandExecutor commandExecutor)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(commandExecutor, nameof(commandExecutor));

        var commandBinding = _commandBindings.FirstOrDefault(cb => cb.Context == context && cb.CommandExecutor == commandExecutor);
        if (commandBinding is not null)
            Unbind(commandBinding);
    }
    #endregion
    #endregion

    #region Private methods
    private static void BindInternal
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

    private static void CheckBindingArgs(object context, object commandExecutor, object command)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(commandExecutor, nameof(commandExecutor));
        ArgumentNullException.ThrowIfNull(command, nameof(command));
    }

    private static void Unbind(CommandBinding commandBinding)
    {
        commandBinding.CommandExecutor.CommandExecutionRequested -= commandBinding.CommandExecutionRequestedEventHandler;
        commandBinding.Command.CanExecuteChanged -= commandBinding.CommanCanExecuteChangedEventHandler;
        _commandBindings.Remove(commandBinding);
    }
    #endregion

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
}
