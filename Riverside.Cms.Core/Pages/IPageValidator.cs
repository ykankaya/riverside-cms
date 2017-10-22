using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Interface for classes that validate page actions.
    /// </summary>
    public interface IPageValidator
    {
        /// <summary>
        /// Validates create page model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="parentPageId">Parent page (can be null if page has no parent - i.e. is home page, or if can be determined).</param>
        /// <param name="pageInfo">If specified, used to override master page settings.</param>
        /// <param name="parentPages">If master has ancestor page specified, will contain list of valid parent pages.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateCreate(long tenantId, long? parentPageId, PageInfo pageInfo, List<Page> parentPages, string keyPrefix = null);

        /// <summary>
        /// Validates update page model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="page">Updated page details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateUpdate(Page page, string keyPrefix = null);

        /// <summary>
        /// Validates page, master page and file upload details before images can be prepared for a page.
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="masterPageId">Master page containing image upload rules.</param>
        /// <param name="model">Uploaded image.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Useful information retrieved during validation.</returns>
        ValidatePrepareImagesResult ValidatePrepareImages(long tenantId, long masterPageId, CreateUploadModel model, string keyPrefix = null);

        /// <summary>
        /// Validates update of page zone.
        /// </summary>
        /// <param name="masterPage">Master page associated with page whose zone is being updated.</param>
        /// <param name="page">Page whose zone is being updated.</param>
        /// <param name="pageZoneId">Identifier of page zone being updated.</param>
        /// <param name="pageZoneElements">Determines the content in a page zone.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateUpdateZone(MasterPage masterPage, Page page, long pageZoneId, List<PageZoneElementInfo> pageZoneElements, string keyPrefix = null);
    }
}
