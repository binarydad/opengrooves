using OpenGrooves.Web.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Areas.Edit.Models
{
    public class EditImagesModel
    {
        [Required]
        [DisplayName("Gallery name")]
        public string NewGalleryName { get; set; }

        public IEnumerable<ImageModel> Images { get; set; }
        public IEnumerable<GalleryModel> Galleries { get; set; }
    }
}