namespace SharePicker.Components;

public class FmpClient(HttpClient httpClient) : IDisposable
***REMOVED***
    public void Dispose() => httpClient.Dispose();
***REMOVED***
