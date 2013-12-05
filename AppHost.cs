using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace ServiceStackv4_Demo_TeamsApi
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Test Razor", typeof (AppHost).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            InitData(container);

            SetConfig(new HostConfig
                {
                    DebugMode = true,
                });
        }

        public static void InitData(Container container)
        {
            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.CreateTableIfNotExists<Team>();

                var teams = new List<Team>
                    {
                        new Team() {Id = 1, Name = "Manchester United"},
                        new Team() {Id = 2, Name = "Arsenal"},
                        new Team() {Id = 3, Name = "Manchester City"},
                        new Team() {Id = 4, Name = "Chelsea"},
                        new Team() {Id = 5, Name = "Newcastle United"},
                    };

                db.InsertAll(teams);
            }
        }
    }

    /// <summary>
    /// A sports team DTO
    /// </summary>
    [Route("/teams/{Id}")]
    public class Team : IReturn<Team>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Route("/teams")]
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