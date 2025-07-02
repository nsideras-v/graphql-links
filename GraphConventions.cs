using EPiServer.Framework.Initialization;
using EPiServer.Framework;
using Optimizely.ContentGraph.Cms.NetCore.ConventionsApi;
using EPiServer.Web;
using EPiServer.ServiceLocation;

namespace graphql_link
{
    [ModuleDependency(typeof(InitializationModule))]
    public class GraphConventions : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var conventionRepo = context.Locate.Advanced.GetInstance<ConventionRepository>();

            conventionRepo?
                .ForInstancesOf<TripPage>()
                .Set(page => page.TripCode, IndexingType.Queryable);
        }
        public void Uninitialize(InitializationEngine context) { }
    }
}
