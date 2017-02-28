using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Assignment3.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public string GenreName { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}