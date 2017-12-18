using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Core.Domain;

namespace Core.API.Controllers
{
    public class MasterPagesController : Controller
    {
        private readonly IMasterPageService _masterPageService;

        public MasterPagesController(IMasterPageService masterPageService)
        {
            _masterPageService = masterPageService;
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/masterpages/{masterPageId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MasterPage), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadMasterPage(long tenantId, long masterPageId)
        {
            MasterPage masterPage = await _masterPageService.ReadMasterPageAsync(tenantId, masterPageId);
            if (masterPage == null)
                return NotFound();
            return Ok(masterPage);
        }
    }
}
