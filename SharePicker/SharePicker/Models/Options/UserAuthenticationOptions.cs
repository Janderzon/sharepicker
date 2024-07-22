namespace SharePicker.Models.Options;

public class UserAuthenticationOptions
{
    public const string Name = "UserAuthenticationOptions";

    public IReadOnlyDictionary<string, string> UserHashes { get; init; } = new Dictionary<string, string>();
}
