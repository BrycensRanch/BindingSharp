using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Gtk;

namespace BindingSharp.Core.Gtk;

public static class WidgetExtension
{
    private static Dictionary<Widget, HashSet<IBinder>> bindings = new();

    #region Providers

    private static required StyleProvider styleProvider;
    internal static StyleProvider StyleProvider
    {
        [ExcludeFromCodeCoverage]
        get
        {
            if (styleProvider == null)
                styleProvider = GetCssProvider();

            return styleProvider;
        }
        set { styleProvider = value; }
    }

    private static required Func<StyleContext, string, IBinder> styleContextBindingProvider;
    internal static Func<StyleContext, string, IBinder> StyleContextBindingProvider
    {
        [ExcludeFromCodeCoverage]
        get
        {
            if (styleContextBindingProvider == null)
                styleContextBindingProvider = GtkStyleContextBindingProvider;

            return styleContextBindingProvider;
        }
        set { styleContextBindingProvider = value; }
    }

    private static required Func<Button, IBinder> commandBindingProvider;
    internal static Func<Button, IBinder> CommandBindingProvider
    {
        [ExcludeFromCodeCoverage]
        get
        {
            if (commandBindingProvider == null)
                commandBindingProvider = GtkCommandBindingProvider;

            return commandBindingProvider;
        }
        set { commandBindingProvider = value; }
    }

    private static required Func<Widget, string, IBinder> widgetBindingProvider;
    internal static Func<Widget, string, IBinder> WidgetBindingProvider
    {
        [ExcludeFromCodeCoverage]
        get
        {
            if (widgetBindingProvider == null)
                widgetBindingProvider = GtkWidgetBindingProvider;

            return widgetBindingProvider;
        }
        set { widgetBindingProvider = value; }
    }

    [ExcludeFromCodeCoverage]
    private static CssProvider GetCssProvider()
    {
        var provider = CssProvider.New();

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("invalid.css");
        if (stream == null || stream.Length == 0) return provider;
        using var reader = new StreamReader(stream);
        provider.LoadFromData(reader.ReadToEnd(), stream.Length);
        return provider;
    }

    [ExcludeFromCodeCoverage]
    private static IBinder GtkStyleContextBindingProvider(StyleContext styleContext, string cssClasName)
    {
        return new BindStyleContextToNotifyDataErrorInfo(styleContext, cssClasName);
    }

    [ExcludeFromCodeCoverage]
    private static IBinder GtkCommandBindingProvider(Button b)
    {
        return new BindButtonToCommand(b);
    }

    [ExcludeFromCodeCoverage]
    private static IBinder GtkWidgetBindingProvider(Widget widget, string propertyName)
    {
        return new BindINotifyPropertyChanged(widget, propertyName);
    }
    #endregion Providers

    public static void Bind(this Widget view, object obj)
    {
        var flags = System.Reflection.BindingFlags.Public;
        flags |= System.Reflection.BindingFlags.NonPublic;
        flags |= System.Reflection.BindingFlags.DeclaredOnly;
        flags |= System.Reflection.BindingFlags.Instance;

        var viewFields = view.GetType().GetFields(flags);

        foreach (var viewField in viewFields)
        {
            if (Attribute.IsDefined(viewField, typeof(CommandBindingAttribute)))
                BindCommand(view, obj, viewField);

            if (Attribute.IsDefined(viewField, typeof(PropertyBindingAttribute)))
                BindProperty(view, obj, viewField);

            if (Attribute.IsDefined(viewField, typeof(ValidationBindingAttribute)))
                BindValidation(view, obj, viewField);
        }
    }

    private static T GetViewFieldAs<T>(Widget view, FieldInfo viewField)
    {
        if (!typeof(T).IsAssignableFrom(viewField.FieldType))
            throw new Exception("??");

        return (T)viewField.GetValue(view);
    }

    private static T GetViewFieldAttribute<T>(FieldInfo viewField)
    {
        var viewFieldBindingAttrs = viewField.GetCustomAttributes(typeof(T), false);

        if (viewFieldBindingAttrs.Length == 0)
            return default(T);
        else
            return (T)viewFieldBindingAttrs[0];
    }

    private static void BindValidation(Widget view, object viewModel, FieldInfo viewField)
    {
        var attribute = GetViewFieldAttribute<ValidationBindingAttribute>(viewField);
        if (attribute != null)
        {
            var widget = GetViewFieldAs<Widget>(view, viewField);
            var styleContext = widget.GetStyleContext();
            styleContext.AddProvider(StyleProvider, uint.MaxValue);

            var binder = StyleContextBindingProvider(styleContext, "invalid");
            Bind(binder, view, viewModel, attribute.Property);
        }
    }

    private static void BindProperty(Widget view, object viewModel, FieldInfo viewField)
    {
        var attribute = GetViewFieldAttribute<PropertyBindingAttribute>(viewField);
        if (attribute != null)
        {
            var widget = GetViewFieldAs<Widget>(view, viewField);
            var binder = WidgetBindingProvider(widget, attribute.WidgetProperty);
            Bind(binder, view, viewModel, attribute.ViewModelProperty);
        }
    }

    private static void BindCommand(Widget view, object viewModel, FieldInfo viewField)
    {
        var attribute = GetViewFieldAttribute<CommandBindingAttribute>(viewField);
        if (attribute != null)
        {
            var button = GetViewFieldAs<Widget>(view, viewField);
            var binder = CommandBindingProvider((Button)button);
            Bind(binder, view, viewModel, attribute.CommandProperty);
        }
    }

    private static void Bind(IBinder binder, Widget view, object viewModel, string propertyName)
    {
        binder.Bind(viewModel, propertyName);
        CacheBinder(view, binder);
    }

    private static void CacheBinder(Widget view, IBinder binder)
    {
        if (!bindings.ContainsKey(view))
            bindings[view] = new HashSet<IBinder>();

        bindings[view].Add(binder);
    }

    ///<summary>
    /// Call this if disposing the widget
    //</summary>
    public static void DisposeBindings(this Widget view)
    {
        if (bindings.ContainsKey(view))
        {
            foreach (var binder in bindings[view])
            {
                if (binder is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            bindings.Remove(view);
        }
    }
}

