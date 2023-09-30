using EasyBindings.Tests.Controls;
using System.Collections.ObjectModel;

namespace EasyBindings.Tests;

[TestClass]
public class TriggerBinderTests
{
    [TestMethod]
    public void TestPropertyChangedTriggerExecutes()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, () => wasTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangedTriggerReceivesActualPropertyValue()
    {
        var textInput = new TextInput();

        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, newText => Assert.AreEqual(newText, textInput.Text));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangedTriggerReceivesSender()
    {
        var textInput = new TextInput();

        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, (sender, _) => Assert.AreEqual(sender, textInput));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangedTriggerMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasFirstTriggerCalled && wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangingTriggerExecutes()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, () => wasTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestPropertyChangingTriggerReceivesOldPropertyValue()
    {
        var textInput = new TextInput();

        var oldText = textInput.Text;
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, text => Assert.AreEqual(text, oldText));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangingTriggerReceivesSender()
    {
        var textInput = new TextInput();

        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, (sender, _) => Assert.AreEqual(sender, textInput));

        textInput.Text = "text";
    }

    [TestMethod]
    public void TestPropertyChangingTriggerMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);

        textInput.Text = "text";

        Assert.IsTrue(wasFirstTriggerCalled && wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerExecutes()
    {
        var observableCollection = new ObservableCollection<object?>();

        var wasTriggerCalled = false;
        TriggerBinder.OnCollectionChanged(this, observableCollection, () => wasTriggerCalled = true);

        observableCollection.Add(null);

        Assert.IsTrue(wasTriggerCalled);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerReceivesEventArgs()
    {
        var observableCollection = new ObservableCollection<object?>();

        TriggerBinder.OnCollectionChanged(this, observableCollection, (_, eventArgs) => Assert.IsNotNull(eventArgs));

        observableCollection.Add(null);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerReceivesSender()
    {
        var observableCollection = new ObservableCollection<object?>();

        TriggerBinder.OnCollectionChanged(this, observableCollection, (sender, _) => Assert.AreEqual(sender, observableCollection));

        observableCollection.Add(null);
    }

    [TestMethod]
    public void TestCollectionChangedTriggerMultiple()
    {
        var observableCollection = new ObservableCollection<object?>();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBinder.OnCollectionChanged(this, observableCollection, () => wasFirstTriggerCalled = true);
        TriggerBinder.OnCollectionChanged(this, observableCollection, () => wasSecondTriggerCalled = true);

        observableCollection.Add(null);

        Assert.IsTrue(wasFirstTriggerCalled && wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChanged()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, () => wasTriggerCalled = true);
        TriggerBinder.UnbindPropertyChanged(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChangedMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBinder.OnPropertyChanged(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);
        TriggerBinder.UnbindPropertyChanged(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasFirstTriggerCalled || wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChanging()
    {
        var textInput = new TextInput();

        var wasTriggerCalled = false;
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, () => wasTriggerCalled = true);
        TriggerBinder.UnbindPropertyChanging(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindPropertyChangingMultiple()
    {
        var textInput = new TextInput();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, () => wasFirstTriggerCalled = true);
        TriggerBinder.OnPropertyChanging(this, textInput, o => o.Text, () => wasSecondTriggerCalled = true);
        TriggerBinder.UnbindPropertyChanging(this, textInput);

        textInput.Text = "text";

        Assert.IsFalse(wasFirstTriggerCalled || wasSecondTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindCollectionChanged()
    {
        var observableCollection = new ObservableCollection<object?>();

        var wasTriggerCalled = false;
        TriggerBinder.OnCollectionChanged(this, observableCollection, () => wasTriggerCalled = true);
        TriggerBinder.UnbindCollectionChanged(this, observableCollection);

        observableCollection.Add(null);

        Assert.IsFalse(wasTriggerCalled);
    }

    [TestMethod]
    public void TestUnbindCollectionChangedMultiple()
    {
        var observableCollection = new ObservableCollection<object?>();

        bool wasFirstTriggerCalled = false,
             wasSecondTriggerCalled = false;
        TriggerBinder.OnCollectionChanged(this, observableCollection, () => wasFirstTriggerCalled = true);
        TriggerBinder.OnCollectionChanged(this, observableCollection, () => wasSecondTriggerCalled = true);
        TriggerBinder.UnbindCollectionChanged(this, observableCollection);

        observableCollection.Add(null);

        Assert.IsFalse(wasFirstTriggerCalled || wasSecondTriggerCalled);
    }
}