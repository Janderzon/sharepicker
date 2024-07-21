using Microsoft.AspNetCore.Components.Authorization;
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

    public bool AuthenticateUser(string Email, string Password)
    {
        if (!authenticationOptions.Value.Users.Contains(new UserAuthenticationOptions.User
        { 
            Email = Email,
            PasswordHash = Password
        }))
            return false;

        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Email, Email),
        ], "Custom Authentication");

        var user = new ClaimsPrincipal(identity);

        _user = user;

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));

        return true;
    }
}