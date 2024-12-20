
/* Unmerged change from project 'BindingSharp.Core.Test(net9.0)'
Before:
using System;
using System.ComponentModel;
After:
using System.ComponentModel;
*/
using System.ComponentModel;
using Gtk;

namespace BindingSharp.Core.Test.TestData;

public class TestButton : Button
{
    public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }

    public event EventHandler Clicked
    {
        add { ClickedEventWasAdded = true; }
        remove { ClickedEventWasRemoved = true; }
    }

    public bool ClickedEventWasRemoved;
    public bool ClickedEventWasAdded;

    public bool Sensitive { get; set; }

    public StyleContext StyleContext => null;
}

