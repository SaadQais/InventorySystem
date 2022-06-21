namespace InventorySystem.Application.Models.Role
{
    public static class RolesVM
    {
        public readonly static List<string> Roles;

        static RolesVM()
        {
            Roles = new List<string>
            {
                "Admin"
            };
        }

        public const string Admin = "Admin";

        public static IList<string> GetAll()
        {
            return new List<string>() { "Admin" };
        }
    }
}
