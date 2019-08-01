using GLib;
using Gtk;
using MVVMSharp.Gtk;

namespace MVVMSharp.Test.TestData.View
{

    public class WithoutCommandBinding : IWidget
    {
        public bool Sensitive { get; set; }

        public IButton Button;
    }

    public class WithCommandBindingWithoutIButton : IWidget
    {
        public bool Sensitive { get; set; }

        [CommandBinding(nameof(TestData.ViewModel.WithCommandProperty.CommandProperty))]
        public object Button;
    }

    public class WithCommandBinding : IWidget
    {
        public bool Sensitive { get; set; }

        [CommandBinding(nameof(TestData.ViewModel.WithCommandProperty.CommandProperty))]
        public IButton Button;
    }

    public interface WithObjectProperty
    {
        object ObjectProperty { get; }
    }

    public interface WithGlibObjectParent : GLib.IObject
    {
        object ObjectProperty { get; }
    }
}