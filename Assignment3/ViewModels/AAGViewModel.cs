using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Assignment3.Models;

namespace Assignment3.ViewModels
{
    public class AAGViewModel
    {
        public IEnumerable<Artist> Artists { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}