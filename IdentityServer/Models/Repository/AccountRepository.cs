using IdentityServer.Models.Data;
using IdentityServer.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Models.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        
        public AccountRepository(ApplicationDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<Result<Account>> CreateAsync(Account account)
        {
            try
            {
                if (!IsUniqueUser(account.Email!))
                    return new Result<Account>(false, new List<string> { "An account with that email already exists!"});

                account.Password = _passwordService.HashPassword(account.Password!);
                
                await _context.Accounts!.AddAsync(account);
                await _context.SaveChangesAsync();

                return new Result<Account>(account, new List<string> { "Account created successfully!"});
            }
            catch (Exception ex)
            {
                return new Result<Account>(false,
                    new List<string> { ex.ToString() });
            }
        }

        public Task<Result<bool>> DeleteAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Account>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Account>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Account>> UpdateAsync(Account account)
        {
            throw new NotImplementedException();
        }


        public async Task<Result<Account>> LoginAsync(LoginRequest login)
        {
            var account = await _context.Accounts!.Where(x => x.Email == login.Email).FirstOrDefaultAsync();

            if (account == null || _passwordService.VerifyHash(login.Password!, account!.Password!) == false)
                return new Result<Account>(false, new List<string>() { "Username or password is incorrect!"});

            return new Result<Account>(account);
        }
        public bool IsUniqueUser(string email)
        {
            var user = _context.Accounts!.SingleOrDefault(x => x.Email == email);

            if (user == null) return true;
            return false;
        }
    }
}