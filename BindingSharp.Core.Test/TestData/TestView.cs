using System.ComponentModel;
using Gtk;

namespace BindingSharp.Core.Test.TestData;

public class TestWidget : Widget
{
    public event PropertyChangedEventHandler PropertyChanged
    {
        add { PropertyChangedEventAdded = true; }
        remove { PropertyChangedEventRemoved = true; }
    }

    public bool PropertyChangedEventAdded;
    public bool PropertyChangedEventRemoved;

    public bool TestBool { get; set; }
    public new bool Sensitive { get; set; }

    public StyleContext StyleContext => null;
}

