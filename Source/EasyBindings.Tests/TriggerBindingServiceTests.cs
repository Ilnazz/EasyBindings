using EasyBindings.Tests.Controls;

namespace EasyBindings.Tests;

[TestClass]
public class TriggerBindingServiceTests
{
    [TestMethod]
    public void TestPropertyChangedTriggerExecutes()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBindingService.RegisterPropertyChanged(this, textInput, o => o.Text, () => wasTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangedTriggerReceivesActualPropertyValue()
    {
        var textInput = new TextInput();

        TriggerBindingService.RegisterPropertyChanged(this, textInput, o => o.Text, newText => Assert.AreEqual(newText, textInput.Text));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangedTriggerReceivesSender()
    {
        var textInput = new TextInput();

        TriggerBindingService.RegisterPropertyChanged(this, textInput, o => o.Text, (sender, _) => Assert.AreEqual(sender, textInput));

        textInput.Text = "text";
    }
}