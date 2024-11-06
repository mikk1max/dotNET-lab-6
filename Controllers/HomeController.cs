using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Controllers
{
    public class HomeController : Controller
    {
        private readonly MoviesDbContext _context;

        public HomeController(MoviesDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            await _context.Movies.Include(x => x.Genre).ToListAsync();
            return _context.Movies != null
                ? View(await _context.Movies.ToListAsync())
                : Problem("Entity set 'MoviesDbContext.Movies'  is null.");
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            var m = new MovieDto
            {
                AllGenres = _context.Genres.Select(x => x.Name).ToList()
            };
            return View(m);
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,Rating,TrailerLink,Genre")] MovieDto movie
        )
        {
            if (ModelState.IsValid)
            {
                var genre = _context.Genres.FirstOrDefault(x => x.Name == movie.Genre);
                if (genre == null)
                {
                    genre = new Genre { Id = 0, Name = movie.Genre };
                }
                Movie m = new Movie
                {
                    Id = 0,
                    Title = movie.Title,
                    Description = movie.Description,
                    Rating = movie.Rating,
                    TrailerLink = movie.TrailerLink,
                    Genre = genre
                };
                _context.Add(m);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(x => x.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                TrailerLink = movie.TrailerLink,
                Genre = movie.Genre?.Name,
                AllGenres = _context.Genres.Select(x => x.Name).ToList()
            };

            return View(movieDto);
        }


        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,Rating,Genre,TrailerLink")] MovieDto movie
        )
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await _context.Movies.Include(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);
                    if (existingMovie == null)
                    {
                        return NotFound();
                    }

                    // Update properties from the DTO to the entity
                    existingMovie.Title = movie.Title;
                    existingMovie.Description = movie.Description;
                    existingMovie.Rating = movie.Rating;
                    existingMovie.TrailerLink = movie.TrailerLink;

                    // Update the genre if needed
                    var genre = _context.Genres.FirstOrDefault(g => g.Name == movie.Genre);
                    if (genre == null)
                    {
                        // Optionally, create a new genre if it doesn't exist (based on your requirements)
                        genre = new Genre { Name = movie.Genre };
                        _context.Genres.Add(genre);
                    }
                    existingMovie.Genre = genre;

                    _context.Update(existingMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // Re-populate AllGenres for the view if the model state is invalid
            movie.AllGenres = _context.Genres.Select(x => x.Name).ToList();
            return View(movie);
        }


        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'MoviesDbContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
