using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Service.Interface;
using System.Security.Claims;

namespace MovieTickets.Web.Controllers
{
    public class MovieTicketsController : Controller
    {
        private readonly IMovieTicketService _ticketService;
        private readonly IMovieService _movieService;

        public MovieTicketsController(IMovieTicketService movieTicketService, IMovieService movieService)
        {
            _ticketService = movieTicketService;
            _movieService = movieService;
        }

        // GET: MovieTickets
        public IActionResult Index(DateTime? selectedDate)
        {
            return View(_ticketService.GetMovieTicketsByDate(selectedDate));
        }

        // GET: MovieTickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieTicket = this._ticketService.GetDetailsForTicket(id);

            if (movieTicket == null)
            {
                return NotFound();
            }

            return View(movieTicket);
        }

        // GET: MovieTickets/Create
        public IActionResult Create()
        {
            ViewData["Movies"] = new SelectList(_movieService.GetAllMovies(), "Id", "Name");
            return View();
        }

        // POST: MovieTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieTicketDto movieTicketDto)
        {
            if (ModelState.IsValid)
            {
                var movie = _movieService.GetDetailsForMovie(movieTicketDto.MovieId);
                if (movie == null)
                {
                    return NotFound();
                }
                _ticketService.CreateNewTicket(movieTicketDto, movie);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Movies"] = new SelectList(_movieService.GetAllMovies(), "Id", "Name");
            return View(movieTicketDto);
        }

        // GET: MovieTickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null || _ticketService.GetAllTickets == null)
            {
                return NotFound();
            }

            var movieTicket = _ticketService.GetDetailsForTicket(id);
            if (movieTicket == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_movieService.GetAllMovies(), "Id", "Name");
            return View(movieTicket);
        }

        // POST: MovieTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, EditMovieTicketDto editMovieTicketDto)
        {
            if (id != editMovieTicketDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var movie = _movieService.GetDetailsForMovie(editMovieTicketDto.MovieId);
                    if (movie == null)
                    {
                        return NotFound();
                    }
                    _ticketService.UpdateExistingTicket(editMovieTicketDto, movie);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!_ticketService.TicketExist(editMovieTicketDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["MovieId"] = new SelectList(_movieService.GetAllMovies(), "Id", "Name");
            return View(editMovieTicketDto);
        }

        public IActionResult AddTicketToCart(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = _ticketService.AddToShoppingCart(id, userId);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        // GET: MovieTickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null || _ticketService.GetAllTickets == null)
            {
                return NotFound();
            }

            var movieTicket = _ticketService.GetDetailsForTicket(id);
            if (movieTicket == null)
            {
                return NotFound();
            }

            return View(movieTicket);
        }

        // POST: MovieTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (_ticketService.GetAllTickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MovieTickets'  is null.");
            }
            var movieTicket = _ticketService.GetDetailsForTicket(id);
            if (movieTicket != null)
            {
                _ticketService.DeleteTicket(movieTicket.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MovieTicketExists(Guid id)
        {
          return (_movieService.GetAllMovies()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
