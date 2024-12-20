using System.Windows.Input;
using Gtk;

namespace BindingSharp.Core.Gtk;

internal partial class BindButtonToCommand : IBinder
{
    private readonly Button button;
    private ICommand command;

    public BindButtonToCommand(Button button)
    {
        this.button = button ?? throw new ArgumentNullException(nameof(button));
    }

    public void Bind(object viewModel, string commandPropertyName)
    {
        if (viewModel == null)
            throw new ArgumentNullException(nameof(viewModel));

        var viewModelCommandProperty = viewModel.GetType().GetProperty(commandPropertyName);

        if (viewModelCommandProperty == null)
            throw new BindingException(viewModel, $"Property {commandPropertyName} is not a property of {nameof(viewModel)}.");

        if (!typeof(ICommand).IsAssignableFrom(viewModelCommandProperty.PropertyType))
            throw new BindingException(viewModel, $"Property {commandPropertyName} is not an {nameof(ICommand)}.");

        command = (ICommand)viewModelCommandProperty.GetValue(viewModel);
        command.CanExecuteChanged += OnCommandCanExectueChanged;

        button.OnClicked += OnButtonBlicked;
        button.Sensitive = command.CanExecute(null);
    }

    private void OnButtonBlicked(object sender, EventArgs args)
    {
        if (command != null)
        {
            command.Execute(null);
        }
    }

    private void OnCommandCanExectueChanged(object sender, EventArgs args)
    {
        button.Sensitive = command.CanExecute(null);
    }
}

