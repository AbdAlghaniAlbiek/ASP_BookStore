using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStore.API.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUp signUp);

        Task<string> LoginAsync(SignIn signIn);
    }
}