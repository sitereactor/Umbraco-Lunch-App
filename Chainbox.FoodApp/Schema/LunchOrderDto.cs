using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Chainbox.FoodApp.Schema
{
    [TableName("LunchOrders")]
    [PrimaryKey("Id")]
    public class LunchOrderDto
    {
        [PrimaryKeyColumn(Name = "PK_structure")]
        public int Id { get; set; }

        public int MemberId { get; set; }

        public long OrderDate { get; set; }

        public int FoodItemId { get; set; }

        [Length(2500)]
        public string FoodItem { get; set; }
    }
}