using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.Enumerations;
using MovieTickets.Repository;
using MovieTickets.Service.Interface;

namespace MovieTickets.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovieService _movieService;

        public MoviesController(ApplicationDbContext context, IMovieService movieService)
        {
            _context = context;
            _movieService = movieService;
        }

        // GET: Movies
        public IActionResult Index()
        {
            return View(_movieService.GetAllMovies());
        }

        // GET: Movies/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = _movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
            ViewData["Genres"] = new SelectList(genres);
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Genre,Duration,Poster")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = Guid.NewGuid();
                _movieService.CreateNewMovie(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
            ViewData["Genres"] = new SelectList(genres);
            var movie = _movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Genre,Duration,Poster")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _movieService.UpdateExistingMovie(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_movieService.MovieExist(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _movieService.GetDetailsForMovie(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _movieService.DeleteMovie(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
