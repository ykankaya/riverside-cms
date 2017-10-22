using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Implement this interface to convert between models and view models.
    /// </summary>
    /// <typeparam name="TModel">Type of business model.</typeparam>
    /// <typeparam name="TViewModel">Type of view model.</typeparam>
    public interface IModelConverter<TModel, TViewModel>
    {
        /// <summary>
        /// Gets model, given view model.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        /// <returns>Model.</returns>
        TModel GetModel(TViewModel viewModel);

        /// <summary>
        /// Gets view model, given model.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>View model.</returns>
        TViewModel GetViewModel(TModel model);
    }
}
