# BindingSharp


BindingSharp enables MVVM-Style programming with [Gir.Core](https://gircore.github.io/). It is a library to bind properties of a GTK widget to a ViewModel.

If you have a .NET 8 or .NET 9 app,
you can port its [View][] to [GTK](https://gtk.org/)
and reuse the rest of your code to be deployed wherever the hell you want.

 [View]: https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel#:~:text=Components%20of%20MVVM%20pattern,-Model&text=As%20in%20the%20model%E2%80%93view,user%20sees%20on%20the%20screen

## Features
 * Binds properties of a [GTK.Widget][] to a ViewModel with a one-way or two-way binding via the [INotifyPropertyChanged][] interface
 * Special binding for a [GTK.Button][] which can be bound to an [ICommand][]
 * Supports binding of a [GTK.Widget][] to a property of the ViewModel to support validation via the [INotifyDataErrorInfo][] interface (still work in progress)

 [GTK.Widget]: https://docs.gtk.org/gtk4/class.Widget.html
 [GTK.Button]: https://docs.gtk.org/gtk4/class.Button.html
 [ICommand]: https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.icommand?view=net-9.0
 [INotifyPropertyChanged]: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-9.0
 [INotifyDataErrorInfo]: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifydataerrorinfo?view=net-9.0

## Using
To use the binding,
the application must provide the ViewModel to the view to be able to create the binding inside the view.

For a complete sample see the [Sample App](BindingSharp.Sample).

1. Create a view class with a matching glade file which describes the user interface as XML. Inside your view reference some UI widgets in fields. For working examples see the [templates][] of Gir.Core.
2. Add the _PropertyBindingAttribute_ or _CommandBindingAttribute_ or _ValidationBindingAttribute_ to a widget of your UI
3. Call _Bind(object obj)_ in your view's constructor to setup the binding

        public class MyWidget : Box
        {
            ...

            [UI]
            [CommandBinding(nameof(ViewModelClass.MyCommand))]
            private Button MyButton;

            public MyWidget(object ViewModel) : this(new Builder("MyWidget.glade"))
            {
                this.Bind(ViewModel)
            }

            ...
        }
[templates]: https://github.com/gircore/gir.core/tree/19ea31d95edc93a61f5d12ebae1ab8d2b69dcfcd/src/Samples

## Building from source

---

There are 3 projects inside the repository:
 - **BindingSharp.Core:** Project source
 - **BindingSharp.Sample:** Example gtk application
 - **BindingSharp.Test:** Unit tests

 To build the source code run `dotnet build` in the corresponding project folder.

 To run the sample app execute `dotnet run` in the BindingSharp.Sample folder.

 To test the code run `dotnet test` in the BindingSharp.Test folder.

## License

BindingSharp and its related components are licensed under [LGPL v2.1 license](LICENSE.md).
