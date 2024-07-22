using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharePicker.Models.Options;
using System.Security.Claims;

namespace SharePicker.Services;

public class CustomAuthenticationStateProvider(
    IOptions<UserAuthenticationOptions> authenticationOptions) : AuthenticationStateProvider
{
    private ClaimsPrincipal _user = new();

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(_user));

    public bool AuthenticateUser(string email, string password)
    {
        var passwordHasher = new PasswordHasher<string>();

        if(!authenticationOptions.Value.UserHashes.TryGetValue(email, out var userHash))
            return false;

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(email, userHash, password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return false;

        var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Email, email),
            ],
            "Custom Authentication");

        var user = new ClaimsPrincipal(identity);

        _user = user;

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

        return true;
    }
}