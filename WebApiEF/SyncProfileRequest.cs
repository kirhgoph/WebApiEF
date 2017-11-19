using System;

namespace WebApiEF
{
    public class SyncProfileRequest : MyAccountRequestBase
    {
        public bool? AdvertisingOptIn { get; set; }
        public string CountryIsoCode { get; set; }
        public DateTime DateModified { get; set; }
        public string Locale { get; set; }

        public SyncProfileRequest(SyncProfileRequest syncProfileRequest)
        {
            AdvertisingOptIn = syncProfileRequest.AdvertisingOptIn;
            CountryIsoCode = syncProfileRequest.CountryIsoCode;
            DateModified = syncProfileRequest.DateModified;
            Locale = syncProfileRequest.Locale;
            RequestId = syncProfileRequest.RequestId;
            UserId = syncProfileRequest.UserId;
        }

        public SyncProfileRequest(){}

        public override bool Equals(object obj)
        {
            if (obj is SyncProfileRequest)
            {
                var that = obj as SyncProfileRequest;
                return RequestId == that.RequestId
                    && UserId == that.UserId
                    && AdvertisingOptIn == that.AdvertisingOptIn
                    && CountryIsoCode == that.CountryIsoCode
                    && DateModified == that.DateModified
                    && Locale == that.Locale;
            }

            return false;
        }
    }
}