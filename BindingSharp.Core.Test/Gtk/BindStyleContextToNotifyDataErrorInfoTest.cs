
/* Unmerged change from project 'BindingSharp.Core.Test(net9.0)'
Before:
using System;
using System.ComponentModel;
After:
using System.ComponentModel;
*/
using System.ComponentModel;
using BindingSharp.Core.Gtk;
using BindingSharp.Core.Test.TestData;
using Gtk;
using NSubstitute;
using Xunit;

namespace BindingSharp.Core.Test.Gtk;

public class BindStyleContextToNotifyDataErrorInfoTest
{
internal BindStyleContextToNotifyDataErrorInfo GetObject(StyleContext styleContext = null, string cssClassName = null)
{
    if (styleContext == null)
        styleContext = Substitute.For<StyleContext>();

    if (cssClassName == null)
        cssClassName = "";

    return new BindStyleContextToNotifyDataErrorInfo(styleContext, cssClassName);
}

[Fact]
public void CreateWithoutStyleContextThrowsArgumentNullException()
{
    Assert.Throws<ArgumentNullException>(() => new BindStyleContextToNotifyDataErrorInfo(null, ""));
}

[Fact]
public void CreateWithoutPropertyThrowsArgumentNullException()
{
    Assert.Throws<ArgumentNullException>(() => new BindStyleContextToNotifyDataErrorInfo(Substitute.For<StyleContext>(), null));
}

[Fact]
public void BindWithoutTargetThrowsArgumentNullException()
{
    var obj = GetObject();
    Assert.Throws<ArgumentNullException>(() => obj.Bind(null, ""));
}

[Fact]
public void BindWithTargetWhichIsNoINotifyDataErrorInfoThrowsArgumentException()
{
    var obj = GetObject();
    Assert.Throws<ArgumentException>(() => obj.Bind(new object(), ""));
}

[Fact]
public void GenericBindWithoutTargetThrowsArgumentNullException()
{
    var obj = GetObject();
    Assert.Throws<ArgumentNullException>(() => obj.Bind(default(INotifyDataErrorInfo), ""));
}

[Fact]
public void GenericBindWithoutPropertyThrowsArgumentNullException()
{
    var obj = GetObject();
    Assert.Throws<ArgumentNullException>(() => obj.Bind(Substitute.For<INotifyDataErrorInfo>(), null));
}

[Fact]
public void OnErrorsChangedAddsClassToStyleContextIfNotPresent()
{
    var cssClassName = "cssClassName";
    var property = "para";

    // Create substitutes for StyleContext and INotifyDataErrorInfo
    var styleContext = Substitute.For<StyleContext>();
    styleContext.HasClass(cssClassName).Returns(false);

    var notifyDataErrorInfo = Substitute.For<INotifyDataErrorInfo>();
    notifyDataErrorInfo.GetErrors(property).Returns(new List<string> { "Error" });

    var obj = GetObject(styleContext, cssClassName);
    obj.Bind(notifyDataErrorInfo, property);

    // Raise the ErrorsChanged event
    notifyDataErrorInfo.ErrorsChanged += Raise.EventWith(new DataErrorsChangedEventArgs(property));

    // Verify that AddClass was called
    styleContext.Received().AddClass(cssClassName);
}

[Fact]
public void OnErrorsChangedRemovesClassFromStyleContextIfPresent()
{
    var cssClassName = "cssClassName";
    var property = "para";

    // Create substitutes for StyleContext and INotifyDataErrorInfo
    var styleContext = Substitute.For<StyleContext>();
    styleContext.HasClass(cssClassName).Returns(true);

    var notifyDataErrorInfo = Substitute.For<INotifyDataErrorInfo>();
    notifyDataErrorInfo.GetErrors(property).Returns(new List<string>());

    var obj = GetObject(styleContext, cssClassName);
    obj.Bind(notifyDataErrorInfo, property);

    // Raise the ErrorsChanged event
    notifyDataErrorInfo.ErrorsChanged += Raise.EventWith(new DataErrorsChangedEventArgs(property));

    // Verify that RemoveClass was called
    styleContext.Received().RemoveClass(cssClassName);
}

    [Fact]
    public void DisposeDeregistersErrorsChangedEvent()
    {
        // Arrange
        var viewModel = Substitute.For<TestViewModel>(); // Substitute for the ViewModel
        var obj = GetObject(); // Assuming GetObject() is your method to get the object you're testing.

        // Bind the viewModel to the object (make sure this method exists on your object)
        obj.Bind(viewModel, "");

        // Act
        obj.Dispose(); // Dispose the object

        // Assert
        // Check if the event was deregistered (this depends on your implementation)
        Assert.True(viewModel.ErrorsChangedEventRemoved);
    }
}

