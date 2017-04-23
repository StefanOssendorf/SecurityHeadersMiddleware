using System.ComponentModel;

namespace SecurityHeaders.Builders {

    /// <summary>
    /// Fluent interface to build the settings object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentBuilder<T> : IHideObjectMethods {

        /// <summary>
        /// Builds the settings object.
        /// </summary>
        /// <returns>The settings object. Never <code>null</code>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        T Build();
    }
}