using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyBindings.Tests.Controls;

public partial class TextInput : ObservableObject
{
    [ObservableProperty]
    private string _text = string.Empty;
}