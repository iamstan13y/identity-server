using IdentityServer.Models.Data;
using IdentityServer.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Models.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ICodeGeneratorService _codeGeneratorService;

        public AccountRepository(ApplicationDbContext context, IPasswordService passwordService, IJwtService jwtService, ICodeGeneratorService codeGeneratorService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _codeGeneratorService = codeGeneratorService;
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

        public async Task<Result<Account>> GetByIdAsync(int id)
        {
            var account = await _context.Accounts!.SingleOrDefaultAsync(x => x.Id == id);
            if (account == null)
                return new Result<Account>(false, new List<string>() { "User not found"});

            return new Result<Account>(account);
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

            account.Token = await _jwtService.GenerateToken(account);
            account.Password = "*************";

            return new Result<Account>(account);
        }
        private bool IsUniqueUser(string email)
        {
            var user = _context.Accounts!.SingleOrDefault(x => x.Email == email);

            if (user == null) return true;
            return false;
        }

        public async Task<Result<Account>> ChangePasswordAsync(ChangePasswordRequest changePassword)
        {
            var account = await GetByIdAsync(changePassword.UserId);
            if (!account.Success) return account;

            if (_passwordService.VerifyHash(changePassword.OldPassword!, account.Data!.Password!) == false)
                return new Result<Account>(false, new List<string>() { "Old password mismatch"});

            account.Data.Password = _passwordService.HashPassword(changePassword.NewPassword!);

            _context.Accounts!.Update(account.Data);
            await _context.SaveChangesAsync();

            return new Result<Account>(account.Data);
        }

        public async Task<Result<string>> GetResetPasswordCodeAsync(string email)
        {
            var account = await _context.Accounts!.SingleOrDefaultAsync(y => y.Email == email);
            if (account == null) return new Result<string>(false, new List<string> { "User account does not exist." });

            var verificationCode = await _codeGeneratorService.GenerateVerificationCode();

            //save code to db
            //await _context.GenerateCodes
            await _context.SaveChangesAsync();
            //send code via email
            return null;
        }
    }
}