using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment3.Models;
using Assignment3.ViewModels;
using System.Data.Entity.Infrastructure;

namespace Assignment3.Controllers
{
    public class AlbumsController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Albums
        public ActionResult Index()
        {
            //new
            var allGenres = db.Genres;
            ViewBag.Genres = new List<string>();
            foreach(var genre in allGenres)
            {
                ViewBag.Genres.Add(genre.GenreName);
            }
            //end new
            var albums = db.Albums.Include(a => a.Artist).Include(g => g.Genres);
            return View(albums.ToList());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            var album = new Album();
            album.Genres = new List<Genre>();
            PopulateAssignedGenreData(album);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumID,ArtistID,AlbumName,YearRelease")] Album album, string[] selectedGenres)
        {
            //add selected genres
            if(selectedGenres != null)
            {
                album.Genres = new List<Genre>();
                foreach(var genre in selectedGenres)
                {
                    var genreToAdd = db.Genres.Find(int.Parse(genre));
                    album.Genres.Add(genreToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", album.ArtistID);
            PopulateAssignedGenreData(album);
            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //make sure album inlcudes necesary info (genres)
            Album album = db.Albums
                .Include(a => a.Genres)
                .Where(i => i.AlbumID == id)
                .Single();
            if (album == null)
            {
                return HttpNotFound();
            }
            //put list of AlbumGenreVM into ViewBag
            PopulateAssignedGenreData(album);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", album.ArtistID);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedGenres)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //make sure album inlcudes necesary info (genres)
            Album album = db.Albums
                .Include(a => a.Genres)
                .Where(i => i.AlbumID == id)
                .Single();
            if (TryUpdateModel(album, "", new string[] { "AlbumID", "ArtistID", "AlbumName", "YearRelease" }))
            {
                try
                {
                    UpdateAlbumGenres(selectedGenres, album);
                    db.Entry(album).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", album.ArtistID);
            PopulateAssignedGenreData(album);
            return View(album);
        }

        /* old method
        public ActionResult Edit([Bind(Include = "AlbumID,ArtistID,AlbumName,YearRelease")] Album album, string[] selectedGenres)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", album.ArtistID);
            return View(album);
        }
        */

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //gets genres assigned to an album (argument) using a list of AlbumGenreVM objects
        private void PopulateAssignedGenreData(Album album)
        {
            //get all genres in db
            var allGenres = db.Genres;
            //get id's of genres assigned to album
            var albumGenres = new HashSet<int>(album.Genres.Select(b => b.GenreID));
            //list of AlbumGenreVM that will belong to album
            var viewModel = new List<AlbumGenreVM>();
            foreach (var genre in allGenres)
            {
                //add AlbumGenreVM to list and mark Assigned if genre id found in albumGenres
                viewModel.Add(new AlbumGenreVM
                {
                    GenreID = genre.GenreID,
                    GenreName = genre.GenreName,
                    Assigned = albumGenres.Contains(genre.GenreID)
                });
            }
            ViewBag.Genres = viewModel;
        }

        private void UpdateAlbumGenres(string[] selectedGenres, Album album)
        {
            if(selectedGenres == null)
            {
                album.Genres = new List<Genre>();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var albumGenres = new HashSet<int>(album.Genres.Select(g => g.GenreID));

            foreach(var genre in db.Genres)
            {
                if(selectedGenresHS.Contains(genre.GenreID.ToString()))
                {
                    if(!albumGenres.Contains(genre.GenreID))
                    {
                        album.Genres.Add(genre);
                    }
                }
                else
                {
                    if(albumGenres.Contains(genre.GenreID))
                    {
                        album.Genres.Remove(genre);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
