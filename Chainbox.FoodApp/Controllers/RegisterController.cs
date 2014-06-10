using System.Web.Mvc;
using Chainbox.FoodApp.ViewModels;
using Umbraco.Web.Mvc;

namespace Chainbox.FoodApp.Controllers
{
    public class RegisterController : SurfaceController
    {
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = Services.MemberService;
            if (memberService.GetByEmail(model.Email) != null)
            {
                ModelState.AddModelError("", "A Member with that email already exists");
                return CurrentUmbracoPage();
            }

            var member = memberService.CreateMemberWithIdentity(model.Email, model.Email, model.Name, "Member");
            memberService.AssignRole(member.Username, "Intranet Users");
            memberService.Save(member);
            memberService.SavePassword(member, model.Password);

            Members.Login(model.Email, model.Password);

            return Redirect("/");
        }
    }
}
