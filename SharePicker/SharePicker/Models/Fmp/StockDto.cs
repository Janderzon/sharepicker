using JetBrains.Annotations;

namespace SharePicker.Models.Fmp;

[PublicAPI]
public class StockDto
{
    public required string Symbol { get; init; }
    public required string? Exchange { get; init; }
    public required string? ExchangeShortName { get; init; }
    public required string Name { get; init; }
}
