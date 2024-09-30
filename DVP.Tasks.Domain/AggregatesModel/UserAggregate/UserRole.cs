using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public class UserRole: IAggregateRoot
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; } 

        public UserRole (Guid userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }       
    }
}