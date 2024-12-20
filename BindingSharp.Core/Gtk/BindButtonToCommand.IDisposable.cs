namespace BindingSharp.Core.Gtk;

internal partial class BindButtonToCommand : IDisposable
{
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (button != null) button.OnClicked -= OnButtonBlicked;
                if (command != null) command.CanExecuteChanged -= OnCommandCanExectueChanged;
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}

