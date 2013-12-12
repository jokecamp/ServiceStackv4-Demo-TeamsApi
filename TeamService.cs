using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ServiceStackv4_Demo_TeamsApi
{
    /// <summary>
    /// A sports team DTO
    /// 
    /// Clean POCO that has no Attributes or routes if that is your cup of tea
    /// 
    /// </summary>
    public class Team : IReturn<Team>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public string HideMe { get; set; }
    }
    public class GetTeams : IReturn<List<Team>> { }

    public class TeamService : Service
    {
        public object Get(GetTeams request)
        {
            // New OrmLite Syntax
            // https://github.com/ServiceStack/ServiceStack/wiki/Release-Notes#improved-consistency
            return Db.Select<Team>();
        }

        public object Get(Team request)
        {
            // object or returns null
            return Db.SingleById<Team>(request.Id);
        }
    }
}