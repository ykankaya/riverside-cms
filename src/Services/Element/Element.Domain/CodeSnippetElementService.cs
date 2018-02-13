using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public enum Language
    {
        Apache,
        Bash,
        CSharp,
        CPlusPlus,
        Css,
        CoffeeScript,
        Diff,
        Html,
        Http,
        Ini,
        Json,
        Java,
        JavaScript,
        Makefile,
        Markdown,
        Nginx,
        ObjectiveC,
        Php,
        Perl,
        Python,
        Ruby,
        Sql,
        Xml
    }

    public class CodeSnippetElementSettings : ElementSettings
    {
        public string Code { get; set; }
        public Language Language { get; set; }
    }

    public interface ICodeSnippetElementService
    {
        Task<CodeSnippetElementSettings> ReadElementAsync(long tenantId, long elementId);
    }

    public class CodeSnippetElementService : ICodeSnippetElementService
    {
        private readonly IElementRepository<CodeSnippetElementSettings> _elementRepository;

        public CodeSnippetElementService(IElementRepository<CodeSnippetElementSettings> elementRepository)
        {
            _elementRepository = elementRepository;
        }

        public Task<CodeSnippetElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            return _elementRepository.ReadElementAsync(tenantId, elementId);
        }
    }
}
