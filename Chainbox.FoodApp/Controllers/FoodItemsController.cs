using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Chainbox.FoodApp.ViewModels;
using Umbraco.Web.Mvc;

namespace Chainbox.FoodApp.Controllers
{
    public class FoodItemsController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Index()
        {
            var contentService = Services.ContentService;
            var contentTypeService = Services.ContentTypeService;
            var contentType = contentTypeService.GetContentType("Menu");
            var foodProviders = contentService.GetContentOfContentType(contentType.Id);

            var model = new FoodItemModel { FoodProviders = new List<SelectListItem>() };
            foreach (var provider in foodProviders)
            {
                model.FoodProviders.Add(new SelectListItem
                                        {
                                            Value = provider.Id.ToString(CultureInfo.InvariantCulture),
                                            Text = provider.Name
                                        });
            }

            var memberId = Members.GetCurrentMemberId();
            var memberService = Services.MemberService;
            var member = memberService.GetById(memberId);
            if (Members.IsLoggedIn() && memberService.FindMembersInRole("Admin", member.Username).Any())
            {
                model.MemberIsAdmin = true;
            }
               
            return PartialView("~/Views/Partials/AdminFoodPartial.cshtml", model);
        }

        public ActionResult AddFoodItem(FoodItemModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var contentService = Services.ContentService;
            var content = contentService.CreateContent(model.Title, model.FoodProviderId, "FoodItem");
            content.SetValue("name", model.Name);

            contentService.SaveAndPublishWithStatus(content);

            return RedirectToCurrentUmbracoPage();
        }
    }
}
