using SjxLogistics.Models;
using SjxLogistics.Models.DatabaseModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace SjxLogistics.Components
{
    public class AccessToken
    {
        private readonly AccessConfig _accessConfig;
        private readonly TokkenGeneratorClass _tokenGen;
        public AccessToken(AccessConfig accessConfig, TokkenGeneratorClass tokkenGeneration)
        {
            _accessConfig = accessConfig;
            _tokenGen = tokkenGeneration;
        }
        public string GenerateToken(Users users)
        {
            List<Claim> claims = new() {
                new Claim(ClaimTypes.Name, users.Id.ToString()),
                new Claim(ClaimTypes.Role, users.Role),
                new Claim(ClaimTypes.Email, users.Email),
            };
            return _tokenGen.TokenGenerator(
                _accessConfig.AccessKey,
                _accessConfig.Audiance,
                _accessConfig.Issuer,
                _accessConfig.Expiration,
                claims);
        }
        public string GenerateResetPasswordToken(Users users)
        {
            List<Claim> claims = new() {
                new Claim(ClaimTypes.Email, users.Email),
            };
            return _tokenGen.TokenGenerator(
                _accessConfig.AccessKey,
                _accessConfig.Audiance,
                _accessConfig.Issuer,
                _accessConfig.RestPassword,
                claims);
        }
        public string GeneratePaymentToken()
        {
            return _tokenGen.TokenGenerator(
                _accessConfig.AccessKey,
                _accessConfig.Audiance,
                _accessConfig.Issuer,
                _accessConfig.PaymentExpiration
                );
        }
        /*public string RefreshTokenGen()
        {
            return _tokenGen.TokenGenerator(
                _accessConfig.RefreshAccessKey,
                _accessConfig.Audiance,
                _accessConfig.Issuer,
                _accessConfig.RefreshExpiration);
        }*/
    }
}
