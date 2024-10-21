namespace SharePicker.Models.Options;

public class DatabaseOptions
{
    public const string Name = "DatabaseOptions";

    public string ConnectionString { get; init; } = string.Empty;
}
