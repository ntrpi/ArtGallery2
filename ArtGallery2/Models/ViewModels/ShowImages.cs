using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Models.ViewModels
{
    public class ShowImages
    {
        public PieceDto piece {
            get; set;
        }

        public IEnumerable<ImageDto> images {
            get; set;
        }
    }
}