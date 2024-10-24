﻿namespace SharePicker.Models.Fmp;

public record TradableSymbolDto
{
    public required string Symbol { get; init; }
    public required string Name { get; init; }
    public string? Exchange { get; init; }
    public string? ExchangeShortName { get; init; }
}
