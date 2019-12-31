using System;
using System.Collections.Generic;


namespace Peanut.Infrastructure.Security.JWT
{
    public interface IJWTService
    {
        string CreateToken(Dictionary<string, object> payload);

        Tuple<int, string> ValidateToken(string token);
    }
}
