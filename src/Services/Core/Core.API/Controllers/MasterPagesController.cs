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

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/masterpages/{masterPageId:int}/zones")]
        [ProducesResponseType(typeof(IEnumerable<MasterPageZone>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchMasterPageZones(long tenantId, long masterPageId)
        {
            IEnumerable<MasterPageZone> masterPageZones = await _masterPageService.SearchMasterPageZonesAsync(tenantId, masterPageId);
            return Ok(masterPageZones);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/masterpages/{masterPageId:int}/zones/{masterPageZoneId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MasterPageZone), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId)
        {
            MasterPageZone masterPageZone = await _masterPageService.ReadMasterPageZoneAsync(tenantId, masterPageId, masterPageZoneId);
            if (masterPageZone == null)
                return NotFound();
            return Ok(masterPageZone);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/masterpages/{masterPageId:int}/zones/{masterPageZoneId:int}")]
        [ProducesResponseType(typeof(IEnumerable<MasterPageZoneElement>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchMasterPageZoneElements(long tenantId, long masterPageId, long masterPageZoneId)
        {
            IEnumerable<MasterPageZoneElement> masterPageZoneElements = await _masterPageService.SearchMasterPageZoneElementsAsync(tenantId, masterPageId, masterPageZoneId);
            return Ok(masterPageZoneElements);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/masterpages/{masterPageId:int}/zones/{masterPageZoneId:int}/elements/{masterPageZoneElementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MasterPageZoneElement), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadMasterPageZoneElement(long tenantId, long masterPageId, long masterPageZoneId, long masterPageZoneElementId)
        {
            MasterPageZoneElement masterPageZoneElement = await _masterPageService.ReadMasterPageZoneElementAsync(tenantId, masterPageId, masterPageZoneId, masterPageZoneElementId);
            if (masterPageZoneElement == null)
                return NotFound();
            return Ok(masterPageZoneElement);
        }
    }
}
