using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Chainbox.FoodApp.Models;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace Chainbox.FoodApp.Controllers.Api
{
    public class FoodRelationController : UmbracoApiController
    {
        private const string FavoriteFoodRelationType = "FavoriteFood";

        [HttpGet]
        [MemberAuthorize(Roles = "Intranet Users")]
        public HttpResponseMessage GetFavoriteFoodItems()
        {
            var currentMemberId = Members.GetCurrentMemberId();
            var relations = Services.RelationService.GetByParentId(currentMemberId);
            var foodItems = Services.RelationService.GetChildEntitiesFromRelations(relations);
            var result = foodItems.Select(x => new {x.Id, x.Name, x.ParentId});

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [MemberAuthorize(Roles = "Intranet Users")]
        public HttpResponseMessage FavoriteFood(FavoriteFoodModel model)
        {
            var currentMemberId = Members.GetCurrentMemberId();
            var relationType = Services.RelationService.GetRelationTypeByAlias(FavoriteFoodRelationType);
            var relation = new Relation(currentMemberId, model.FoodItemId, relationType);
            Services.RelationService.Save(relation);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
