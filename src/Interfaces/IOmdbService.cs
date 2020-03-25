using System.Threading.Tasks;

namespace MovieRating.Interfaces
{
    public interface IOmdbService<T>
    {
        Task<T> GetRating(string title);
    }
}
