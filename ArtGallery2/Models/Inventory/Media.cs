using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery2.Models.Inventory
{
    public class Media
    {
        [Key]
        public int mediaId {
            get; set;
        }

        public string mediaName {
            get; set;
        }
    }

    public class MediaDto
    {
        public int mediaId {
            get; set;
        }

        public string mediaName {
            get; set;
        }
    }
}