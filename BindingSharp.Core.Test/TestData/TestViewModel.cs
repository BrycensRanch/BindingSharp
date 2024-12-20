
/* Unmerged change from project 'BindingSharp.Core.Test(net9.0)'
Before:
using System;
using System.Collections;
After:
using System.Collections;
*/
using System.Collections;
using System.ComponentModel;

namespace BindingSharp.Core.Test.TestData;

public class TestViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
    {
        add { ErrorsChangedEventAdded = true; }
        remove { ErrorsChangedEventRemoved = true; }
    }
    public bool ErrorsChangedEventAdded;
    public bool ErrorsChangedEventRemoved;

    public bool HasErrors => false;

    public IEnumerable GetErrors(string propertyName)
    {
        return default;
    }

    public event PropertyChangedEventHandler PropertyChanged
    {
        add { PropertyChangedEventAdded = true; }
        remove { PropertyChangedEventRemoved = true; }
    }

    public bool PropertyChangedEventAdded;
    public bool PropertyChangedEventRemoved;

    public bool TestBool { get; set; }
}

