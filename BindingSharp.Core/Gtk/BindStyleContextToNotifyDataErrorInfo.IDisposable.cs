namespace BindingSharp.Core.Gtk;

internal partial class BindStyleContextToNotifyDataErrorInfo : IDisposable
{
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (notifyDataErrorInfo != null) notifyDataErrorInfo.ErrorsChanged -= OnErrorsChanged;
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}

