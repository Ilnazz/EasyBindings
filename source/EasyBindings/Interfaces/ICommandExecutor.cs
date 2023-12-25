using System;

namespace EasyBindings.Interfaces;

/// <summary>
/// Provides a mechanism for notifying about the need to execute a command and switch command execution ability.
/// </summary>
public interface ICommandExecutor
{
    /// <summary>
    /// Occurs when there is a need to execute the command.
    /// </summary>
    event Action? CommandExecutionRequested;

    /// <summary>
    /// A property that allows to switch the ability of an object to execute a command.
    /// </summary>
    bool CanExecuteCommand { set; }
}