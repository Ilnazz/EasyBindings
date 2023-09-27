using EasyBindings;
using EasyBindings.Tests.Controls;

namespace EasyBindings.Tests;

[TestClass]
public class PropertyBindingServiceTests
{
    [TestMethod]
    public void TestOneWay()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);

        textInput.Text = "...";

        Assert.AreEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestOneWayToSource()
    {
        var textInput = new TextInput();
        var textLabel = new TextLabel();

        PropertyBindingService.OneWayToSource(this, textInput, t => t.Text, textLabel, s => s.Text);

        textInput.Text = "...";

        Assert.AreEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestTwoWay()
    {
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();

        PropertyBindingService.TwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text);

        textInput1.Text = "...";
        Assert.AreEqual(textInput2.Text, textInput1.Text);

        textInput2.Text = "";
        Assert.AreEqual(textInput1.Text, textInput2.Text);
    }

    [TestMethod]
    public void TestUnregisterFromTarget()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);

        textInput.Text = "...";

        PropertyBindingService.UnregisterFromTarget(this, textLabel);

        textInput.Text = "";

        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregisterFromSource()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);

        textInput.Text = "...";

        PropertyBindingService.UnregisterFromSource(this, textInput);

        textInput.Text = "";

        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregister()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);

        textInput.Text = "...";

        PropertyBindingService.Unregister(this);

        textInput.Text = "";

        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }
}