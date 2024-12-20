// See https://aka.ms/new-console-template for more information

using BindingSharp.Sample.View;
using Gio;
using Gtk;

var app = Gtk.Application.New("BindingSharp.Sample", ApplicationFlags.DefaultFlags);
app.OnActivate += (sender, eventArgs) =>
{
    var win = new ApplicationWindow()
    {
        Title = "Sample App",
        Application = app,
    };
    win.OnDestroy += (o, argss) => app.Quit();

    var viewModel = new ViewModel();
    var view = new View(viewModel);

    win.SetChild(view);
    win.Show();
};
app.Run(0, args);

