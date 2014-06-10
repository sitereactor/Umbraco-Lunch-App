using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Chainbox.FoodApp.Schema;
using Chainbox.FoodApp.ViewModels;
using Umbraco.Web.Mvc;

namespace Chainbox.FoodApp.Controllers
{
    public class FoodOrdersController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Index()
        {
            var model = new FoodOrdersModel { MemberIsAdmin = false, Orders = new List<FoodOrder>()};
            var memberId = Members.GetCurrentMemberId();
            var memberService = Services.MemberService;
            var member = memberService.GetById(memberId);
            if (Members.IsLoggedIn() && memberService.FindMembersInRole("Admin", member.Username).Any())
            {
                model.MemberIsAdmin = true;
            }
            else
            {
                return PartialView("~/Views/Partials/AdminFoodOrderPartial.cshtml", model);
            }

            var orderNumber = GetCurrentOrderDate();
            var orders = DatabaseContext.Database.Fetch<LunchOrderDto>("WHERE OrderDate = @OrderNumber ORDER BY MemberId",
                new { OrderNumber = orderNumber });
            var memberIds = orders.Select(x => x.MemberId).Distinct().ToArray();
            var members = Services.MemberService.GetAllMembers(memberIds).ToArray();

            foreach (var order in orders)
            {
                var memberName = members.First(m => m.Id.Equals(order.MemberId)).Name;
                model.Orders.Add(new FoodOrder
                                 {
                                     FoodItem = order.FoodItem,
                                     FoodItemId = order.FoodItemId,
                                     MemberId = order.MemberId,
                                     MemberName = memberName
                                 });
            }
            
            return PartialView("~/Views/Partials/AdminFoodOrderPartial.cshtml", model);
        }

        private long GetCurrentOrderDate()
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day);
            return date.Ticks;
        }
    }
}