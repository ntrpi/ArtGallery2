using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery2.Models.Inventory
{
    public class PieceTechnique
    {
        [Key]
        public int pieceTechniqueId {
            get; set;
        }

        [ForeignKey( "Piece" )]
        public int pieceId {
            get; set;
        }
        public virtual Piece Piece {
            get; set;
        }

        [ForeignKey( "Technique" )]
        public int techniqueId {
            get; set;
        }
        public virtual Technique Technique {
            get; set;
        }
    }
    public class PieceTechniqueDto
    {
        public int pieceTechniqueId {
            get; set;
        }

        public int pieceId {
            get; set;
        }

        public int techniqueId {
            get; set;
        }
    }
}