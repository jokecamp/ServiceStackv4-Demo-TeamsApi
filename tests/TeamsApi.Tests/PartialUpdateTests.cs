using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceStack;
using ServiceStackv4_Demo_TeamsApi;

namespace TeamsApi.Tests
{
    [TestFixture]
    public class PartialUpdateTests
    {
        [Test]
        public void CanPerform_PartialUpdate()
        {
            var client = new JsonServiceClient("http://localhost:53990/api/");

            // back date for readability
            var created = DateTime.Now.AddHours(-2);

            // Create a record so we can patch it 
            var league = new League() {Name = "BEFORE", Abbreviation = "BEFORE", DateUpdated = created, DateCreated = created};
            var newLeague = client.Post<League>(league);

            // Update Name and DateUpdated fields. Notice I don't want to update DateCreatedField. 
            // I also added a fake field to show it does not cause any errors
            var updated = DateTime.Now;
            newLeague.Name = "AFTER";
            newLeague.Abbreviation = "AFTER"; // setting to after but it should not get updated
            newLeague.DateUpdated = updated;

            client.Patch<League>("http://localhost:53990/api/leagues/" + newLeague.Id + "?fields=Name,DateUpdated,thisFieldDoesNotExist", newLeague);

            var updatedLeague = client.Get<League>(newLeague);

            Assert.AreEqual(updatedLeague.Name, "AFTER");
            Assert.AreEqual(updatedLeague.Abbreviation, "BEFORE");
            Assert.AreEqual(updatedLeague.DateUpdated.ToString(), updated.ToString(), "update fields don't match");
            Assert.AreEqual(updatedLeague.DateCreated.ToString(), created.ToString(), "created fields don't match");
            
            // double check
            Assert.AreNotEqual(updatedLeague.DateCreated, updatedLeague.DateUpdated);
        }
    }
}
