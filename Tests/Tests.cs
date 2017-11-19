using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.Data.Entity;
using Moq;
using System.Linq;
using System.Collections.Generic;

namespace WebApiEF.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CountryIsoCodeIsNull()
        {
            var sample = new SyncProfileRequest(_correctSample1)
            {
                CountryIsoCode = null
            };
            _mockController.Post(sample);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LocaleIsNull()
        {
            var sample = new SyncProfileRequest(_correctSample1)
            {
                Locale = null
            };
            _mockController.Post(sample);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LocaleIsIncorrect()
        {
            var sample = new SyncProfileRequest(_correctSample1)
            {
                Locale = "rus-RUS"
            };
            _mockController.Post(sample);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CountryIsoCodeIsIncorrect()
        {
            var sample = new SyncProfileRequest(_correctSample1)
            {
                CountryIsoCode = "rus"
            };
            _mockController.Post(sample);
        }

        [TestMethod]
        public void Import()
        {
            _mockContext.Setup(m => m.MyAccountRequestBases).Returns(_mockSet.Object);
            _mockController.Post(_correctSample1);
            _mockSet.Verify(m => m.Add(It.IsAny<MyAccountRequestBase>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void Update()
        {
             _mockController.Post(_correctSample1);
            Assert.IsNotNull(_mockSet.Object.Find(_correctSample1.UserId));
            Assert.AreEqual(1, _mockSet.Object.Count());
            _mockController.Post(_correctSample2);
            Assert.IsNotNull(_mockSet.Object.Find(_correctSample1.UserId));
            Assert.AreEqual(1, _mockSet.Object.Count());
            _mockSet.Verify(m => m.Add(It.IsAny<MyAccountRequestBase>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Get()
        {
            InitializeMockSet(new List<MyAccountRequestBase>() { _correctSample2 });
            UserInfoService userInfoService = new UserInfoService(_userInfoProvider);
            var UserInfoFromWcf = userInfoService.GetUserInfo(_correctSample2.UserId);
            Assert.AreEqual(new UserInfo(_correctSample2), UserInfoFromWcf);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<string>))]
        public void FailedGet()
        {
            InitializeMockSet(new List<MyAccountRequestBase>() { _correctSample2 });
            UserInfoService userInfoService = new UserInfoService(_userInfoProvider);
            var abc = userInfoService.GetUserInfo(new Guid("00000000-0000-0000-0000-000000000000"));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _guid = new Guid("AAAAAAAA-1111-2222-3333-000000000001");
            _correctSample1 = new SyncProfileRequest()
            {
                AdvertisingOptIn = true,
                CountryIsoCode = "ru",
                DateModified = new DateTime(1800, 06, 20, 13, 00, 15),
                Locale = "en",
                RequestId = new Guid("DEADBEEF-1234-4321-7531-314159265359"),
                UserId = _guid
            };
            _correctSample2 = new SyncProfileRequest()
            {
                AdvertisingOptIn = false,
                CountryIsoCode = "en",
                DateModified = new DateTime(2100, 08, 12, 11, 10, 25),
                Locale = "ru-RU",
                RequestId = new Guid("DEADBEEF-1234-4321-7531-314159265360"),
                UserId = _guid
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeMockSet(null);
            _mockContext = new Mock<MyAccountRequestBaseContext>();
            _mockContext.Setup(m => m.MyAccountRequestBases).Returns(_mockSet.Object);
            _userInfoProvider = new UserInfoProvider(_mockContext.Object);
            _mockController = new ApiController(_userInfoProvider);
            
        }

        private void InitializeMockSet(ICollection<MyAccountRequestBase> data)
        {
            Func<object[], MyAccountRequestBase> linqQuery = ids => _mockSet.Object.FirstOrDefault(d => d.UserId == (Guid)ids[0]);
            _mockSet = new Mock<DbSet<MyAccountRequestBase>>().SetupData(data, linqQuery);
        }

        static SyncProfileRequest _correctSample1;
        static SyncProfileRequest _correctSample2;
        static Guid _guid;
        private static Mock<DbSet<MyAccountRequestBase>> _mockSet;
        private static Mock<MyAccountRequestBaseContext> _mockContext;
        private static ApiController _mockController;
        private static UserInfoProvider _userInfoProvider;
    }
}