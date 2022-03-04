using System.Linq;
using System.Text;

namespace IdentityServer.Services
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        private readonly IConfiguration _configuration;

        public CodeGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateVerificationCode()
        {
            var characters = _configuration["GeneratorService:Characters"];
            var numerals = _configuration["GeneratorService:Numerals"];

            var sequence = characters.Concat(numerals).ToList();
            var code = new StringBuilder();
            var rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                code.Append(sequence[rnd.Next(sequence.Count - 1)]);
            }

            return Task.FromResult(code.ToString());
        }
    }
}
