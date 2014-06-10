using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Chainbox.FoodApp.ViewModels
{
    public class FoodItemModel
    {
        public List<SelectListItem> FoodProviders { get; set; }
        public int FoodProviderId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool MemberIsAdmin { get; set; }
    }
}
