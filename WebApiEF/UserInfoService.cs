using System;
using System.ServiceModel;
using Serilog;

namespace WebApiEF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UserInfoService : IUserInfoService
    {
        public UserInfoService(IUserInfoProvider userInfoProvider)
        {
            _userInfoProvider = userInfoProvider;
        }

        public UserInfo GetUserInfo(Guid guid)
        {
            try
            {
                return _userInfoProvider.GetUserInfo(guid);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Getting user's info failed");
                throw new FaultException<string>("User not found");
            }
        }

        private IUserInfoProvider _userInfoProvider;
    }

    [ServiceContract]
    public interface IUserInfoService
    {
        [FaultContract(typeof(string))]
        [OperationContract]
        UserInfo GetUserInfo(Guid userId);
    }
}
