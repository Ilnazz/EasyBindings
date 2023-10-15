using EasyBindings.Tests.Controls;

namespace EasyBindings.Tests;

[TestClass]
public class PropertyBinderTests
{
    [TestMethod]
    public void TestOneWay()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestOneWayWithConverter()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text, sourceText => sourceText.ToUpper());

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text.ToUpper());
    }

    [TestMethod]
    public void TestOneWayMultiple()
    {
        var textLabel = new TextLabel();
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput1, s => s.Text);
        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput2, s => s.Text);

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

        PropertyBinder.BindOneWayToSource(this, textInput, t => t.Text, textLabel, s => s.Text);

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestOneWayToSourceWithConverter()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text, sourceText => sourceText.ToUpper());

        textInput.Text = "text";
        Assert.AreEqual(textLabel.Text, textInput.Text.ToUpper());
    }

    [TestMethod]
    public void TestOneWayToSourceMultiple()
    {
        var textInput1 = new TextInput();
        var textInput2 = new TextInput();
        var textLabel = new TextLabel();

        PropertyBinder.BindOneWayToSource(this, textInput1, t => t.Text, textLabel, s => s.Text);
        PropertyBinder.BindOneWayToSource(this, textInput2, t => t.Text, textLabel, s => s.Text);

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

        PropertyBinder.BindTwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text);

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

        PropertyBinder.BindTwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text, targetText => targetText.ToUpper(), sourceText => sourceText.ToLower());

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

        PropertyBinder.BindTwoWay(this, textInput1, t => t.Text, textInput2, s => s.Text);
        PropertyBinder.BindTwoWay(this, textInput3, t => t.Text, textInput4, s => s.Text);

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
    public void TestUnbindFromTarget()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.UnbindFromTarget(this, textLabel);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnbindFromTargetMultiple()
    {
        var textLabel = new TextLabel();
        TextInput textInput = new(),
                  textInput2 = new();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput2, s => s.Text);
        PropertyBinder.UnbindFromTarget(this, textLabel);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
        textInput2.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput2.Text);
    }

    [TestMethod]
    public void TestUnbindFromSource()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.UnbindFromSource(this, textInput);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnbindFromSourceMultiple()
    {
        var textLabel = new TextLabel();
        TextInput textInput = new(),
                  textInput2 = new();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput2, s => s.Text);
        PropertyBinder.UnbindFromSource(this, textLabel);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
        textInput2.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput2.Text);
    }

    [TestMethod]
    public void TestUnbind()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.Unbind(this);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }

    [TestMethod]
    public void TestUnbindMultiple()
    {
        var textLabel = new TextLabel();
        var textInput = new TextInput();

        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.BindOneWay(this, textLabel, t => t.Text, textInput, s => s.Text);
        PropertyBinder.Unbind(this);

        textInput.Text = "text";
        Assert.AreNotEqual(textLabel.Text, textInput.Text);
    }
}