using System;
using System.Web.Http;
using Serilog;

namespace WebApiEF
{
    [RoutePrefix("import.json")]
    public class ApiController : System.Web.Http.ApiController
    {
        public ApiController(IUserInfoProvider userInfoProvider)
        {
            _userInfoProvider = userInfoProvider;
        }

        [Route("")]
        public void Post(SyncProfileRequest newRequest)
        {
            try
            {
                _userInfoProvider.AddUserInfo(newRequest);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Request processing raised an exception");
                throw exception;
            }
        }

        private IUserInfoProvider _userInfoProvider;
    }
}