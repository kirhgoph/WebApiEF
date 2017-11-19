using StructureMap;

namespace WebApiEF
{
    class ApiControllerRegistry : Registry
    {
        public ApiControllerRegistry()
        {
            For<IUserInfoProvider>().Use<UserInfoProvider>();
        }
    }
}
