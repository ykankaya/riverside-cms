using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Elements.Albums;
using Riverside.Cms.Elements.Authentication;
using Riverside.Cms.Elements.Carousels;
using Riverside.Cms.Elements.CodeSnippets;
using Riverside.Cms.Elements.Contacts;
using Riverside.Cms.Elements.Footers;
using Riverside.Cms.Elements.Forms;
using Riverside.Cms.Elements.Forums;
using Riverside.Cms.Elements.Html;
using Riverside.Cms.Elements.LatestThreads;
using Riverside.Cms.Elements.Maps;
using Riverside.Cms.Elements.NavBars;
using Riverside.Cms.Elements.PageHeaders;
using Riverside.Cms.Elements.PageList;
using Riverside.Cms.Elements.Pages;
using Riverside.Cms.Elements.Shares;
using Riverside.Cms.Elements.Tables;
using Riverside.Cms.Elements.TagCloud;
using Riverside.Cms.Elements.TestimonialCarousels;
using Riverside.Cms.Elements.Testimonials;
using Riverside.Cms.Elements.Themes;
using Riverside.Utilities.Reflection;

namespace WebApplication.Services
{
    public class ListElementServices : IListElementServices
    {
        public readonly IReflectionService _reflectionService;

        public ListElementServices(IReflectionService reflectionService)
        {
            _reflectionService = reflectionService;
        }

        public IEnumerable<Type> ListTypes()
        {
            string assemblyFolderPath = _reflectionService.GetExecutingAssemblyFolderPath();
            string[] assemblyPaths = Directory.GetFiles(assemblyFolderPath, "*.dll");
            return _reflectionService.GetTypesThatImplementInterface<IBasicElementService>(assemblyPaths);
        }
    }
}