using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3.ViewModels
{   
    //each album will have an AlbumGenreVM for each genre, marked as assigned if assigned
    public class AlbumGenreVM
    {
        public int GenreID { get; set; }

        public string GenreName { get; set; }

        public bool Assigned { get; set; }

    }
}