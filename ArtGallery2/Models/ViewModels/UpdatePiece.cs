using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Models.ViewModels
{
    public class UpdatePiece
    {
        public PieceDto piece {
            get; set;
        }

        public IEnumerable<FormDto> forms {
            get; set;
        }

        public IEnumerable<TechniqueDto> techniques {
            get; set;
        }

        public IEnumerable<TechniqueDto> notTechniques {
            get; set;
        }
    }
}