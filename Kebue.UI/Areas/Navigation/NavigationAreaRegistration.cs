using System.Web.Mvc;

namespace Kebue.UI.Areas.Navigation
{
    public class NavigationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Navigation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Navigation_default",
                "Navigation/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Kebue.UI.Areas.Navigation.Controllers" }
            );
        }
    }
}