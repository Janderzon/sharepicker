namespace SharePicker.Models.Database;

public class Exchange
{
    public int ExchangeId { get; set; }

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public ICollection<Company> Companies { get; set; } = [];
}
