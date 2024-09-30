using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public class AadUser: IAggregateRoot
    {
        public string id { get; set; } = string.Empty;
        public string UserPrincipalName { get; set; } = string.Empty;      
        public string DisplayName { get; set; } = string.Empty;
        public string MailNickname { get; set; } = string.Empty;
        public bool AccountEnabled { get; set; } = false;
        public AadPasswordProfile PasswordProfile { get; set; } = new AadPasswordProfile();
    }
}
