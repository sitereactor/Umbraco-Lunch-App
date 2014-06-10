using Chainbox.FoodApp.Schema;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Chainbox.FoodApp.Startup
{
    public class CreateSchemaIfNotExists : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase app, ApplicationContext ctx)
        {
            var db = ctx.DatabaseContext.Database;

            if (db.TableExist("LunchOrders") == false)
                db.CreateTable<LunchOrderDto>();
        }
    }
}