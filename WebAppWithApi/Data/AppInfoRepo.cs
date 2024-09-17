using System;

namespace WebAppWithApi.Data
{
    public interface IAppInfoRepo
    {
        AppInfo GetAppInfo(Guid appInfoId);
    }

    public class AppInfoRepo : IAppInfoRepo
    {
        public AppInfo GetAppInfo(Guid appInfoId)
        {
            return new AppInfo
            {
                AppInfoId = appInfoId,
                FirstName = "Peter",
                LastName = "Lemonjello",
            };
        }
    }

    public class AppInfo
    {
        public Guid AppInfoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}