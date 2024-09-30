using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public class AadPasswordProfile: IAggregateRoot
    {
        public bool ForceChangePasswordNextSignIn { get; set; } = false;
        public string Password { get; set; } = string.Empty;

    }
}