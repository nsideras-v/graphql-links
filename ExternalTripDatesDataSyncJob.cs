using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Optimizely.ContentGraph.Cms.Configuration;
using Optimizely.Graph.Source.Sdk;

namespace graphql_link
{
    [ScheduledPlugIn(DisplayName = "External Trip Dates Data Sync",
        GUID = "fcfe4f2d-e38e-4bd3-a0c1-d348aa925fce",
        IntervalType = ScheduledIntervalType.Days,
        InitialTime = "2025-01-01T00:00:00",
        IntervalLength = 1,
        Restartable = true)]
    public class ExternalTripDatesDataSyncJob : ScheduledJobBase
    {
        protected Injected<IConfiguration> AppSettingsConfig { get; set; }

        public ExternalTripDatesDataSyncJob()
        {
            IsStoppable = false;
        }
        public override string Execute()
        {
            SyncData();

            return "Completed.";
        }

        private async void SyncData()
        {
            var contentGraphSettings = AppSettingsConfig.Service.GetSection("Optimizely:ContentGraph").Get<QueryOptions>();

            var client = GraphSourceClient.Create(
              new Uri("https://cg.optimizely.com"),
              "src1",
              contentGraphSettings.AppKey,
              contentGraphSettings.Secret
            );

            await client.DeleteContentAsync();

            client.AddLanguage("en");

            client.ConfigureContentType<ExternalTripDates>()
                .Field(x => x.ExternalTripCode, Optimizely.Graph.Source.Sdk.SourceConfiguration.IndexingType.Searchable)
                .Field(x => x.ExternalDepartureDate, Optimizely.Graph.Source.Sdk.SourceConfiguration.IndexingType.Searchable)
                .Field(x => x.ExternalReturnDate, Optimizely.Graph.Source.Sdk.SourceConfiguration.IndexingType.Searchable)
                .Field(x => x.ExternalSpaceAvailable, Optimizely.Graph.Source.Sdk.SourceConfiguration.IndexingType.Searchable)
                .Field(x => x.ExternalSingleAvailability, Optimizely.Graph.Source.Sdk.SourceConfiguration.IndexingType.Searchable);

            await client.SaveTypesAsync();

            var tripDates = new ExternalTripDates[]
            {
                new() { ExternalTripCode = "LVC", ExternalDepartureDate = new DateTime(2025, 6, 1),  ExternalReturnDate = new DateTime(2025, 6, 8),  ExternalSpaceAvailable = "0",    ExternalSingleAvailability = "0" },
                new() { ExternalTripCode = "LVC", ExternalDepartureDate = new DateTime(2025, 6, 3),  ExternalReturnDate = new DateTime(2025, 6, 10), ExternalSpaceAvailable = "< 10", ExternalSingleAvailability = "0" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 5),  ExternalReturnDate = new DateTime(2025, 6, 12), ExternalSpaceAvailable = "< 10", ExternalSingleAvailability = "0" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 7),  ExternalReturnDate = new DateTime(2025, 6, 14), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "0" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 9),  ExternalReturnDate = new DateTime(2025, 6, 16), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "1" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 11), ExternalReturnDate = new DateTime(2025, 6, 18), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "4" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 13), ExternalReturnDate = new DateTime(2025, 6, 20), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "3" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 15), ExternalReturnDate = new DateTime(2025, 6, 22), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "2" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 17), ExternalReturnDate = new DateTime(2025, 6, 24), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "2" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 19), ExternalReturnDate = new DateTime(2025, 6, 26), ExternalSpaceAvailable = "0",    ExternalSingleAvailability = "0" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 21), ExternalReturnDate = new DateTime(2025, 6, 28), ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "4" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 23), ExternalReturnDate = new DateTime(2025, 6, 30), ExternalSpaceAvailable = "0",    ExternalSingleAvailability = "0" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 25), ExternalReturnDate = new DateTime(2025, 7, 2),  ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "2" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 27), ExternalReturnDate = new DateTime(2025, 7, 4),  ExternalSpaceAvailable = "< 10", ExternalSingleAvailability = "2" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 6, 29), ExternalReturnDate = new DateTime(2025, 7, 6),  ExternalSpaceAvailable = "< 10", ExternalSingleAvailability = "3" },
                new() { ExternalTripCode = "HOI", ExternalDepartureDate = new DateTime(2025, 7, 1),  ExternalReturnDate = new DateTime(2025, 7, 8),  ExternalSpaceAvailable = "10 +", ExternalSingleAvailability = "4" }
            };

            await client.SaveContentAsync(x => Guid.NewGuid().ToString(), tripDates);
        }
    }

    public class ExternalTripDates
    {
        //public int Id { get; set; }
        public string ExternalTripCode { get; set; } = string.Empty;
        public DateTime ExternalDepartureDate { get; set; }
        public DateTime ExternalReturnDate { get; set; }
        public string ExternalSpaceAvailable { get; set; } = string.Empty;
        public string ExternalSingleAvailability { get; set; } = string.Empty;
    }
}
