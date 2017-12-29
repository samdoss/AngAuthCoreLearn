using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WebBoardAuth.Logic
{
    public class TokenProviderOptions
    {
        ///  The Issuer (iss) claim for generated tokens.
        public Func<string, Task<string>> Issuer { get; set; }

        /// The Audience (aud) claim for the generated tokens.
        public Func<string, Task<string>> Audience { get; set; }

        /// The expiration time for the generated tokens. (Default is 5 minutes)
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(30);

        /// The signing key to use when generating tokens.
        public Func<byte[], Task<SigningCredentials>> SigningCredentials { get; set; }

        /// Resolves a user identity given a username and password.
        public Func<string, bool, Task<ClaimsIdentity>> IdentityResolver { get; set; }

        /// Generates a random value (nonce) for each generated token.
        /// The default nonce is a random GUID.
        public Func<Task<string>> NonceGenerator { get; set; }
            = new Func<Task<string>>(() => Task.FromResult(Guid.NewGuid().ToString()));
    }
}
