using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface for services that implement Riverside forms.
    /// </summary>
    public interface IFormService
    {
        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        Guid FormId { get; }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        Form GetForm(string context);

        /// <summary>
        /// Submits form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        FormResult PostForm(Form form);
    }
}
