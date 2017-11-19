using System;
using Serilog;
using System.Text.RegularExpressions;

namespace WebApiEF
{
    public class UserInfoProvider : IUserInfoProvider
    {
        public UserInfoProvider(MyAccountRequestBaseContext context)
        {
            _context = context;
        }

        public UserInfo GetUserInfo(Guid guid)
        {
            try
            {
                using (var database = new MyAccountRequestBaseContext())
                {
                    SyncProfileRequest ProfileRequest = (SyncProfileRequest)database.MyAccountRequestBases.Find(guid);
                    if (ProfileRequest == null) throw new Exception("User not found");
                    return new UserInfo(ProfileRequest);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Getting user's info failed");
                throw exception;
            }
        }

        public void AddUserInfo(SyncProfileRequest newRequest)
        {
            try
            {
                ValidateSyncProfileRequest(newRequest);
                var alreadyExistingRequest = _context.MyAccountRequestBases.Find(newRequest.UserId);
                if (alreadyExistingRequest != null) _context.MyAccountRequestBases.Remove(alreadyExistingRequest);
                _context.MyAccountRequestBases.Add(newRequest);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Request processing raised an exception");
                throw exception;
            }
        }

        private void ValidateSyncProfileRequest(SyncProfileRequest request)
        {
            if (request.CountryIsoCode == null) throw new InvalidOperationException("CountryIsoCode is null");
            if (request.Locale == null) throw new InvalidOperationException("Locale is null");

            var countryIsoCodeMatch = request.CountryIsoCode.Length == 2;
            if (!countryIsoCodeMatch) throw new InvalidOperationException("Country ISO code is incorrect");

            var localeIsoMatch = Regex.Match(request.Locale, "^[a-z]{2}-[A-Z]{2}$|^[a-z]{2}$");
            if (!localeIsoMatch.Success) throw new InvalidOperationException("Locale is incorrect");
        }

        private MyAccountRequestBaseContext _context;
    }

    public interface IUserInfoProvider
    {
        UserInfo GetUserInfo(Guid userId);
        void AddUserInfo(SyncProfileRequest request);
    }
}