namespace SharePicker.Models.Database;

public class Exchange
{
    public int ExchangeId { get; set; }

    public required string Name { get; set; }

    public required string Symbol { get; set; }

    public ICollection<Company> Companies { get; set; } = [];
}
