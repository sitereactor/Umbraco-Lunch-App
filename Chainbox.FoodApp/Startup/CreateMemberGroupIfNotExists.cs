using Umbraco.Core;
using Umbraco.Core.Models;

namespace Chainbox.FoodApp.Startup
{
    public class CreateMemberGroupIfNotExists : ApplicationEventHandler
    {
        const string AdminGroupName = "Admin";
        const string IntranetGroupName = "Intranet Users";

        protected override void ApplicationStarted(UmbracoApplicationBase app, ApplicationContext ctx)
        {
            var memberGroupService = ctx.Services.MemberGroupService;
            var adminGroup = memberGroupService.GetByName(AdminGroupName);
            if (adminGroup == null)
            {
                memberGroupService.Save(new MemberGroup { Name = AdminGroupName });
            }

            var intranetGroup = memberGroupService.GetByName(IntranetGroupName);
            if (intranetGroup == null)
            {
                memberGroupService.Save(new MemberGroup { Name = IntranetGroupName });
            }
        }
    }
}