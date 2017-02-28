using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment3.Models
{
    public class Artist
    {
        [Key]
        public int ArtistID { get; set; }

        [Required]
        [Display(Name = "Artist")]
        public string ArtistName { get; set; }

        [Display(Name = "Hometown of Artist")]
        public string ArtistHometown { get; set; }

        public virtual List<Album> Albums { get; set; }
    }
}