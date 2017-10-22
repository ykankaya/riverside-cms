using System.Collections.Generic;
using Riverside.Cms.Core.Administration;
using Riverside.UI.Grids;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Interface for services that implement page related portal functionality.
    /// </summary>
    public interface IPagePortalService
    {
        /// <summary>
        /// Gets view model for page search.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>View model for page search.</returns>
        AdminPageViewModel<Grid> GetSearchViewModel(long tenantId, int? page, string search);

        /// <summary>
        /// Gets search grid.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>Grid view model.</returns>
        Grid GetSearchGrid(long tenantId, int? page, string search);

        /// <summary>
        /// Retrieves page for display.
        /// </summary>
        /// <param name="tenantId">Website whose page is read.</param>
        /// <param name="pageId">Unique page identifier.</param>
        /// <returns>View model required to render a page.</returns>
        PageViewModel GetReadPagePageViewModel(long tenantId, long pageId);

        /// <summary>
        /// Retrieves home page for display.
        /// </summary>
        /// <param name="tenantId">Website whose page is read.</param>
        /// <returns>View model required to render a page.</returns>
        PageViewModel GetHomePagePageViewModel(long tenantId);

        /// <summary>
        /// Retrieves page for update theme administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdateThemePageViewModel(long tenantId);

        /// <summary>
        /// Retrieves page for create page administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The type of page that is to be created.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetCreatePagePageViewModel(long tenantId, long masterPageId);

        /// <summary>
        /// Retrieves page for update page administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="pageId">The page that is being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdatePagePageViewModel(long tenantId, long pageId);

        /// <summary>
        /// Gets page view model for creating master pages.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetCreateMasterPagePageViewModel(long tenantId);

        /// <summary>
        /// Gets page view model for update of master page.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The master page that is being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdateMasterPagePageViewModel(long tenantId, long masterPageId);

        /// <summary>
        /// Gets page view model for update of master page zone.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The master page whose zone is being updated.</param>
        /// <param name="masterPageZoneId">The master page zone that is being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdateMasterPageZonePageViewModel(long tenantId, long masterPageId, long masterPageZoneId);

        /// <summary>
        /// Gets page view model for add and remove of master page zones.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The master page whose zones are being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdateMasterPageZonesPageViewModel(long tenantId, long masterPageId);

        /// <summary>
        /// Retrieves page for update page element administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">The website whose element is being updated.</param>
        /// <param name="pageId">The page that we were on when update element action was selected.</param>
        /// <param name="elementId">Identifies element being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdatePageElementPageViewModel(long tenantId, long pageId, long elementId);

        /// <summary>
        /// Retrieves page for update master page element administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">The website whose element is being updated.</param>
        /// <param name="masterPageId">The master page that we were on when update element action was selected.</param>
        /// <param name="elementId">Identifies element being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdateMasterPageElementPageViewModel(long tenantId, long masterPageId, long elementId);

        /// <summary>
        /// Retrieves page for update page zone administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">The website whose page zone is being updated.</param>
        /// <param name="pageId">The page that we were on when update page zone action was selected.</param>
        /// <param name="pageZoneId">Identifies page zone being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        PageViewModel GetUpdatePageZonePageViewModel(long tenantId, long pageId, long pageZoneId);

        /// <summary>
        /// Gets a page view model for the register user page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetCreateUserPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for the confirm user set password page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetConfirmUserSetPasswordPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for the confirm user page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetConfirmUserPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for the logon page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetLogonUserPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for the change password page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetChangePasswordPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for the updating a user profile.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetUpdateUserPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for initiating a password reset.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetForgottenPasswordPageViewModel(long tenantId);

        /// <summary>
        /// Gets a page view model for reseting a user's password.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        PageViewModel GetResetPasswordPageViewModel(long tenantId);

        /// <summary>
        /// Converts tags in a text string into a list of strongly typed tags.
        /// </summary>
        /// <param name="separators">Separators used to split text string of tags.</param>
        /// <param name="text">Text string containing tags.</param>
        /// <returns>List of tags.</returns>
        IList<Tag> GetTagsFromTextString(string[] separators, string text);

        /// <summary>
        /// Converts list of tags into a space separated string of tag names.
        /// </summary>
        /// <param name="separator">Separator used to split tags.</param>
        /// <param name="tags">List of tags.</param>
        /// <returns>Space separated list of tag names.</returns>
        string GetTagsAsTextString(string separator, IList<Tag> tags);

        /// <summary>
        /// Gets URL for page.
        /// </summary>
        /// <param name="pageId">The page whose URL is returned.</param>
        /// <param name="name">Page name.</param>
        /// <returns>URL for page.</returns>
        string GetPageUrl(long pageId, string name);

        /// <summary>
        /// Gets URL for home page.
        /// </summary>
        /// <returns>URL for page.</returns>
        string GetHomeUrl();
    }
}
