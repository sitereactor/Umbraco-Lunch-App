﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            var orderNumber = GetCurrentOrderDate();
            var orders = DatabaseContext.Database.Fetch<LunchOrderDto>("WHERE OrderDate = @OrderNumber ORDERBY MemberId",
                new { OrderNumber = orderNumber });
            var memberIds = orders.Select(x => x.MemberId).Distinct().ToArray();
            var members = Services.MemberService.GetAllMembers(memberIds);

            var result =
                orders.Select(x => new {x.FoodItemId, x.FoodItem, members.First(m => m.Id.Equals(x.MemberId)).Name});

            return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [HttpPost]
        [MemberAuthorize(Roles = "Intranet Users")]
        public HttpResponseMessage ResetOrderFood()
        {
            var orderNumber = GetCurrentOrderDate();
            var currentMemberId = Members.GetCurrentMemberId();

            var result =
                DatabaseContext.Database.Delete<LunchOrderDto>(
                    "WHERE OrderDate = @OrderNumber AND MemberId = @CurrentMemberId",
                    new {OrderNumber = orderNumber, CurrentMemberId = currentMemberId});

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [MemberAuthorize(Roles = "Intranet Users")]
        public HttpResponseMessage OrderFavorites(FavoritesFoodOrderModel model)
        {
            var currentMemberId = Members.GetCurrentMemberId();
            var orderNumber = GetCurrentOrderDate();

            var relations = Services.RelationService.GetByParentId(currentMemberId);
            var foodItems = Services.RelationService.GetChildEntitiesFromRelations(relations);
            var currentFavorites = foodItems.Where(x => x.ParentId == model.CurrentFoodSupplier);

            var ordered = new List<LunchOrderDto>();
            foreach (var favorite in currentFavorites)
            {
                var order = new LunchOrderDto
                            {
                                FoodItemId = favorite.Id,
                                FoodItem = favorite.Name,
                                OrderDate = orderNumber,
                                MemberId = currentMemberId
                            };
                var result = DatabaseContext.Database.Insert(order);
                ordered.Add(order);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ordered);
        }

        private long GetCurrentOrderDate()
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day);
            return date.Ticks;
        }
    }
}
