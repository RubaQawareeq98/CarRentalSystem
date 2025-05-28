using System.Net.Http.Headers;
using CarRentalSystem.Db.Enums;

namespace CarRentalSystem.Test.Shared;

public abstract class TestAuthenticationHeader
{
    public static void SetTestAuthHeader(HttpClient client, Guid userId, UserRole role)
    {
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("TestAuthScheme", $"{userId}|{role.ToString()}");
    }
}