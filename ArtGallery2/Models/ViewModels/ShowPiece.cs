using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Models.ViewModels
{
    public class ShowPiece
    {
        public PieceDto piece {
            get; set;
        }
        public FormDto form {
            get; set;
        }

        public IEnumerable<TechniqueDto> techniques;
    }
}