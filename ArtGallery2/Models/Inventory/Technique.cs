using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery2.Models.Inventory
{
    public class Technique
    {
        [Key]
        public int techniqueId {
            get; set;
        }

        public string techniqueName {
            get; set;
        }
    }

    public class TechniqueDto
    {
        public int techniqueId {
            get; set;
        }

        public string techniqueName {
            get; set;
        }
    }
}