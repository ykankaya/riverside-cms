using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Templates;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Administration
{
    public class AdministrationService : IAdministrationService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementService _elementService;
        private IMasterPageService _masterPageService;
        private IPageService _pageService;
        private IWebService _webService;

        public AdministrationService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementService elementService, IMasterPageService masterPageService, IPageService pageService, IWebService webService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementService = elementService;
            _masterPageService = masterPageService;
            _pageService = pageService;
            _webService = webService;
        }

        private void AddDynamicContentToMasterPageZone(MasterPage masterPage, MasterPageZone masterPageZone, Guid elementTypeId, long elementId)
        {
            masterPageZone.AdminType = MasterPageZoneAdminType.Static;
            masterPageZone.MasterPageZoneElements.Add(new MasterPageZoneElement
            {
                TenantId = masterPage.TenantId,
                MasterPageId = masterPage.MasterPageId,
                MasterPageZoneId = masterPageZone.MasterPageZoneId,
                MasterPageZoneElementId = 0,
                SortOrder = 0,
                ElementId = elementId,
                Element = new ElementSettings { TenantId = masterPage.TenantId, ElementId = elementId, ElementTypeId = elementTypeId }
            });
        }

        private void AddDynamicContentToMasterPageZone(MasterPage masterPage, MasterPageZone masterPageZone, Guid elementTypeId)
        {
            AddDynamicContentToMasterPageZone(masterPage, masterPageZone, elementTypeId, 0);
        }

        private Page GetCreatePagePage(long tenantId, MasterPage adminMasterPage, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId, unitOfWork);
            Page adminPage = new Page
            {
                Name = string.Format(AdministrationResource.CreatePageName, masterPage.Name),
                Created = now,
                Description = string.Format(AdministrationResource.CreatePageDescription, masterPage.Name.ToLower()),
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("079a2393-1f20-4a6b-8cf2-1095e4828031"));
            return adminPage;
        }

        private Page GetUpdatePagePage(long tenantId, MasterPage adminMasterPage, long pageId, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page page = _pageService.Read(tenantId, pageId, unitOfWork);
            Page adminPage = new Page
            {
                Name = page.Name,
                Created = now,
                Description = AdministrationResource.UpdatePageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageId = pageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("079a2393-1f20-4a6b-8cf2-1095e4828031"));
            return adminPage;
        }

        private Page GetUpdateThemePage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AdministrationResource.UpdateThemeName,
                Created = now,
                Description = AdministrationResource.UpdateThemeDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("9602083f-d8b9-4be3-ada7-eeed6b3fd450"));
            return adminPage;
        }

        public IPageContext GetUpdateThemeContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdateThemePage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        public IPageContext GetCreatePageContext(long tenantId, long masterPageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetCreatePagePage(tenantId, adminMasterPage, masterPageId, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        public IPageContext GetUpdatePageContext(long tenantId, long pageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdatePagePage(tenantId, adminMasterPage, pageId, unitOfWork);
            Page hierarchy = _pageService.ReadHierarchy(tenantId, pageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = hierarchy,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetCreateMasterPagePage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AdministrationResource.CreateMasterPageName,
                Created = now,
                Description = AdministrationResource.CreateMasterPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("3b945a38-bb9c-41ff-9e1a-42607b75458e"));
            return adminPage;
        }

        public IPageContext GetCreateMasterPageContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetCreateMasterPagePage(tenantId, adminMasterPage, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetUpdateMasterPagePage(long tenantId, MasterPage adminMasterPage, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId, unitOfWork);
            Page adminPage = new Page
            {
                Name = string.Format(AdministrationResource.UpdateMasterPageName, masterPage.Name.ToLower()),
                Created = now,
                Description = AdministrationResource.UpdateMasterPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("3b945a38-bb9c-41ff-9e1a-42607b75458e"));
            return adminPage;
        }

        public IPageContext GetUpdateMasterPageContext(long tenantId, long masterPageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdateMasterPagePage(tenantId, adminMasterPage, masterPageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetUpdateMasterPageZonePage(long tenantId, MasterPage adminMasterPage, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId, unitOfWork);
            MasterPageZone masterPageZone = masterPage.MasterPageZones.Where(mpz => mpz.MasterPageZoneId == masterPageZoneId).FirstOrDefault();
            Page adminPage = new Page
            {
                Name = string.Format(AdministrationResource.UpdateMasterPageZoneName, masterPage.Name.ToLower(), masterPageZone.Name.ToLower()),
                Created = now,
                Description = AdministrationResource.UpdateMasterPageZoneDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("24281fa2-edad-4af9-9f60-e4fc869061c5"));
            return adminPage;
        }

        public IPageContext GetUpdateMasterPageZoneContext(long tenantId, long masterPageId, long masterPageZoneId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdateMasterPageZonePage(tenantId, adminMasterPage, masterPageId, masterPageZoneId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetUpdateMasterPageZonesPage(long tenantId, MasterPage adminMasterPage, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId, unitOfWork);
            Page adminPage = new Page
            {
                Name = string.Format(AdministrationResource.UpdateMasterPageZonesName, masterPage.Name.ToLower()),
                Created = now,
                Description = AdministrationResource.UpdateMasterPageZoneDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("6e628c1b-7876-436c-b2d0-ac6a4859d507"));
            return adminPage;
        }

        public IPageContext GetUpdateMasterPageZonesContext(long tenantId, long masterPageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdateMasterPageZonesPage(tenantId, adminMasterPage, masterPageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetUpdatePageElementPage(long tenantId, long pageId, long elementId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            IElementSettings settings = _elementService.Read(tenantId, elementId, false, unitOfWork);
            Page page = _pageService.Read(tenantId, pageId, unitOfWork);
            Page adminPage = new Page
            {
                Name = string.Format(AdministrationResource.UpdatePageElementPageName, page.Name.ToLower(), settings.Name.ToLower()),
                Created = now,
                MasterPageId = adminMasterPage.MasterPageId,
                PageId = pageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, settings.ElementTypeId, elementId);
            return adminPage;
        }

        public IPageContext GetUpdatePageElementContext(long tenantId, long pageId, long elementId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdatePageElementPage(tenantId, pageId, elementId, adminMasterPage, unitOfWork);
            Page hierarchy = _pageService.ReadHierarchy(tenantId, pageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = hierarchy,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetUpdateMasterPageElementPage(long tenantId, long masterPageId, long elementId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            IElementSettings settings = _elementService.Read(tenantId, elementId, false, unitOfWork);
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId, unitOfWork);
            Page page = _pageService.ReadHome(tenantId, unitOfWork);
            Page adminPage = new Page
            {
                Name = string.Format(AdministrationResource.UpdateMasterPageElementPageName, masterPage.Name.ToLower(), settings.Name.ToLower()),
                Created = now,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                PageId = page.PageId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, settings.ElementTypeId, elementId);
            return adminPage;
        }

        public IPageContext GetUpdateMasterPageElementContext(long tenantId, long masterPageId, long elementId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdateMasterPageElementPage(tenantId, masterPageId, elementId, adminMasterPage, unitOfWork);
            Page hierarchy = _pageService.ReadHierarchy(tenantId, page.PageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                Hierarchy = hierarchy,
                MasterPage = adminMasterPage,
                Page = page,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetUpdatePageZonePage(long tenantId, long pageId, long pageZoneId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page page = _pageService.Read(tenantId, pageId, unitOfWork);
            Page adminPage = new Page
            {
                Name = page.Name,
                Created = now,
                Description = AdministrationResource.UpdatePageZoneDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageId = pageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("2835ba3d-0e3d-4820-8c94-fbfba293c6f0"));
            return adminPage;
        }

        public IPageContext GetUpdatePageZoneContext(long tenantId, long pageId, long pageZoneId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdatePageZonePage(tenantId, pageId, pageZoneId, adminMasterPage, unitOfWork);
            Page hierarchy = _pageService.ReadHierarchy(tenantId, pageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = hierarchy,
                Parameters = parameters,
                Tags = tags,
                Web = web,
                UpdateElement = true
            };
        }

        private Page GetCreateUserPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.CreateUserPageName,
                Created = now,
                Description = AuthenticationResource.CreateUserPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("3422b3dd-6442-40da-9e6e-4a5036cf1a87"));
            return adminPage;
        }

        public IPageContext GetCreateUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetCreateUserPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetConfirmUserSetPasswordPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.ConfirmUserSetPasswordPageName,
                Created = now,
                Description = AuthenticationResource.ConfirmUserSetPasswordPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("8d71180f-9a1b-4711-b60c-90e4ea8d1c2e"));
            return adminPage;
        }

        public IPageContext GetConfirmUserSetPasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetConfirmUserSetPasswordPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetConfirmUserPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.ConfirmUserPageName,
                Created = now,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("b2ab475d-39f7-4ae2-ab85-6e0bba6cd9f2"));
            return adminPage;
        }

        public IPageContext GetConfirmUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetConfirmUserPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetLogonUserPage(Web web, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.LogonUserPageName,
                Created = now,
                Description = string.Format(AuthenticationResource.LogonUserPageDescription, web.Name),
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = web.TenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("5c17be3a-415a-4367-80bd-9406d527664c"));
            return adminPage;
        }

        public IPageContext GetLogonUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            Page page = GetLogonUserPage(web, adminMasterPage, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetChangePasswordPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.ChangePasswordPageName,
                Created = now,
                Description = AuthenticationResource.ChangePasswordPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("65ddc9df-922b-46e0-b7aC-313ddd2da828"));
            return adminPage;
        }

        public IPageContext GetChangePasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetChangePasswordPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetUpdateUserPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.UpdateUserPageName,
                Created = now,
                Description = AuthenticationResource.UpdateUserPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("92391b8c-55ff-41d7-81c6-15970a319dde"));
            return adminPage;
        }

        public IPageContext GetUpdateUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetUpdateUserPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetForgottenPasswordPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.ForgottenPasswordPageName,
                Created = now,
                Description = AuthenticationResource.ForgottenPasswordPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("e7b223b4-3b83-4b3c-8a3c-11fa8c44dda5"));
            return adminPage;
        }

        public IPageContext GetForgottenPasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetForgottenPasswordPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private Page GetResetPasswordPage(long tenantId, MasterPage adminMasterPage, IUnitOfWork unitOfWork = null)
        {
            DateTime now = DateTime.UtcNow;
            Page adminPage = new Page
            {
                Name = AuthenticationResource.ResetPasswordPageName,
                Created = now,
                Description = AuthenticationResource.ResetPasswordPageDescription,
                MasterPageId = adminMasterPage.MasterPageId,
                PageZones = new List<PageZone>(),
                Updated = now,
                TenantId = tenantId,
                ChildPages = new List<Page>()
            };
            MasterPageZone mainMasterPageZone = adminMasterPage.MasterPageZones.Where(mpz => mpz.ContentType == MasterPageZoneContentType.Main).FirstOrDefault();
            AddDynamicContentToMasterPageZone(adminMasterPage, mainMasterPageZone, new Guid("9a0bd0fd-f15c-431a-a51a-a228a96a364b"));
            return adminPage;
        }

        public IPageContext GetResetPasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            MasterPage adminMasterPage = _masterPageService.ReadAdministration(tenantId, unitOfWork);
            Page page = GetResetPasswordPage(tenantId, adminMasterPage, unitOfWork);
            Page homePage = _pageService.ReadHome(tenantId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            homePage.ChildPages.Add(page);
            return new PageContext
            {
                MasterPage = adminMasterPage,
                Page = page,
                Hierarchy = homePage,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        public IPageContext GetPageContext(long tenantId, long pageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null)
        {
            Page page = _pageService.Read(tenantId, pageId, unitOfWork);
            MasterPage masterPage = _masterPageService.Read(tenantId, page.MasterPageId, unitOfWork);
            Page hierarchy = _pageService.ReadHierarchy(tenantId, pageId, unitOfWork);
            Web web = _webService.Read(tenantId, unitOfWork);
            return new PageContext
            {
                Page = page,
                MasterPage = masterPage,
                Hierarchy = hierarchy,
                Parameters = parameters,
                Tags = tags,
                Web = web
            };
        }

        private IAdministrationOptionGroup GetWebGroup(IUnitOfWork unitOfWork)
        {
            if (!_authorizationService.UserInFunction(Functions.UpdateTheme))
                return null;

            AdministrationOptionGroup group = new AdministrationOptionGroup
            {
                Group = AdministrationGroup.Site,
                Sections = new List<IAdministrationOptionSection>
                {
                    new AdministrationOptionSection
                    {
                        Name = AdministrationResource.ThemeSectionLabel,
                        Options = new List<IAdministrationOption>
                        {
                            new UpdateThemeOption
                            {
                                Name = AdministrationResource.FontsAndColoursOptionLabel
                            }
                        }
                    }
                }
            };
            return group;
        }

        private IAdministrationOptionSection GetCreatePageSection(long tenantId, IUnitOfWork unitOfWork)
        {
            // Check user in function that lets them create pages
            if (!_authorizationService.UserInFunction(Functions.CreatePages))
                return null;

            // Get list of of master pages that can be created for the given website
            IEnumerable<MasterPage> masterPages = _masterPageService.ListCreatable(tenantId, unitOfWork);
            if (masterPages.Count() == 0)
                return null;

            // Construct administration group based on master page list
            IAdministrationOptionSection section = new AdministrationOptionSection
            {
                Name = AdministrationResource.CreatePageSectionLabel,
                Options = new List<IAdministrationOption>()
            };
            foreach (MasterPage masterPage in masterPages)
                section.Options.Add(new CreatePageOption { MasterPageId = masterPage.MasterPageId, Name = string.Format(AdministrationResource.CreatePageOptionLabel, masterPage.Name) });

            // Return result
            return section;
        }

        private IAdministrationOptionGroup GetLoggedOffUserGroup(Web web)
        {
            List<IAdministrationOption> options = new List<IAdministrationOption>();
            options.Add(new LogonUserOption { Name = AdministrationResource.LogonUserLabel });
            if (web.CreateUserEnabled)
                options.Add(new CreateUserOption { Name = AdministrationResource.CreateUserLabel });
            return new AdministrationOptionGroup
            {
                Group = AdministrationGroup.LoggedOffUser,
                Sections = new List<IAdministrationOptionSection> {
                    new AdministrationOptionSection {
                        Name = AdministrationResource.LoggedOffUserSectionLabel,
                        Options = options
                    }
                }
            };
        }

        private IAdministrationOptionGroup GetLoggedOnUserGroup(AuthenticatedUserInfo userInfo)
        {
            return new AdministrationOptionGroup
            {
                Group = AdministrationGroup.LoggedOnUser,
                Sections = new List<IAdministrationOptionSection> {
                    new AdministrationOptionSection {
                        Name = string.Format(AdministrationResource.LoggedOnUserSectionLabel, userInfo.User.Alias),
                        Options = new List<IAdministrationOption>{
                            new UpdateUserOption     { Name = AdministrationResource.UpdateUserLabel     },
                            new ChangePasswordOption { Name = AdministrationResource.ChangePasswordLabel },
                            new LogoffUserOption     { Name = AdministrationResource.LogoffUserLabel     }
                        }
                    }
                }
            };
        }

        private IAdministrationOptionSection GetUpdatePageContentSection(List<IElementInfo> pageElements, long pageId, IUnitOfWork unitOfWork)
        {
            if (!_authorizationService.UserInFunction(Functions.UpdatePageElements))
                return null;
            if (pageElements.Count == 0)
                return null;
            IAdministrationOptionSection section = new AdministrationOptionSection
            {
                Name = AdministrationResource.UpdatePageContentSectionLabel,
                Options = new List<IAdministrationOption>()
            };
            foreach (IElementInfo element in pageElements)
                section.Options.Add(new UpdatePageElementOption { PageId = pageId, ElementId = element.Settings.ElementId, ElementTypeId = element.Settings.ElementTypeId, Name = element.Settings.Name });
            return section;
        }

        private IAdministrationOptionSection GetUpdateMasterPageContentSection(List<IElementInfo> masterElements, long masterPageId)
        {
            if (!_authorizationService.UserInFunction(Functions.UpdateMasterPageElements))
                return null;
            if (masterElements.Count == 0)
                return null;
            IAdministrationOptionSection section = new AdministrationOptionSection
            {
                Name = AdministrationResource.UpdateMasterPageContentSectionLabel,
                Options = new List<IAdministrationOption>()
            };
            foreach (IElementInfo element in masterElements)
                section.Options.Add(new UpdateMasterPageElementOption { MasterPageId = masterPageId, ElementId = element.Settings.ElementId, ElementTypeId = element.Settings.ElementTypeId, Name = element.Settings.Name });
            return section;
        }

        private IAdministrationOptionGroup GetPageGroup(Page page, List<IElementInfo> pageElements, List<IElementInfo> masterElements, List<PageZone> configurablePageZones, IUnitOfWork unitOfWork)
        {
            IAdministrationOptionGroup group = new AdministrationOptionGroup
            {
                Group = AdministrationGroup.Page,
                Sections = new List<IAdministrationOptionSection>()
            };
            IAdministrationOptionSection createPageSection = GetCreatePageSection(page.TenantId, unitOfWork);
            if (createPageSection != null)
                group.Sections.Add(createPageSection);
            if (_authorizationService.UserInFunction(Functions.UpdatePages))
            {
                IAdministrationOptionSection updatePageSection = new AdministrationOptionSection
                {
                    Name = AdministrationResource.UpdatePageSectionLabel,
                    Options = new List<IAdministrationOption> {
                        new UpdatePageOption { PageId = page.PageId, Name = string.Format(AdministrationResource.UpdatePageOptionLabel, page.Name) }
                    }
                };
                group.Sections.Add(updatePageSection);
            }
            IAdministrationOptionSection updatePageZoneSection = GetUpdatePageZonesSection(configurablePageZones, unitOfWork);
            if (updatePageZoneSection != null)
                group.Sections.Add(updatePageZoneSection);
            IAdministrationOptionSection updatePageContentSection = GetUpdatePageContentSection(pageElements, page.PageId, unitOfWork);
            if (updatePageContentSection != null)
                group.Sections.Add(updatePageContentSection);
            if (group.Sections.Count == 0)
                return null;
            return group;
        }

        private IAdministrationOptionSection GetUpdateMasterPageZonesSection(MasterPage masterPage)
        {
            if (!_authorizationService.UserInFunction(Functions.UpdateMasterPages))
                return null;
            if (masterPage.MasterPageZones.Count == 0)
                return null;
            IAdministrationOptionSection section = new AdministrationOptionSection
            {
                Name = AdministrationResource.UpdateMasterPageZonesSectionLabel,
                Options = new List<IAdministrationOption>()
            };
            foreach (MasterPageZone masterPageZone in masterPage.MasterPageZones)
            {
                section.Options.Add(new UpdateMasterPageZoneOption
                {
                    MasterPageId = masterPage.MasterPageId,
                    MasterPageZoneId = masterPageZone.MasterPageZoneId,
                    Name = string.Format(AdministrationResource.UpdateMasterPageZoneOptionLabel, masterPageZone.Name)
                });
            }
            return section;
        }

        private IAdministrationOptionGroup GetMasterPageGroup(MasterPage masterPage, Page page, List<IElementInfo> masterElements)
        {
            // Construct group
            IAdministrationOptionGroup group = new AdministrationOptionGroup
            {
                Group = AdministrationGroup.MasterPage,
                Sections = new List<IAdministrationOptionSection>()
            };

            // Add administration option section for creating new master pages
            if (_authorizationService.UserInFunction(Functions.CreateMasterPages))
            {
                IAdministrationOptionSection createMasterPageSection = new AdministrationOptionSection
                {
                    Name = AdministrationResource.CreateMasterPageSectionLabel,
                    Options = new List<IAdministrationOption> {
                        new CreateMasterPageOption { Name = AdministrationResource.CreateMasterPageOptionLabel }
                    }
                };
                group.Sections.Add(createMasterPageSection);
            }

            // Add administration option section for updating master page (details and zone management)
            if (_authorizationService.UserInFunction(Functions.UpdateMasterPages))
            {
                IAdministrationOptionSection updateMasterPageSection = new AdministrationOptionSection
                {
                    Name = AdministrationResource.UpdateMasterPageSectionLabel,
                    Options = new List<IAdministrationOption> {
                        new UpdateMasterPageOption { MasterPageId = page.MasterPageId, Name = AdministrationResource.UpdateMasterPageOptionLabel },
                        new UpdateMasterPageZonesOption { MasterPageId = page.MasterPageId, Name = AdministrationResource.UpdateMasterPageZonesOptionLabel }
                    }
                };
                group.Sections.Add(updateMasterPageSection);
            }

            // Add administration option section for updating a master page zone
            IAdministrationOptionSection updateMasterPageZonesSection = GetUpdateMasterPageZonesSection(masterPage);
            if (updateMasterPageZonesSection != null)
                group.Sections.Add(updateMasterPageZonesSection);

            // Add administration option section for updating (site wide) master page content
            IAdministrationOptionSection updateMasterPageContentSection = GetUpdateMasterPageContentSection(masterElements, masterPage.MasterPageId);
            if (updateMasterPageContentSection != null)
                group.Sections.Add(updateMasterPageContentSection);

            // Return result
            if (group.Sections.Count == 0)
                return null;
            return group;
        }

        private IAdministrationOptionSection GetUpdatePageZonesSection(List<PageZone> pageZones, IUnitOfWork unitOfWork)
        {
            if (!_authorizationService.UserInFunction(Functions.UpdatePageElements))
                return null;
            if (pageZones.Count == 0)
                return null;
            IAdministrationOptionSection section = new AdministrationOptionSection
            {
                Name = AdministrationResource.UpdatePageZonesSectionLabel,
                Options = new List<IAdministrationOption>()
            };
            foreach (PageZone pageZone in pageZones)
                section.Options.Add(new UpdatePageZoneOption { PageId = pageZone.PageId, PageZoneId = pageZone.PageZoneId, Name = string.Format(AdministrationResource.UpdatePageZoneOptionLabel, pageZone.MasterPageZone.Name) });
            return section;
        }

        public IAdministrationOptions GetAdministrationOptions(IPageContext pageContext, List<IElementInfo> pageElements, List<IElementInfo> masterElements, List<PageZone> configurablePageZones, IUnitOfWork unitOfWork = null)
        {
            AuthenticatedUserInfo userInfo = _authenticationService.GetCurrentUser();
            IAdministrationOptions options = new AdministrationOptions { Groups = new List<IAdministrationOptionGroup>(), LoggedOnUser = userInfo };
            if (userInfo != null && !pageContext.MasterPage.Administration)
            {
                IAdministrationOptionGroup webGroup = GetWebGroup(unitOfWork);
                if (webGroup != null)
                    options.Groups.Add(webGroup);
                IAdministrationOptionGroup masterPageGroup = GetMasterPageGroup(pageContext.MasterPage, pageContext.Page, masterElements);
                if (masterPageGroup != null)
                    options.Groups.Add(masterPageGroup);
                IAdministrationOptionGroup pageGroup = GetPageGroup(pageContext.Page, pageElements, masterElements, configurablePageZones, unitOfWork);
                if (pageGroup != null)
                    options.Groups.Add(pageGroup);
            }
            if (userInfo == null)
                options.Groups.Add(GetLoggedOffUserGroup(pageContext.Web));
            if (userInfo != null)
                options.Groups.Add(GetLoggedOnUserGroup(userInfo));
            return options;
        }
    }
}
