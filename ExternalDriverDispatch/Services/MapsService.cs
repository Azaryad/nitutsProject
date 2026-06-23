using System;
using System.Net.Http;
using System.Text.Json;

namespace ExternalDriverDispatch
{
    /// <summary>Drive estimate returned by the Maps service: minutes + kilometres.</summary>
    public record DriveInfo(int DurationMinutes, double DistanceKm);

    /// <summary>
    /// Service 1 — "how far / how long." Given pickup + dropoff + time, returns a drive estimate.
    /// Maps is upstream of everything: it runs once when a trip is enriched and the numbers it
    /// writes onto the Trip drive the long-distance driver filter and feed the AI ranking prompt.
    /// </summary>
    public interface IDriveInfoProvider
    {
        DriveInfo GetDriveInfo(string origin, string destination, DateTime pickupTime);
    }

    /// <summary>
    /// Offline fallback — always returns 60 min / 0 km. Consequence: every trip is treated as
    /// 60 minutes and not long-distance. The system still dispatches; it just loses
    /// traffic/distance awareness. An acceptable downgrade, not a failure.
    /// </summary>
    public class StaticDriveInfoProvider : IDriveInfoProvider
    {
        public DriveInfo GetDriveInfo(string origin, string destination, DateTime pickupTime)
        {
            return new DriveInfo(60, 0.0);
        }
    }

    /// <summary>
    /// Real implementation — Google Distance Matrix API. Any exception / non-OK response falls
    /// back to (60, 0): a missing key downgrades the feature, it never throws to the caller.
    /// </summary>
    public class GoogleMapsDriveInfoProvider : IDriveInfoProvider
    {
        private static readonly HttpClient http = new HttpClient();
        private readonly string apiKey;

        public GoogleMapsDriveInfoProvider(string apiKey) { this.apiKey = apiKey; }

        public DriveInfo GetDriveInfo(string origin, string destination, DateTime pickupTime)
        {
            try
            {
                long departure = ((DateTimeOffset)pickupTime.ToUniversalTime()).ToUnixTimeSeconds();
                if (departure < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    departure = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // API rejects past departure_time
                string url =
                    "https://maps.googleapis.com/maps/api/distancematrix/json" +
                    "?origins=" + Uri.EscapeDataString(origin) +
                    "&destinations=" + Uri.EscapeDataString(destination) +
                    "&departure_time=" + departure +
                    "&key=" + apiKey;

                using HttpResponseMessage resp = http.Send(new HttpRequestMessage(HttpMethod.Get, url));
                if (!resp.IsSuccessStatusCode) return new DriveInfo(60, 0.0);

                using var stream = resp.Content.ReadAsStream();
                using JsonDocument doc = JsonDocument.Parse(stream);
                JsonElement elem = doc.RootElement
                    .GetProperty("rows")[0]
                    .GetProperty("elements")[0];

                if (elem.GetProperty("status").GetString() != "OK") return new DriveInfo(60, 0.0);

                int seconds = elem.GetProperty("duration").GetProperty("value").GetInt32();
                int meters = elem.GetProperty("distance").GetProperty("value").GetInt32();
                return new DriveInfo((int)Math.Round(seconds / 60.0), Math.Round(meters / 1000.0, 1));
            }
            catch
            {
                return new DriveInfo(60, 0.0); // graceful degradation
            }
        }
    }
}
