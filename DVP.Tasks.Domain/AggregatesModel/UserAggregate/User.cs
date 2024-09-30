using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public class User: Entity<Guid>,IAggregateRoot    {
        
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;        
        public string Nickname { get; set; } = string.Empty;       
        public List<int>? Roles { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsEnabled { get; set;} = true;
    
        public User(Guid id): base(id)
        {
            Roles = new List<int>();
        }
        
        public User(Guid id, string name, string email, string nickname, List<int> roles): base(id)
        {           
            Id = id;
            Name = name;
            Email = email;            
            Nickname = nickname;

            if(roles == null) 
            {
                Roles = new List<int>();
            }
            else
            {
                Roles = roles;
            }
        }

        public void AddRole(int role)
        {
            if (Roles != null && !Roles.Contains(role))
            {
                Roles.Add(role);
            }
        }

        public void RemoveRole(int role)
        {
            if (Roles != null && Roles.Contains(role))
            {
                Roles.Remove(role);
            }
        }
    }
}