using System;
using System.Collections.Generic;
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
            // We can dynamically add routes now. I believe this must be done in the app host constructor. Does not work in the Configure section.
            // https://github.com/ServiceStack/ServiceStack/wiki/Release-Notes#add-code-first-attributes-at-runtime-de-coupled-from-pocos
            typeof(Team).AddAttributes(new RouteAttribute("/teams/{Id}"));
            typeof(GetTeams).AddAttributes(new RouteAttribute("/teams"));
        }

        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature { AutoHandleOptionsRequests = true });

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            InitData(container);

            SetConfig(new HostConfig
                {
                    DebugMode = true,
                });
        }

        public static void InitData(Container container)
        {
            // dynamic OrmLite attributes can be added before using the DTO with OrmLite
            typeof(Team).GetProperty("Id").AddAttributes(new AutoIncrementAttribute());
            typeof(Team).GetProperty("HideMe").AddAttributes(new IgnoreAttribute());

            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
                db.CreateTableIfNotExists<Team>();

                var teams = new List<Team>
                    {
                        new Team() {Id = 1, Name = "Manchester United", CreatedDate = DateTime.Now, HideMe = "you shouldn't see this"},
                        new Team() {Id = 2, Name = "Arsenal", CreatedDate = DateTime.Now},
                        new Team() {Id = 3, Name = "Manchester City", CreatedDate = DateTime.Now},
                        new Team() {Id = 4, Name = "Chelsea", CreatedDate = DateTime.Now},
                        new Team() {Id = 5, Name = "Newcastle United", CreatedDate = DateTime.Now},
                    };

                db.InsertAll(teams);
            }
        }
    }

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