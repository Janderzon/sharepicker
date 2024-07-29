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

        var user = authenticationOptions.Value.Users.SingleOrDefault(x => x.Email == email);

        if(user == null)
            return false;

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(email, user.Password, password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return false;

        var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Email, email),
            ],
            "Custom Authentication");

        _user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));

        return true;
    }

    public void Logout()
    {
        _user = new ClaimsPrincipal();

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
    }
}