namespace Envelop
{
    /// <summary>
    /// A module creating a group of bindings
    /// </summary>
    public interface IModule : IBuilder
    {
        /// <summary>
        /// The name of the module
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Called to load the module bindings into the Kernel
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        void OnLoad(IKernel kernel);
    }
}