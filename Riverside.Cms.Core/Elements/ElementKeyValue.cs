using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    public struct ElementKeyValue : IEquatable<ElementKeyValue>
    {
        /// <summary>
        /// Returns hash code for this object. For more information on this implementation see:
        /// Jon Skeet's answer on http://stackoverflow.com/questions/263400
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ElementId.GetHashCode();
                hash = hash * 23 + ElementTypeId.GetHashCode();
                return hash;
            }
        }

        public bool Equals(ElementKeyValue elementKeyValue)
        {
            if (elementKeyValue.GetType() != this.GetType())
                return false;
            else
                return ElementId == elementKeyValue.ElementId && ElementTypeId == elementKeyValue.ElementTypeId;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ElementKeyValue))
                return false;
            return Equals((ElementKeyValue)obj);
        }

        public static bool operator!=(ElementKeyValue elementKeyValue1, ElementKeyValue elementKeyValue2)
        {
            return !(elementKeyValue1 == elementKeyValue2);
        }

        public static bool operator==(ElementKeyValue elementKeyValue1, ElementKeyValue elementKeyValue2)
        {
            // If both are null, or both are same instance, return true
            if (Object.ReferenceEquals(elementKeyValue1, elementKeyValue2))
                return true;

            // If one is null, but not both, return false
            if (((object)elementKeyValue1 == null) || ((object)elementKeyValue2 == null))
                return false;

            // Compare member variables
            return elementKeyValue1.Equals(elementKeyValue2);
        }

        public long ElementId { get; set; }
        public Guid ElementTypeId { get; set; }
    }
}
