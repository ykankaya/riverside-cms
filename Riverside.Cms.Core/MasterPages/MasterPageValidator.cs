using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Interface for classes that validate master page actions.
    /// </summary>
    public class MasterPageValidator : IMasterPageValidator
    {
        /// <summary>
        /// Validates update master page zone model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="masterPageZone">Updated master page zone details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateUpdateZone(MasterPageZone masterPageZone, string keyPrefix = null)
        {
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
            {
                if (masterPageZoneElement.ElementId == 0)
                {
                    // New element details must be specified if element ID zero 
                    if (masterPageZoneElement.Element == null || string.IsNullOrWhiteSpace(masterPageZoneElement.Element.Name) || masterPageZoneElement.Element.ElementTypeId == new Guid())
                        throw new ValidationErrorException(new ValidationError(null, MasterPageResource.ElementInvalidMessage, keyPrefix));

                    // Element name must not contain forward slash
                    if (masterPageZoneElement.Element.Name.Contains("/"))
                        throw new ValidationErrorException(new ValidationError(null, MasterPageResource.ElementNameInvalidMessage, keyPrefix));
                }
            }
        }
    }
}
