using ProjectManagement.Data.Entity;

namespace ProjectManagement.Model.Services.Interface;

public interface IMovieServices
{
     List<Movie> GetAll();
     Task  AddMovieAsync (Movie movie);


}