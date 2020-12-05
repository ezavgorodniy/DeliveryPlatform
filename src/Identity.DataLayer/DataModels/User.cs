using Identity.Contract;

namespace Identity.DataLayer.DataModels
{
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
