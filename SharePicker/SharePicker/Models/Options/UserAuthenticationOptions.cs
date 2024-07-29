namespace SharePicker.Models.Options;

public class UserAuthenticationOptions
{
    public const string Name = "UserAuthenticationOptions";

    public List<User> Users { get; init; } = [];
}

public class User
{
    public string Email { get; init; } = string.Empty;
    
    public string Password { get; init; } = string.Empty;
}
