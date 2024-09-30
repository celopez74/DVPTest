using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.Models;

public class RoleDto : IDto
{
        public string Id { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set;}  = string.Empty;    
}
