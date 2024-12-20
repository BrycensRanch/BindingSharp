using System.ComponentModel;
using BindingSharp.Core.Gtk;
using GtkLib = Gtk;

namespace BindingSharp.Sample.View;

public class View : GtkLib.Box
{
    [PropertyBinding(nameof(GetProperty), nameof(ViewModel.Label))]
    public GtkLib.Label Label;

    [CommandBinding(nameof(ViewModel.ChangeLabelCommand))]
    public GtkLib.Button ChangeLabelButton;

    [ValidationBinding(nameof(ViewModel.ToggleErrorCommand))]
    [CommandBinding(nameof(ViewModel.ToggleErrorCommand))]
    public GtkLib.Button ToggleErrorButton;

    public View(object viewModel)
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel), "The view model cannot be null.");
        }
        if (!(viewModel is INotifyPropertyChanged))
        {
            throw new InvalidOperationException("View model must implement INotifyPropertyChanged.");
        }

        // Output the view model for debugging
        Console.WriteLine(viewModel);

        // Bind the view model to the view
        this.Bind(viewModel);

        // Load the UI from the Glade file
        var builder = new GtkLib.Builder("View.glade");

        // Connect signals defined in the Glade file to this object
        builder.Connect(this);
    }




    protected void Dispose(bool disposing = false)
    {
        if (disposing)
        {
            this.DisposeBindings();
        }
        base.Dispose();
    }
}
