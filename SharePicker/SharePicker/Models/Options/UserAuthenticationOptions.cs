namespace SharePicker.Models.Options;

public class UserAuthenticationOptions
{
    public const string Name = "UserAuthenticationOptions";

    public HashSet<User> Users { get; init; } = [];

    public string Salt { get; init; } = string.Empty;

    public record User
    {
        public required string Email { get; set; } = string.Empty;

        public required string PasswordHash { get; set; } = string.Empty;
    }
}
