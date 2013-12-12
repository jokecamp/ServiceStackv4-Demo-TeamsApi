using System.Collections.Generic;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ServiceStackv4_Demo_TeamsApi
{
    /// <summary>
    /// Sports league - Notice it does not use IReturn marker interface
    /// </summary>
    [Route("/leagues/{Id}")]
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Route("/leagues")]
    public class GetLeagues
    {

    }

    public class LeagueService : Service
    {
        // You can now use DTOs that do not adhere to IReturn. Cool!
        public object Get(League request)
        {
            return Db.SingleById<League>(request.Id);
        }

        public object Get(GetLeagues request)
        {
            return Db.Select<League>();
        }
    }

    /// <summary>
    /// A consumer example using DTOs with and without IReturn
    /// </summary>
    public class ExampleConsumer
    {
        public IEnumerable<Team> GetAllTeams()
        {
            // GetTeams implements IReturn
            return new JsonServiceClient().Get(new GetTeams());
        }

        public IEnumerable<League> GetAllLeagues()
        {
            // GetLeagues does NOT implement IReturn
            // See https://github.com/ServiceStack/ServiceStack/wiki/Release-Notes#use-any-request-dto-in-client-apis
            return new JsonServiceClient().Get<List<League>>(new GetLeagues());
        }
    }
}