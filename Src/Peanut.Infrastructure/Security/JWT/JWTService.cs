using System;
using System.Collections.Generic;

using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Peanut.Infrastructure.Security.JWT
{
    /// <summary>
    /// JWT服务类
    /// 
    /// 目的：
    ///     1.为API提供JWT的创建和验证
    ///     2.为API提供负载创建器
    /// 
    /// 使用规范：
    ///    var load = new Dictionary<typeparamref name="string", typeparamref name="object"/>
    ///    {
    ///    { "iss","combacrm" },
    ///    { "sub","all" },
    ///    { "aud","SAP2" },
    ///    { "exp","2019-12-02 15:00:00" }
    ///    };
    /// </summary>
    public class JWTService : IJWTService
    {
        static readonly IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        static readonly IJsonSerializer serializer = new JsonNetSerializer();
        static readonly IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        static readonly IDateTimeProvider provider = new UtcDateTimeProvider();

        public string CreateToken(Dictionary<string, object> payload)
        {
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, "combaCRM");
        }

        public Tuple<int, string> ValidateToken(string token)
        {
            try
            {
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                decoder.Decode(token, "combaCRM", verify: true);

                return new Tuple<int, string>(1, "验证成功");
            }
            catch (TokenExpiredException)
            {
                return new Tuple<int, string>(0, "认证过期");
            }
            catch (SignatureVerificationException)
            {
                return new Tuple<int, string>(0, "签名错误");
            }
        }
    }
}