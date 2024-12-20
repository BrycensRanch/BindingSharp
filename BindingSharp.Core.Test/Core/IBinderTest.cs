using Xunit;

namespace BindingSharp.Core.Test.Core;

public abstract class IBinderTest
{
    internal abstract IBinder GetObject();

    [Fact]
    public void BindThrowsArgumentNullExceptionIfTargetIsNull()
    {
        var obj = GetObject();

        var exception = Assert.Throws<ArgumentNullException>(() => obj.Bind(null, ""));
        Assert.Equal("target", exception.ParamName); // Assuming you have validation that throws ArgumentNullException with "target" param name
    }

    [Fact]
    public void BindThrowsBindingExceptionIfAttributeIsNotFoundInTarget()
    {
        var obj = GetObject();

        var exception = Assert.Throws<BindingException>(() => obj.Bind(new object(), "Invalid"));
        Assert.Equal("Invalid", exception.Message);
    }
}

