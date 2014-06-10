using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Chainbox.FoodApp.Models;
using Chainbox.FoodApp.Schema;
using Umbraco.Web.WebApi;

namespace Chainbox.FoodApp.Controllers.Api
{
    public class FoodOrderController : UmbracoApiController
    {
        [HttpGet]
        [MemberAuthorize(Roles = "Intranet Users")]
        public HttpResponseMessage GetOrderFoodItems()
        {
            var currentMemberId = Members.GetCurrentMemberId();
            var orderNumber = GetCurrentOrderDate();
            var result = DatabaseContext.Database.Fetch<LunchOrderDto>("WHERE OrderDate = @OrderNumber AND MemberId = @CurrentMemberId",
                new { OrderNumber = orderNumber, CurrentMemberId = currentMemberId});

            return Request.CreateResponse(HttpStatusCode.OK, result);
            
        }

        [HttpGet]
        [MemberAuthorize(Roles = "Admin")]
        public HttpResponseMessage GetAllOrderFoodItems()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [MemberAuthorize(Roles = "Intranet Users")]
        public HttpResponseMessage OrderFoodItem(OrderFoodModel model)
        {
            var orderNumber = GetCurrentOrderDate();
            var currentMemberId = Members.GetCurrentMemberId();
            var order = new LunchOrderDto
                        {
                            FoodItemId = model.FoodItemId,
                            FoodItem = model.FoodItem,
                            OrderDate = orderNumber,
                            MemberId = currentMemberId
                        };
            var result = DatabaseContext.Database.Insert(order);

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        private long GetCurrentOrderDate()
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day);
            return date.Ticks;
        }
    }
}
