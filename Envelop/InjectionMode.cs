namespace Envelop
{
    /// <summary>
    /// An enum to indicate multi-injection if any
    /// </summary>
    public enum InjectionMode
    {
        /// <summary>
        /// No multi-injection
        /// </summary>
        None,
        /// <summary>
        /// Array based multi-injection
        /// </summary>
        Array,
        /// <summary>
        /// Enumerable based multi-injection
        /// </summary>
        Enumerable,
        /// <summary>
        /// List based multi-injection
        /// </summary>
        List,
        /// <summary>
        /// Return a factory method
        /// </summary>
        Factory,
        /// <summary>
        /// Return a Lazy&lt;T&gt;
        /// </summary>
        Lazy,
        /// <summary>
        /// Indicates that the requested type is a generic type
        /// </summary>
        Generic
    }
}