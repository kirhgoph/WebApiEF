using System;
using System.Runtime.Serialization;

namespace WebApiEF
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public bool? AdvertisingOptIn { get; set; }
        [DataMember]
        public string CountryIsoCode { get; set; }
        [DataMember]
        public DateTime DateModified { get; set; }
        [DataMember]
        public string Locale { get; set; }

        public UserInfo(SyncProfileRequest request)
        {
            UserId = request.UserId;
            AdvertisingOptIn = request.AdvertisingOptIn;
            CountryIsoCode = request.CountryIsoCode;
            DateModified = request.DateModified;
            Locale = request.Locale;
        }

        public override bool Equals(object obj)
        {
            if (obj is UserInfo)
            {
                var that = obj as UserInfo;
                return UserId == that.UserId
                    && AdvertisingOptIn == that.AdvertisingOptIn
                    && CountryIsoCode == that.CountryIsoCode
                    && DateModified == that.DateModified
                    && Locale == that.Locale;
            }

            return false;
        }
    }
}