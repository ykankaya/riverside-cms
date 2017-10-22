using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Riverside.Utilities.Drawing
{
    public enum ResizeMode
    {
        // Resize image do not maintain aspect ratio
        [Display(ResourceType = typeof(DrawingResource), Name = "ResizeModeSimpleLabel")]
        Simple,

        // Resize maintain aspect ratio
        [Display(ResourceType = typeof(DrawingResource), Name = "ResizeModeMaintainAspectLabel")]
        MaintainAspect,

        // Resize image, maintain aspect ratio and crop to size
        [Display(ResourceType = typeof(DrawingResource), Name = "ResizeModeCropLabel")]
        Crop
    }
}
