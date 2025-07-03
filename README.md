## Content Graph Example

This project uses an embedded SQL database and should be generally ready to run using Visual Studio on Windows.

**Before running, add Content Graph connection details to `appsettings.Development.json`**


Steps:

1. Run the project and log into the [backend ](https://localhost:5000/episerver/CMS) with the credentials `admin` / `Admin1!!`
2. Ensure that the `Optimizely Graph content synchronization job` has run to sync CMS content to the `default` content source.
3. Run the `External Trip Dates Data Sync` scheduled job to sync additional trip data to the `src1` content source.
4. Manually create links between the CMS TripPage `TripCode` poperty and the `src1` ExternalTripDates `ExternalTripCode` field with the `https://prod.cg.optimizely.com/api/content/v3/sources?id=default` endpoint
5. Run the following GraphQL query:
   ````GraphQL
   query PageQuery {
     TripPage {
       total
       items {
         TripCode
         Name
         _link(type: TRIPDATES) {
           ExternalTripDates {
             items {
               ExternalTripCode
               ExternalSpaceAvailable
               ExternalSingleAvailability
               ExternalReturnDate
               ExternalDepartureDate
             }
           }
         }
       }
     }
   }
   ````
