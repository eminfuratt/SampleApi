using SampleApi.Models;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string name, string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
