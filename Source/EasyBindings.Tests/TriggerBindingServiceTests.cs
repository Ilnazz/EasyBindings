using EasyBindings.Tests.Controls;
using System.Collections.ObjectModel;

namespace EasyBindings.Tests;

[TestClass]
public class TriggerBindingServiceTests
{
    [TestMethod]
    public void TestPropertyChangedTriggerExecutes()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, () => wasTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangedTriggerReceivesActualPropertyValue()
    {
        var textInput = new TextInput();

        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, newText => Assert.AreEqual(newText, textInput.Text));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangedTriggerReceivesSender()
    {
        var textInput = new TextInput();

        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, (sender, _) => Assert.AreEqual(sender, textInput));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangedTriggerMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasFirstTriggerCalled && wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangingTriggerExecutes()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, () => wasTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangingTriggerReceivesOldPropertyValue()
    {
        var textInput = new TextInput();

        var oldText = textInput.Text;
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, text => Assert.AreEqual(text, oldText));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangingTriggerReceivesSender()
    {
        var textInput = new TextInput();

        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, (sender, _) => Assert.AreEqual(sender, textInput));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangingTriggerMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasFirstTriggerCalled && wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerExecutes()
    {
        var observableCollection = new ObservableCollection<object?>();

        var wasTriggerCalled = false;
        TriggerBindingService.OnCollectionChanged(this, observableCollection, () => wasTriggerCalled = true);

        observableCollection.Add(null);

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerReceivesEventArgs()
    {
        var observableCollection = new ObservableCollection<object?>();

        TriggerBindingService.OnCollectionChanged(this, observableCollection, (_, eventArgs) => Assert.IsNotNull(eventArgs));

        observableCollection.Add(null);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerReceivesSender()
    {
        var observableCollection = new ObservableCollection<object?>();

        TriggerBindingService.OnCollectionChanged(this, observableCollection, (sender, _) => Assert.AreEqual(sender, observableCollection));

        observableCollection.Add(null);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerMultiple()
    {
        var observableCollection = new ObservableCollection<object?>();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBindingService.OnCollectionChanged(this, observableCollection, () => wasFirstTriggerCalled = true);
        TriggerBindingService.OnCollectionChanged(this, observableCollection, () => wasSecondTriggerCalled = true);

        observableCollection.Add(null);

        Assert.IsTrue(wasFirstTriggerCalled && wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChanged()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, () => wasTriggerCalled = true);
        TriggerBindingService.UnbindPropertyChanged(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChangedMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBindingService.OnPropertyChanged(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);
        TriggerBindingService.UnbindPropertyChanged(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasFirstTriggerCalled || wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChanging()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, () => wasTriggerCalled = true);
        TriggerBindingService.UnbindPropertyChanging(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChangingMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBindingService.OnPropertyChanging(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);
        TriggerBindingService.UnbindPropertyChanging(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasFirstTriggerCalled || wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindCollectionChanged()
    {
        var observableCollection = new ObservableCollection<object?>();

        var wasTriggerCalled = false;
        TriggerBindingService.OnCollectionChanged(this, observableCollection, () => wasTriggerCalled = true);
        TriggerBindingService.UnbindCollectionChanged(this, observableCollection);

        observableCollection.Add(null);

        Assert.IsFalse(wasTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindCollectionChangedMultiple()
    {
        var observableCollection = new ObservableCollection<object?>();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBindingService.OnCollectionChanged(this, observableCollection, () => wasFirstTriggerCalled = true);
        TriggerBindingService.OnCollectionChanged(this, observableCollection, () => wasSecondTriggerCalled = true);
        TriggerBindingService.UnbindCollectionChanged(this, observableCollection);

        observableCollection.Add(null);

        Assert.IsFalse(wasFirstTriggerCalled || wasSecondTriggerCalled);
    }
}