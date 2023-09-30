using CommunityToolkit.Mvvm.Input;
using EasyBindings.Tests.Controls;

namespace EasyBindings.Tests;

[TestClass]
public class CommandBinderTests
{
    [TestMethod]
    public void TestWithoutCanExecuteAndWithoutParameter()
    {
        var wasCommandExecuted = false;
        var command = new RelayCommand(() => wasCommandExecuted = true);

        var button = new Button();
        CommandBinder.Bind(this, button, command);

        button.Press();

        Assert.IsTrue(wasCommandExecuted);
    }

    [TestMethod]
    public void TestWithCanExecuteAndWithoutParameter()
    {
        var wasCommandExecuted = false;
        var command = new RelayCommand(() => wasCommandExecuted = true, () => false);

        var button = new Button();
        CommandBinder.Bind(this, button, command);

        button.Press();

        Assert.IsFalse(wasCommandExecuted);
    }

    [TestMethod]
    public void TestWithCanExecuteAndWithParameter()
    {
        int leftNumber = 0,
            rightNumber = 0,
            divisionResult = 0;
        
        var divideCommand = new RelayCommand<(int, int)>
        (
            numbers => divisionResult = numbers.Item1 / numbers.Item2,
            numbers => numbers.Item2 != 0
        );

        var doDivisionButton = new Button();
        CommandBinder.Bind(this, doDivisionButton, divideCommand, () => (leftNumber, rightNumber));

        leftNumber = 10;
        rightNumber = 5;

        divideCommand.NotifyCanExecuteChanged();
        doDivisionButton.Press();
        Assert.AreEqual(divisionResult, 2);

        rightNumber = 0;
        
        divideCommand.NotifyCanExecuteChanged();
        doDivisionButton.Press();
        // Assert result was not changed => command was not executed
        Assert.AreEqual(divisionResult, 2);
    }
}