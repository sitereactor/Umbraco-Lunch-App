using System.Collections.Generic;

namespace Chainbox.FoodApp.ViewModels
{
    public class FoodOrdersModel
    {
        public bool MemberIsAdmin { get; set; }
        public List<FoodOrder> Orders { get; set; }
    }

    public class FoodOrder
    {
        public int FoodItemId { get; set; }
        public string FoodItem { get; set; }
        public string MemberName { get; set; }
        public int MemberId { get; set; }
    }
}