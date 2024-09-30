using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.RoleAggregate
{
    public class Role: Entity<int>, IAggregateRoot
    {
        public string RoleName { get; set; } = string.Empty;      
        public string RoleDescription { get; set; } = string.Empty;

        public Role(int id, string roleName, string roleDescription): base(id)
        {
            RoleName = roleName;
            RoleDescription = roleDescription;
        }

    }
    public enum RolesEnum
    {
        Administrator,        
        Suppervisor,     
        Employee 
    }
}