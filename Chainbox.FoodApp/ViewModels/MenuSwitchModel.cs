using Umbraco.Core.Models;

namespace Chainbox.FoodApp.ViewModels
{
    public class MenuSwitchModel
    {
        public int CurrentMenu { get; set; }
        public IPublishedContent PublishedContent { get; set; }
    }
}