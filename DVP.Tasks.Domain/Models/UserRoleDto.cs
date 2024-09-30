using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.Models;

public class UserRoleDto : IDto
{
        public string UserId { get; set; } = string.Empty;
        public int RoleId { get; set; }        

}
