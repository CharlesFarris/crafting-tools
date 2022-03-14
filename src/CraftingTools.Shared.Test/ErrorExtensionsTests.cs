using System;
using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Test for the <see cref="ErrorExtensions"/> class.
/// </summary>
internal static class ErrorExtensionsTests
{
    /// <summary>
    /// Validates the behavior of the extension method used
    /// to create an <see cref="Error"/> instance from an
    /// exception.
    /// </summary>
    [Test]
    public static void ToError_Exception_ValidatesBehavior()
    {
        // use case: wrap non-null exception with default error message
        {
            var exception = new Exception(message: "exception message");
            var error = exception.ToError();
            Assert.That(error.Message, Is.EqualTo(exception.Message));
            Assert.That(error.Exception, Is.EqualTo(exception));
        }

        // use case: wrap non-null exception with an supplied error message
        {
            var exception = new Exception(message: "exception message");
            var error = exception.ToError(message: "base message");
            Assert.That(error.Message, Is.EqualTo(expected: "base message"));
            Assert.That(error.Exception, Is.EqualTo(exception));
        }

        // use case: wrap null exception with no supplied error message
        {
            var error = default(Exception).ToError();
            Assert.That(error.Message, Is.Empty);
            Assert.That(error.Exception, Is.Null);
        }
    }

    [Test]
    public static void To_Error_String_ValidatesBehavior()
    {
        // use case: wrap non-empty error message
        {
            const string message = "message";
            var error = message.ToError();
            Assert.That(error.Message, Is.EqualTo(message));
            Assert.That(error.Exception, Is.Null);
        }

        // use case: wrap empty error message
        {
            var error = default(string).ToError();
            Assert.That(error.Message, Is.EqualTo(string.Empty));
            Assert.That(error.Exception, Is.Null);
        }
    }
}