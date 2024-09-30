using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.Models;

public class UserDto : IDto
{
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;        
        public string Nickname { get; set; } = "";       
        public List<int>? Roles { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsEnabled { get; set;} = true;    

}
