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

        private void DeployContent(long tenantId, string hostname, string filename, UploadContent content, IUnitOfWork unitOfWork)
        {
            // Create Webs folder?
            string websPath = _webHelperService.MapPath("~/webs");
            if (!Directory.Exists(websPath))
                Directory.CreateDirectory(websPath);

            // Create Tenant folder?
            string tenantPath = _webHelperService.MapPath(string.Format("~/webs/{0}", tenantId));
            if (!Directory.Exists(tenantPath))
                Directory.CreateDirectory(tenantPath);

            // Create Assets folder?
            string assetsPath = _webHelperService.MapPath(string.Format("~/webs/{0}/assets", tenantId));
            if (!Directory.Exists(assetsPath))
                Directory.CreateDirectory(assetsPath);

            // Create file
            string path = _webHelperService.MapPath(string.Format("~/webs/{0}/assets/{1}", tenantId, filename));
            using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(content.Content, 0, content.Content.Length);
            }
        }

        public void Deploy(long tenantId, string hostname, IUnitOfWork unitOfWork = null)
        {
            List<string> storageHierarchy = new List<string> { "assets" };
            List<string> uploads = _storageService.List(tenantId, storageHierarchy, unitOfWork);
            foreach (string upload in uploads)
            {
                string tempUpload = upload.Replace("%20", " "); // TODO: Is this a bug with the Azure SDK? 
                UploadContent content = _storageService.Read(tenantId, storageHierarchy, tempUpload, unitOfWork);
                DeployContent(tenantId, hostname, tempUpload, content, unitOfWork);
            }
        }

        public string GetAssetStyleSheetPath(long tenantId)
        {
            return string.Format("~/webs/{0}/assets/site.css", tenantId);
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
    }
}
