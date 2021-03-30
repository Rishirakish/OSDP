namespace Neubel.Wow.Win.Authentication.Core.Model
{ 
    public class Role : Entity
    {

        public string Name { get; set; }
        public int Level { get; set; }
    }
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string GeneralUser = "GeneralUser";
    }
}
