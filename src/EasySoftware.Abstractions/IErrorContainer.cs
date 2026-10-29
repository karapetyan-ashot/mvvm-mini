using System.Collections.Generic;

namespace EasySoftware.Abstractions
{
    /// <summary>
    /// Stores validation errors, either for the whole object or for individual properties.
    /// </summary>
    /// <remarks>
    /// Errors that belong to the whole object are stored under the <see cref="string.Empty"/> key.
    /// </remarks>
    public interface IErrorContainer
	{
		/// <summary>
		/// Gets a value indicating whether the container holds at least one error.
		/// </summary>
		bool HasErrors { get; }

		/// <summary>
		/// Gets or sets all errors, keyed by property name.
		/// </summary>
		/// <remarks>
		/// Object-level errors use the <see cref="string.Empty"/> key.
		/// Setting this property replaces the current errors with a copy of the given ones.
		/// </remarks>
		IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; set; }

		/// <summary>
		/// Adds an object-level error that is not tied to a specific property.
		/// </summary>
		/// <param name="errorMessage">The error message to add.</param>
		void AddError(string errorMessage);

		/// <summary>
		/// Adds an error for the specified property.
		/// </summary>
		/// <param name="propName">The property name, or <see langword="null"/> or empty for an object-level error.</param>
		/// <param name="errorMessage">The error message to add.</param>
		void AddError(string propName, string errorMessage);

		/// <summary>
		/// Removes all errors.
		/// </summary>
		void ClearErrors();

		/// <summary>
		/// Removes all errors of the specified property.
		/// </summary>
		/// <param name="propName">The property name, or <see langword="null"/> or empty for object-level errors.</param>
		void ClearErrors(string propName);

		/// <summary>
		/// Replaces the current errors with a copy of the errors of another container.
		/// </summary>
		/// <param name="other">The container to copy the errors from.</param>
		void CloneErrors(IErrorContainer other);
	}
}