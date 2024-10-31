using JetBrains.Annotations;

namespace SharePicker.Models.Fmp;

[PublicAPI]
public record TradableSymbolDto
{
    public required string Symbol { get; init; }
    public required string Name { get; init; }
    public string? Exchange { get; init; }
    public string? ExchangeShortName { get; init; }
}
