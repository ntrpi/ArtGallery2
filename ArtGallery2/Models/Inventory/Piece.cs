using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery2.Models.Inventory
{
    public class Piece
    {
        [Key]
        public int pieceId {
            get; set;
        }

        [Required]
        public string pieceName {
            get; set;
        }

        public string pieceDescription {
            get; set;
        }

        public decimal length {
            get; set;
        }

        public decimal width {
            get; set;
        }

        public decimal height {
            get; set;
        }

        public decimal piecePrice {
            get; set;
        }

        // A piece has one form.
        [ForeignKey( "Form" )]
        public int formId {
            get; set;
        }
        public virtual Form Form {
            get; set;
        }

        // A piece has one Media.
        //        [ForeignKey( "Media" )]
        //public int mediaId {
        //    get; set;
        //}
        //public virtual Media Media {
        //    get; set;
        //}

        // A piece has one Status.
        //        [ForeignKey( "Status" )]
        //public int statusId {
        //    get; set;
        //}
        //public virtual Status Status {
        //    get; set;
        //}

        ICollection<Image> images {
            get; set;
        }

        [DisplayName( "Techniques" )]
        ICollection<Technique> techniques {
            get; set;
        }
    }

    public class PieceDto
    {
        public int pieceId {
            get; set;
        }

        [DisplayName( "Name" )]
        public string pieceName {
            get; set;
        }

        [DisplayName( "Description" )]
        public string pieceDescription {
            get; set;
        }

        [DisplayName( "Length (cm)" )]
        public decimal length {
            get; set;
        }

        [DisplayName( "Width (cm)" )]
        public decimal width {
            get; set;
        }

        [DisplayName( "Height (cm)" )]
        public decimal height {
            get; set;
        }

        [DisplayName( "Price" )]
        [DataType( DataType.Currency )]
        public decimal piecePrice {
            get; set;
        }

        [DisplayName( "Form" )]
        public int formId {
            get; set;
        }

        //public int statusId {
        //    get; set;
        //}

        //public int mediaId {
        //    get; set;
        //}

        [DisplayName( "Techniques" )]
        public int techniques {
            get; set;
        }
    }
}