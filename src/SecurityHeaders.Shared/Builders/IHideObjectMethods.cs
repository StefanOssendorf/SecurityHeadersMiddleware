using System;
using System.ComponentModel;

namespace SecurityHeaders.Builders {
    /// <summary>
    /// Interface to hide the object related methods in the fluent interfaces.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHideObjectMethods {
        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}