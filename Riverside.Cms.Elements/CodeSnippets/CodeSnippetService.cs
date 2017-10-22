using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.CodeSnippets
{
    public class CodeSnippetService : IAdvancedElementService
    {
        private ICodeSnippetRepository _codeSnippetRepository;

        public CodeSnippetService(ICodeSnippetRepository codeSnippetRepository)
        {
            _codeSnippetRepository = codeSnippetRepository;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("5401977d-865f-4a7a-b416-0a26305615de");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new CodeSnippetSettings { TenantId = tenantId, ElementTypeId = ElementTypeId, Language = Language.CSharp, Code = ElementResource.CodeSnippetDefaultCode };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<CodeSnippetSettings, CodeSnippetContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _codeSnippetRepository.Create((CodeSnippetSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _codeSnippetRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _codeSnippetRepository.Read((CodeSnippetSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _codeSnippetRepository.Update((CodeSnippetSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _codeSnippetRepository.Delete(tenantId, elementId, unitOfWork);
        }

        private string GetCssClass(Language language)
        {
            switch (language)
            {
                case Language.Apache:
                case Language.Bash:
                case Language.CoffeeScript:
                case Language.Css:
                case Language.Diff:
                case Language.Html:
                case Language.Http:
                case Language.Ini:
                case Language.Java:
                case Language.JavaScript:
                case Language.Json:
                case Language.Makefile:
                case Language.Markdown:
                case Language.Nginx:
                case Language.ObjectiveC:
                case Language.Perl:
                case Language.Php:
                case Language.Python:
                case Language.Ruby:
                case Language.Sql:
                case Language.Xml:
                    return language.ToString().ToLower();

                case Language.CPlusPlus:
                    return "cpp";

                case Language.CSharp:
                    return "cs";

                default:
                    return null;
            }
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            CodeSnippetSettings codeSnippetSettings = (CodeSnippetSettings)settings;
            CodeSnippetContent content = new CodeSnippetContent
            {
                PartialViewName = "CodeSnippet",
                CssClass = GetCssClass(codeSnippetSettings.Language)
            };
            return content;
        }
    }
}
