using Optimizely.ContentGraph.Cms.NetCore.Core;
using System.ComponentModel.DataAnnotations;

namespace graphql_link
{
    [ContentType(DisplayName = "Trip Page",
        GUID = "7585c5fc-b668-4045-9370-cf248ce4fc01",
        GroupName = SystemTabNames.Content)]
    public class TripPage : PageData
    {

        [Display(Name = "Trip Code", GroupName = SystemTabNames.Content, Order = 100)]
        [GraphProperty(PropertyIndexingMode.Default)]
        public virtual string TripCode { get; set; }

        [Display(Name = "Countries", GroupName = SystemTabNames.Content, Order = 200)]
        public virtual IList<string> Countries { get; set; }

        [Display(Name = "Region", GroupName = SystemTabNames.Content, Order = 300)]
        public virtual string Region { get; set; }

        [Display(Name = "Main content area", GroupName = SystemTabNames.Content, Order = 500)]
        public virtual ContentArea MainContentArea { get; set; }
    }
}