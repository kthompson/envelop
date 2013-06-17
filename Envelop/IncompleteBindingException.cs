using System;

namespace Envelop
{
    /// <summary>
    /// This exception can occur when a binding was not completed.
    /// </summary>
    public class IncompleteBindingException : InvalidOperationException
    {
    }

    /// <summary>
    /// This exception can occur when a binding is not found.
    /// </summary>
    public class BindingNotFoundException : InvalidOperationException
    {
    }
}