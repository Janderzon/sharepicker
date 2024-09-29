namespace SharePicker.Models;

public class YearlyStatements<T> : Dictionary<int, T> where T : Statement;
