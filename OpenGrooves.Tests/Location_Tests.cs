using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenGrooves.Services.Caching;
using OpenGrooves.Services.Mapping;

namespace OpenGrooves.Tests
{
    [TestClass]
    public class Location_Tests
    {
        ILocationService location { get; set; }
        ICacheService cache { get; set; }

        public Location_Tests()
        {
            // geo location
            location = new GoogleLocationService();

            // caching
            //cache = new DirectSqlCacheStub();
            //cache.DataProvider = new UserSettingsCacheDataProvider(new Guid("7AF3989D-DE36-4B70-904F-9DECD0DFB2A4"));
        }

        [TestMethod]
        public void City_State_Zip_Returned_From_Zip()
        {
            var zip = "21122";

            var coord = location.GetLocation(zip);

            Assert.IsTrue(coord != null);
            Assert.IsTrue(coord.Zip.Equals(zip));
            Assert.IsTrue(coord.Zip != null);
            Assert.IsTrue(coord.City != null);
            Assert.IsTrue(coord.State != null);
        }

        //[TestMethod]
        //public void Cache_Save_Get()
        //{
        //    var key = "TestKey";
        //    var value = "TestValue";

        //    cache.Save(key, value);

        //    Assert.IsNotNull(cache.Get<string>(key));
        //    Assert.AreEqual<string>(value, cache.Get<string>(key));
        //}
    }
}
