using ServiceStack;
using ServiceStack.Model;

namespace ServiceStackv4_Demo_TeamsApi
{
    [Route("/players/{Id}")]
    public class Player : IHasId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // https://github.com/ServiceStack/ServiceStack/wiki/Restricting-Services#restrict-services
    [Restrict(LocalhostOnly = true)]
    public class PlayerService : Service
    {
        /* You can bypass this role by calling /api/players/1?authsecret=opensesame
         * 
         * New Feature:
         * Users in the Admin role have super-user access giving them access to all protected resources. 
         * You can also use Config.AdminAuthSecret to specify a special string to give you admin access 
         * without having to login by adding ?authsecret=xxx to the query string.
         * 
         */
        [RequiredRole("FakeRoleNoOneHas")]
        public object Get(Player request)
        {
            return new Player() { Id = 1, Name = "John Doe"};
        }
    }
}