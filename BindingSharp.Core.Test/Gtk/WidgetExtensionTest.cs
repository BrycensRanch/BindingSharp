
using BindingSharp.Core.Gtk;
using BindingSharp.Core.Test.TestData;
using Gtk;
using NSubstitute;
using Xunit;

namespace BindingSharp.Core.Test.Gtk;

public class WidgetExtensionTest
{
    [Fact]
    public void BindViewModelBindsButtonToICommandOfViewModel()
    {
        var viewModel = Substitute.For<ViewModel.WithCommandProperty>();
        var button = Substitute.For<Button>();
        var bindToCommand = Substitute.For<IBinder>();

        var view = new View.WithCommandBinding
        {
            Button = button
        };

        var buttonPassed = false;

        WidgetExtension.CommandBindingProvider = (Button b) =>
        {
            if (b == button)
                buttonPassed = true;

            return bindToCommand;
        };

        view.Bind(viewModel);

        Assert.True(buttonPassed);
        bindToCommand.Received().Bind(viewModel, nameof(viewModel.CommandProperty));
    }
}
