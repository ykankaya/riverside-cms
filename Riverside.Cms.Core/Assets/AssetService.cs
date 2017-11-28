using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Riverside.Cms.Core.Assets;
using Riverside.Cms.Core.Uploads;
using Riverside.UI.Web;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Assets
{
    public class AssetService : IAssetService
    {
        private IAssetRepository _assetRepository;
        private IStorageService _storageService;
        private IWebHelperService _webHelperService;

        public const string FontOptionPrefix = "option-font-";
        public const string ColourOptionPrefix = "option-colour-";

        public AssetService(IAssetRepository assetRepository, IStorageService storageService, IWebHelperService webHelperService)
        {
            _assetRepository = assetRepository;
            _storageService = storageService;
            _webHelperService = webHelperService;
        }

        public void RegisterDeployment(long tenantId, string hostname, IUnitOfWork unitOfWork = null)
        {
            _assetRepository.RegisterDeployment(tenantId, hostname, DateTime.UtcNow, unitOfWork);
        }

        public AssetDeployment ReadDeployment(long tenantId, string hostname, IUnitOfWork unitOfWork = null)
        {
            return _assetRepository.ReadDeployment(tenantId, hostname, unitOfWork);
        }

        private void DeployFile(string fileName, UploadContent content, List<string> storageHierarchy, bool useContentPath)
        {
            string relativePath = "~/" + string.Join("/", storageHierarchy) + "/" + fileName;
            string path = useContentPath ? _webHelperService.ContentPath(relativePath) : _webHelperService.MapPath(relativePath);
            using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(content.Content, 0, content.Content.Length);
            }
        }

        private void CheckCreateFolder(List<string> storageHierarchy, bool useContentPath)
        {
            string relativePath = "~";
            foreach (string folder in storageHierarchy)
            {
                relativePath = relativePath + "/" + folder;
                string path = useContentPath ? _webHelperService.ContentPath(relativePath) : _webHelperService.MapPath(relativePath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        private void DeployFiles(long tenantId, List<string> localStorageHierarchy, List<string> remoteStorageHierarchy, bool useContentPath, IUnitOfWork unitOfWork)
        {
            List<string> uploads = _storageService.List(tenantId, remoteStorageHierarchy, unitOfWork);
            if (uploads.Count == 0)
                return;
            CheckCreateFolder(localStorageHierarchy, useContentPath);
            foreach (string upload in uploads)
            {
                UploadContent content = _storageService.Read(tenantId, remoteStorageHierarchy, upload, unitOfWork);
                DeployFile(upload, content, localStorageHierarchy, useContentPath);
            }
        }

        private void DeployElements(long tenantId, IUnitOfWork unitOfWork)
        {
            List<string> localStorageHierarchy = new List<string>
            {
                "Views",
                "Tenants",
                tenantId.ToString(),
                "Elements"
            };

            List<string> remoteStorageHierarchy = new List<string>
            {
                "assets",
                "elements"
            };

            DeployFiles(tenantId, localStorageHierarchy, remoteStorageHierarchy, true, unitOfWork);
        }

        private void DeployStyles(long tenantId, IUnitOfWork unitOfWork)
        {
            List<string> localStorageHierarchy = new List<string>
            {
                "webs",
                tenantId.ToString(),
                "assets"
            };

            List<string> remoteStorageHierarchy = new List<string>
            {
                "assets"
            };

            DeployFiles(tenantId, localStorageHierarchy, remoteStorageHierarchy, false, unitOfWork);
        }

        public void Deploy(long tenantId, IUnitOfWork unitOfWork = null)
        {
            DeployStyles(tenantId, unitOfWork);
            DeployElements(tenantId, unitOfWork);
        }

        public string GetAssetStyleSheetPath(long tenantId)
        {
            return string.Format("~/webs/{0}/assets/site.css", tenantId);
        }

        public string GetElementViewPath(long tenantId)
        {
            return string.Format("~/Views/Tenants/{0}/Elements", tenantId);
        }

        public string GetFontOptionStyleSheetPath(long tenantId, string fontOption)
        {
            if (fontOption == null)
                return null;
            return string.Format("~/webs/{0}/assets/{1}{2}.css", tenantId, FontOptionPrefix, fontOption);
        }

        public string GetColourOptionStyleSheetPath(long tenantId, string colourOption)
        {
            if (colourOption == null)
                return null;
            return string.Format("~/webs/{0}/assets/{1}{2}.css", tenantId, ColourOptionPrefix, colourOption);
        }

        public List<string> GetFontOptions(long tenantId)
        {
            List<string> options = new List<string>();
            string assetsPath = _webHelperService.MapPath(string.Format("~/webs/{0}/assets", tenantId));
            string[] files = Directory.GetFiles(assetsPath, FontOptionPrefix + "*.css");
            foreach (string file in files)
            {
                string option = file.Substring(file.IndexOf(FontOptionPrefix) + FontOptionPrefix.Length);
                option = option.Substring(0, option.IndexOf(".css"));
                options.Add(option);
            }
            options.Sort();
            return options;
        }

        public List<string> GetColourOptions(long tenantId)
        {
            List<string> options = new List<string>();
            string assetsPath = _webHelperService.MapPath(string.Format("~/webs/{0}/assets", tenantId));
            string[] files = Directory.GetFiles(assetsPath, ColourOptionPrefix + "*.css");
            foreach (string file in files)
            {
                string option = file.Substring(file.IndexOf(ColourOptionPrefix) + ColourOptionPrefix.Length);
                option = option.Substring(0, option.IndexOf(".css"));
                options.Add(option);
            }
            options.Sort();
            return options;
        }

        public HashSet<Guid> GetAssetElementTypes(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IEnumerable<Guid> guids = _assetRepository.ListAssetElementTypes(tenantId, unitOfWork);
            HashSet<Guid> guidHashSet = guids.ToHashSet<Guid>();
            return guidHashSet;
        }
    }
}
