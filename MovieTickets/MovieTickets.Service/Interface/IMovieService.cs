using MovieTickets.Domain.DomainModels;

namespace MovieTickets.Service.Interface
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetDetailsForMovie(Guid? id);
        void CreateNewMovie(Movie m);
        void UpdateExistingMovie(Movie m);
        void DeleteMovie(Guid id);
        public Boolean MovieExist(Guid? id);
    }
}
