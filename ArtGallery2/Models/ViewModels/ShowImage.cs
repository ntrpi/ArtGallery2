using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Models.ViewModels
{
    public class ShowImage
    {
        public ImageDto image {
            get; set;
        }

        public ShowImages showImages {
            get; set;
        }
    }
}