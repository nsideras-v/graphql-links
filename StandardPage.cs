using EPiServer.Core;
using System.ComponentModel.DataAnnotations;

namespace graphql_link
{
    [ContentType(DisplayName = "Standard Page",
        GUID = "64574252-3db2-47d2-86a2-5444a75dccc4",
        GroupName = SystemTabNames.Content)]
    public class StandardPage : PageData
    {

        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 100)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(Name = "Main content area", GroupName = SystemTabNames.Content, Order = 200)]
        public virtual ContentArea MainContentArea { get; set; }
    }
}