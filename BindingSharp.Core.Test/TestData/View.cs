using System.ComponentModel;
using BindingSharp.Core.Gtk;
using Gtk;

namespace BindingSharp.Core.Test.TestData;

public class View
{
    public class WithoutCommandBinding : Widget
    {
        public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        public new bool Sensitive { get; set; }

        public StyleContext StyleContext => null;

        public Button Button = null;
    }

    public class WithCommandBindingWithoutIButton : Widget
    {
        public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        public new bool Sensitive { get; set; }

        public StyleContext StyleContext => null;

        [CommandBinding(nameof(ViewModel.WithCommandProperty.CommandProperty))]
        public object Button = null;
    }

    public class WithCommandBinding : Widget
    {
        public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        public new bool Sensitive { get; set; }

        public StyleContext StyleContext => null;

        [CommandBinding(nameof(TestData.ViewModel.WithCommandProperty.CommandProperty))]
        public required Button Button;
    }

    public interface WithObjectProperty
    {
        object ObjectProperty { get; }
    }

    public class WidgetWithObjectPropery : Widget
    {
        public required object ObjectProperty { get; set; }
    }


    public interface WithoutINotifyPropertyChanged
    {
        object ObjectProperty { get; }
    }
}

