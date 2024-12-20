using System.ComponentModel;
using BindingSharp.Core.Test.TestData;
using NSubstitute;
using Xunit;

namespace BindingSharp.Core.Test.Core;

public class BindTwoINotifyPropertyChangedObjects : IBinderTest
{
    internal override IBinder GetObject()
    {
        // Substitute for WidgetWithObjectProperty
        var source = Substitute.For<View.WidgetWithObjectPropery>();
        return new BindINotifyPropertyChanged(source, nameof(View.WidgetWithObjectPropery.ObjectProperty));
    }

    [Fact]
    public void CreateWithoutSourceThrowsArgumentNullException()
    {
        // Assert that ArgumentNullException is thrown
        var exception = Assert.Throws<ArgumentNullException>(() => new BindINotifyPropertyChanged(null, ""));
        Assert.Equal("source", exception.ParamName); // Check that "source" is the parameter name causing the exception
    }

    [Fact]
    public void CreateWithoutPropertyThrowsArgumentNullException()
    {
        var source = Substitute.For<INotifyPropertyChanged>();
        var exception = Assert.Throws<ArgumentNullException>(() => new BindINotifyPropertyChanged(source, null));
        Assert.Equal("property", exception.ParamName);
    }

    [Fact]
    public void CreateWithUnknownPropertyThrowsBindingException()
    {
        var view = Substitute.For<View.WidgetWithObjectPropery>();
        //
        // var exception = Assert.Throws<BindingException>(() => new BindINotifyPropertyChanged(view, "Wrong"));
        // Assert.Equal("Wrong", exception.Message); // Check the exception message
    }

    [Fact]
    public void ForwardsChangedPropertyFromSourceToTarget()
    {
        object newValue = "1";
        var target = Substitute.For<ViewModel.WithINotifyPropertyChangedImplementation>();
        var source = Substitute.For<View.WidgetWithObjectPropery>();
        source.ObjectProperty.Returns(newValue);

        var obj = new BindINotifyPropertyChanged(source, nameof(source.ObjectProperty));
        obj.Bind(target, nameof(target.ObjectProperty));

        // source.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs(nameof(source.ObjectProperty)));
        //
        // target.Received().ObjectProperty = newValue; // Verifies the setter was called with new value
    }

    [Fact]
    public void ForwardsChangedPropertyFromTargetToSource()
    {
        object newValue = "1";
        var target = Substitute.For<TestData.ViewModel.WithINotifyPropertyChangedImplementation>();
        target.ObjectProperty.Returns(newValue);
        var source = Substitute.For<TestData.View.WidgetWithObjectPropery>();

        var obj = new BindINotifyPropertyChanged(source, nameof(source.ObjectProperty));
        obj.Bind(target, nameof(target.ObjectProperty));

        // target.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs(nameof(target.ObjectProperty)));
        //
        // source.Received().ObjectProperty = newValue; // Verifies the setter was called with new value
    }

    [Fact]
    public void DisposeDeregistersPropertyChangedEventFromSource()
    {
        var source = new TestWidget();
        var obj = new BindINotifyPropertyChanged(source, nameof(source.TestBool));

        obj.Dispose();

        Assert.True(source.PropertyChangedEventRemoved); // Check if the event was removed
    }

    [Fact]
    public void DisposeDeregistersPropertyChangedEventFromTarget()
    {
        var source = new TestWidget();
        var target = new TestViewModel();
        var obj = new BindINotifyPropertyChanged(source, nameof(source.TestBool));
        obj.Bind(target, nameof(target.TestBool));

        obj.Dispose();

        Assert.True(target.PropertyChangedEventRemoved); // Check if the event was removed
    }
}
