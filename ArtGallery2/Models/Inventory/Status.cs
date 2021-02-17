using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery2.Models.Inventory
{
    public class Status
    {
        [Key]
        public int statusId {
            get; set;
        }

        public string statusName {
            get; set;
        }
    }

    public class StatusDto
    {
        public int statusId {
            get; set;
        }

        public string statusName {
            get; set;
        }
    }
}