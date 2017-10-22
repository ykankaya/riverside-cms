using System;
using System.Collections.Generic;
using System.Linq;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Templates;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Converts between web business models and web view models.
    /// </summary>
    public class WebConverter : IModelConverter<Web, WebViewModel>
    {
        // Member variables
        private IDataAnnotationsService _dataAnnotationsService;
        private ITemplateRepository _templateRepository;

        /// <summary>
        /// Constructor sets dependent comnponents.
        /// </summary>
        /// <param name="dataAnnotationsService">Data annotations service.</param>
        /// <param name="templateRepository">Template repository.</param>
        public WebConverter(IDataAnnotationsService dataAnnotationsService, ITemplateRepository templateRepository)
        {
            _dataAnnotationsService = dataAnnotationsService;
            _templateRepository = templateRepository;
        }

        /// <summary>
        /// Gets web model, given web view model.
        /// </summary>
        /// <param name="viewModel">Web view model.</param>
        /// <returns>Web model.</returns>
        public Web GetModel(WebViewModel viewModel)
        {
            Web web = new Web
            {
                Name = viewModel.Name.Value,
                Domains = new List<Domain> { new Domain { Url = viewModel.Domain.Value } }
            };
            return web;
        }

        /// <summary>
        /// Gets web view model, given web model.
        /// </summary>
        /// <param name="model">Web model.</param>
        /// <returns>Web view model.</returns>
        public WebViewModel GetViewModel(Web model)
        {
            // Construct view model
            WebViewModel viewModel = new WebViewModel();

            // Website name field
            viewModel.Name = new TextField
            {
                Name = "name",
                Label = WebResource.NameLabel,
                Value = model.Name,
                Required = true,
                RequiredErrorMessage = WebResource.NameRequiredMessage,
                MaxLength = WebLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(WebResource.NameMaxLengthMessage, "name", WebLengths.NameMaxLength)
            };

            // Website domain field
            Domain domain = model.Domains != null ? model.Domains.FirstOrDefault() : null;
            viewModel.Domain = new TextField
            {
                Name = "domain",
                Label = DomainResource.UrlLabel,
                Required = true,
                RequiredErrorMessage = DomainResource.UrlRequiredMessage,
                MaxLength = DomainLengths.UrlMaxLength,
                MaxLengthErrorMessage = DomainResource.UrlMaxLengthMessage,
                Pattern = RegularExpression.Url,
                PatternErrorMessage = DomainResource.UrlInvalidMessage,
                Value = domain != null ? domain.Url : null
            };

            // Setup template field
            viewModel.Template = new SelectListField<string>
            {
                Name = "template",
                Label = WebResource.TemplateLabel,
                Items = new List<ListFieldItem<string>>(),
                Required = true,
                RequiredErrorMessage = WebResource.TemplateRequiredMessage
            };
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 100, Search = string.Empty };
            ISearchResult<Template> searchResult = _templateRepository.Search(searchParameters);
            viewModel.Template.Items.Add(new ListFieldItem<string> { Name = WebResource.SelectTemplateLabel, Value = null });
            foreach (Template template in searchResult.Items)
                viewModel.Template.Items.Add(new ListFieldItem<string> { Name = template.Name, Value = template.TenantId.ToString() });

            // Return result
            return viewModel;
        }
    }
}
