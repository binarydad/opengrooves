using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class ImageModel
    {
        public Guid ImageId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public IEnumerable<BandModel> Bands { get; set; }
        public UserModel User { get; set; }
        public GalleryModel Gallery { get; set; }
        public Guid UserId { get; set; }
        public Guid BatchId { get; set; }
        public Guid? GalleryId { get; set; }
        public DateTime Date { get; set; }
    }

    public class GalleryModel
    {
        public Guid GalleryId { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage="A gallery title is required")]
        public string Name { get; set; }

        public string UrlName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public IEnumerable<ImageModel> Images { get; set; }

        public Guid? BandId { get; set; }
        public BandModel Band { get; set; }

        public Guid UserId { get; set; }
        public UserModel User { get; set; }

        public DateTime Date { get; set; }
    }
}