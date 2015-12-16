#region Using

using System.Web.Mvc;

#endregion

namespace StudyConfigurationServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}