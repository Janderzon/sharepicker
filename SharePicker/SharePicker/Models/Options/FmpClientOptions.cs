﻿namespace SharePicker.Models.Options;

public class FmpClientOptions
{
    public const string Name = "FmpClientOptions";

    public string ApiKey { get; init; } = string.Empty;

    public TimeSpan CacheExpiry { get; init; } = TimeSpan.FromHours(1);
}
