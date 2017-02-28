using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Assignment3.Models
{

    public class Album
    {
        [Key]
        public int AlbumID { get; set; }

        [ForeignKey("Artist")]
        public int ArtistID { get; set; }
        public virtual Artist Artist { get; set; }

        [Required]
        [Display(Name = "Album Name")]
        public string AlbumName { get; set; }

        [Display(Name = "Year Released")]
        [RegularExpression(@"([0-9][0-9][0-9][0-9])", ErrorMessage = "You must enter a 4 digit year")]
        public int YearRelease { get; set; }
        
        public virtual ICollection<Genre> Genres { get; set; }
    }
}