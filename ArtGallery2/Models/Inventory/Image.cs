using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery2.Models.Inventory
{
    public class Image
    {
        [Key]
        public int imageId {
            get; set;
        }

        public string imagePath {
            get; set;
        }

        // An image is of one piece.
        [ForeignKey( "Piece" )]
        public int pieceId {
            get; set;
        }
        public virtual Piece Piece {
            get; set;
        }
    }

    public class ImageDto
    {
        public int imageId {
            get; set;
        }

        public string imagePath {
            get; set;
        }

        public int pieceId {
            get; set;
        }
    }
}