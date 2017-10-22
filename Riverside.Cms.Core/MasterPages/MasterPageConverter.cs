using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Riverside.UI.Forms;
using Riverside.Utilities.Annotations;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Converts between master page business models and master page view models.
    /// </summary>
    public class MasterPageConverter : IModelConverter<MasterPage, MasterPageViewModel>
    {
        // Member variables
        private IDataAnnotationsService _dataAnnotationsService;

        /// <summary>
        /// Constructor sets dependent comnponents.
        /// </summary>
        /// <param name="dataAnnotationsService">Data annotations service.</param>
        public MasterPageConverter(IDataAnnotationsService dataAnnotationsService)
        {
            _dataAnnotationsService = dataAnnotationsService;
        }

        /// <summary>
        /// Gets master page model, given master page view model.
        /// </summary>
        /// <param name="viewModel">Master page view model.</param>
        /// <returns>Master page model.</returns>
        public MasterPage GetModel(MasterPageViewModel viewModel)
        {
            throw new NotImplementedException(); // TODO: Code this
        }

        /// <summary>
        /// Gets master page view model, given master page model.
        /// </summary>
        /// <param name="model">Master page model.</param>
        /// <returns>Master page view model.</returns>
        public MasterPageViewModel GetViewModel(MasterPage model)
        {
            throw new NotImplementedException(); // TODO: Code this
        }
    }
}
