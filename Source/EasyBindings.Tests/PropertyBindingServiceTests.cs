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

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestOneWayWithConverter()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text, sourceText => sourceText.ToUpper());

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text.ToUpper());
    }

    [TestMethod]
    public void TestOneWayMultiple()
    {
        var textLabel = new TextLabel();
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput1, s => s.Text);
        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput2, s => s.Text);

        textInput1.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput1.Text);

        textInput2.Text = "text2";
        Assert.AreEqual(textLabel.Text, textInput2.Text);
    }

    [TestMethod]
    public void TestOneWayToSource()
    {
        var textInput = new TextInput();
        var textLabel = new TextLabel();

        PropertyBindingService.OneWayToSource(this, textInput, t => t.Text, textLabel, s => s.Text);

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestOneWayToSourceWithConverter()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text, sourceText => sourceText.ToUpper());

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text.ToUpper());
    }

    [TestMethod]
    public void TestOneWayToSourceMultiple()
    {
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();
        var textLabel = new TextLabel();

        PropertyBindingService.OneWayToSource(this, textInput1, t => t.Text, textLabel, s => s.Text);
        PropertyBindingService.OneWayToSource(this, textInput2, t => t.Text, textLabel, s => s.Text);

        textInput1.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput1.Text);

        textInput2.Text = "text2";
        Assert.AreEqual(textLabel.Text, textInput2.Text);
    }

    [TestMethod]
    public void TestTwoWay()
    {
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();

        PropertyBindingService.TwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text);

        textInput1.Text = "text";
        Assert.AreEqual(textInput2.Text, textInput1.Text);

        textInput2.Text = "";
        Assert.AreEqual(textInput1.Text, textInput2.Text);
    }

    [TestMethod]
    public void TestTwoWayWithConverter()
    {
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();

        PropertyBindingService.TwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text, targetText => targetText.ToUpper(), sourceText => sourceText.ToLower());

        textInput1.Text = "text";
        Assert.AreEqual(textInput2.Text, textInput1.Text.ToUpper());

        textInput2.Text = "TEXT";
        Assert.AreEqual(textInput1.Text, textInput2.Text.ToLower());
    }

    [TestMethod]
    public void TestTwoWayMultiple()
    {
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();
        var textInput3 = new TextInput();
        var textInput4 = new TextInput();

        PropertyBindingService.TwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text);
        PropertyBindingService.TwoWay(this, textInput3, t => t.Text, textInput4, s => s.Text);

        textInput1.Text = "text1";
        Assert.AreEqual(textInput1.Text, textInput2.Text);
        textInput2.Text = "text2";
        Assert.AreEqual(textInput2.Text, textInput1.Text);

        textInput3.Text = "text3";
        Assert.AreEqual(textInput3.Text, textInput4.Text);
        textInput3.Text = "text4";
        Assert.AreEqual(textInput3.Text, textInput3.Text);
    }

    [TestMethod]
    public void TestUnregisterFromTarget()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.UnregisterFromTarget(this, textLabel);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregisterFromTargetMultiple()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.UnregisterFromTarget(this, textLabel);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregisterFromSource()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.UnregisterFromSource(this, textInput);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregisterFromSourceMultiple()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.UnregisterFromSource(this, textInput);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregister()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.Unregister(this);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnregisterMultiple()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.OneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBindingService.Unregister(this);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }
}