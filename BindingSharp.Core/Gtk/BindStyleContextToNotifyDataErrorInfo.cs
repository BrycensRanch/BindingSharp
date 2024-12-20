using System.ComponentModel;
using Gtk;

namespace BindingSharp.Core.Gtk
{
    internal partial class BindStyleContextToNotifyDataErrorInfo : IBinder, IBinder<INotifyDataErrorInfo>
    {
        private INotifyDataErrorInfo notifyDataErrorInfo;
        private readonly StyleContext styleContext;
        private readonly string invalidCssClassName;
        private string propertyName;

        public BindStyleContextToNotifyDataErrorInfo(StyleContext styleContext, string invalidCssClassName)
        {
            this.invalidCssClassName = invalidCssClassName ?? throw new ArgumentNullException(nameof(invalidCssClassName));
            this.styleContext = styleContext ?? throw new System.ArgumentNullException(nameof(styleContext));
        }

        public void Bind(object target, string property)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            else if (target is INotifyDataErrorInfo n)
                Bind(n, property);
            else
                throw new ArgumentException($"{nameof(target)} must be of type {nameof(INotifyDataErrorInfo)}", nameof(target));
        }

        public void Bind(INotifyDataErrorInfo target, string property)
        {
            notifyDataErrorInfo = target ?? throw new ArgumentNullException(nameof(target));
            propertyName = property ?? throw new ArgumentNullException(nameof(property));

            target.ErrorsChanged += OnErrorsChanged;
        }

        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            if (e.PropertyName == propertyName && sender is INotifyDataErrorInfo target)
            {
                var errors = target.GetErrors(e.PropertyName);

                var hasErros = errors.Cast<object>().Any();
                var hasClass = styleContext.HasClass(invalidCssClassName);

                if (hasErros && !hasClass)
                {
                    styleContext.AddClass(invalidCssClassName);
                }
                else if (!hasErros && hasClass)
                {
                    styleContext.RemoveClass(invalidCssClassName);
                }
            }
        }
    }
}
