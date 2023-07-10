using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using MovieTickets.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Service.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _repository;

        public MovieService(IRepository<Movie> repository)
        {
            _repository = repository;
        }

        public void CreateNewMovie(Movie m)
        {
            _repository.Insert(m);
        }

        public void DeleteMovie(Guid id)
        {
            var movieToDelete = this.GetDetailsForMovie(id);
            _repository.Delete(movieToDelete);
        }

        public List<Movie> GetAllMovies()
        {
            return _repository.GetAll().ToList();
        }

        public Movie GetDetailsForMovie(Guid? id)
        {
            return _repository.Get(id);
        }

        public bool MovieExist(Guid? id)
        {
            var movieExists = this.GetDetailsForMovie(id);

            return movieExists != null;
        }

        public void UpdateExistingMovie(Movie m)
        {
            _repository.Update(m);
        }
    }
}
