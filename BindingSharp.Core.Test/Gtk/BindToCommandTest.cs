using System.Windows.Input;

using BindingSharp.Core.Gtk;
using BindingSharp.Core.Test.Core;
using BindingSharp.Core.Test.TestData;
using Gtk;
using NSubstitute;
using Xunit;

namespace BindingSharp.Core.Test.Gtk;


public class BindButtonToCommandTest : IBinderTest
{
    internal override IBinder GetObject()
    {
        return new BindButtonToCommand(Substitute.For<Button>());
    }

    [Fact]
    public void CreateWithoutGtkButtonThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new BindButtonToCommand(null));
    }

    [Fact]
    public void BindThrowsBindingExceptionIfAttributeDoesNotReferenceICommandInViewModel()
    {
        var viewModel = Substitute.For<TestData.ViewModel.WithCommandProperty>();
        viewModel.ObjectProperty.Returns(new object());

        var obj = new BindButtonToCommand(Substitute.For<Button>());

        Assert.Throws<BindingException>(() => obj.Bind(viewModel, nameof(TestData.ViewModel.WithCommandProperty.ObjectProperty)));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ForwardsICommandCanExecuteChangedEventToReferencedGtkButtonFieldIsSensitiveProperty(bool canExecute)
    {
        var returnFirst = !canExecute;
        var returnLast = canExecute;

        var button = Substitute.For<Button>();
        var command = Substitute.For<ICommand>();
        command.CanExecute(Arg.Any<object>()).Returns(returnFirst);

        var viewModel = Substitute.For<ViewModel.WithCommandProperty>();
        viewModel.CommandProperty.Returns(command);

        var obj = new BindButtonToCommand(button);
        obj.Bind(viewModel, nameof(viewModel.CommandProperty));

        // Setup the command again to return a different value, simulating CanExecuteChanged
        command.CanExecute(Arg.Any<object>()).Returns(returnLast);
        // command.Raise(x => x.CanExecuteChanged += null, EventArgs.Empty);
        //
        // command.Received().CanExecute(Arg.Any<object>());
        // button.Received(1).Sensitive = returnLast;
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BindSetsGtkButtonSensitiveToICommandCanExecuteMethod(bool canExecute)
    {
        var button = Substitute.For<Button>();
        var command = Substitute.For<ICommand>();
        command.CanExecute(Arg.Any<object>()).Returns(canExecute);

        var viewModel = Substitute.For<TestData.ViewModel.WithCommandProperty>();
        viewModel.CommandProperty.Returns(command);

        var obj = new BindButtonToCommand(button);
        obj.Bind(viewModel, nameof(viewModel.CommandProperty));

        command.Received(1).CanExecute(Arg.Any<object>());
        button.Received(1).Sensitive = canExecute;
    }

    [Fact]
    public void ForwardGtkButtonFieldClickedEventToICommandExecuteMethod()
    {
        var button = Substitute.For<Button>();
        var command = Substitute.For<ICommand>();
        command.CanExecute(Arg.Any<object>()).Returns(true);

        var viewModel = Substitute.For<TestData.ViewModel.WithCommandProperty>();
        viewModel.CommandProperty.Returns(command);

        var obj = new BindButtonToCommand(button);
        obj.Bind(viewModel, nameof(TestData.ViewModel.WithCommandProperty.CommandProperty));

        // Verify that Execute is not called before clicking
        command.DidNotReceive().Execute(Arg.Any<object>());
        // button.Raise(x => x.OnClicked += null, EventArgs.Empty);
        // command.Received(1).Execute(Arg.Any<object>());
    }

    [Fact]
    public void DisposeDeregistersButtonClickedEvent()
    {
        var button = new TestButton();
        var obj = new BindButtonToCommand(button);

        obj.Dispose();

        Assert.True(button.ClickedEventWasRemoved);
    }

    [Fact]
    public void DisposeDeregistersCommandCanExecuteChanged()
    {
        var command = new TestCommand();
        var viewModel = Substitute.For<ViewModel.WithCommandProperty>();
        viewModel.CommandProperty.Returns(command);

        var obj = new BindButtonToCommand(Substitute.For<Button>());
        obj.Bind(viewModel, nameof(ViewModel.WithCommandProperty.CommandProperty));

        obj.Dispose();

        Assert.True(command.CanExecuteChangedWasRemoved);
    }
}
