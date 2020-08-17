using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using NCB.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.Core.Helpers
{
    public class JwtHelper
    {
        public static string GenerateToken(JwtPayload payload, string secret)
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(3).ToUnixTimeSeconds())
                .AddClaim("JwtPayload", payload)
                .Encode();

            return token;
        }

        public static JwtPayload ValidateToken(string token, string secret)
        {
            try
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode<JwtJsonDecode>(token);

                return json.JwtPayload;
            }
            //catch (TokenExpiredException e)
            //{
            //    throw e;
            //}
            //catch (SignatureVerificationException e)
            //{
            //    throw e;
            //}
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class JwtJsonDecode
    {
        public long Exp { get; set; }
        public JwtPayload JwtPayload { get; set; }
    }
}
