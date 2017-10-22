using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Riverside.UI.Forms;

namespace Riverside.UI.ApiControllers
{
    /// <summary>
    /// API controller for the retrieval (get) and submission (post) of forms.
    /// </summary>
    public class FormsController : Controller
    {
        // Member variables
        private IFormServiceFactory _formServiceFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="formServiceFactory">For the retrieval of form services given a form ID.</param>
        public FormsController(IFormServiceFactory formServiceFactory)
        {
            _formServiceFactory = formServiceFactory;
        }

        /// <summary>
        /// Retrieves form. GET: api/forms/id?context=info
        /// </summary>
        /// <param name="id">Form identifier.</param>
        /// <param name="context">Form context (null if there is no context).</param>
        /// <returns>View model used to render form.</returns>
        [HttpGet("/apps/admin/api/forms/{id}")]
        public Form Get(string id, string context = null)
        {
            IFormService formService = _formServiceFactory.GetFormService(id);
            return formService.GetForm(context);
        }

        /// <summary>
        /// Submits form. POST: api/forms/id?context=info.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        [HttpPost("/apps/admin/api/forms/{id}")]
        public FormResult Post([ModelBinder(BinderType = typeof(FormModelBinder))]Form form)
        {
            IFormService formService = _formServiceFactory.GetFormService(form.Id);
            return formService.PostForm(form);
        }
    }
}
