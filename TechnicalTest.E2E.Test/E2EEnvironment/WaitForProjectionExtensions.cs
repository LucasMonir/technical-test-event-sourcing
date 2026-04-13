using System.Net.Http.Json;

namespace TechnicalTest.E2E.Test.E2EEnvironment
{
    public static class WaitForProjectionExtensions
    {
        public static async Task<T?> WaitForProjectionAsync<T>(
            this HttpClient client,
            string url,
            Func<T, bool> predicate,
            int timeoutSeconds = 10,
            int pollIntervalMs = 200)
        {
            var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);

            while (DateTime.UtcNow < deadline)
            {
                try
                {
                    var result = await client.GetFromJsonAsync<T>(url);
                    if (result is not null && predicate(result))
                        return result;
                }
                catch (HttpRequestException)
                {

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                await Task.Delay(pollIntervalMs);
            }

            throw new TimeoutException($"Projection at {url} did not satisfy condition within {timeoutSeconds}s.");
        }
    }
}
