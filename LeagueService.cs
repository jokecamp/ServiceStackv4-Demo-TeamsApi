using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace ServiceStackv4_Demo_TeamsApi
{
    /// <summary>
    /// Sports league - Notice it does not use IReturn marker interface
    /// </summary>
    [Route("/leagues/{Id}")]
    public class League
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
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

        public object Post(League request)
        {
            Db.Insert(request);
            return Db.SingleById<League>(Db.LastInsertId());
        }

        public object Patch(League request)
        {
            var fields = Request.QueryString["fields"].Split(new[] { ',' });

            if (!fields.Any())
                throw new Exception("You must provide the fields to update via ?fields= in querystring");

            Db.UpdateOnly(request, delegate(SqlExpression<League> expression)
                {
                    foreach (var field in fields)
                    {
                        var match = ModelDefinition<League>.Definition.FieldDefinitions.FirstOrDefault(x => x.FieldName.ToLower() == field.ToLower());
                        if (match != null)
                            expression.UpdateFields.Add(match.FieldName);
                    }

                    return expression.Where(x => x.Id == request.Id);
                });

            // returning entire object. You may want to do a different response code
            return Db.SingleById<League>(request.Id);
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