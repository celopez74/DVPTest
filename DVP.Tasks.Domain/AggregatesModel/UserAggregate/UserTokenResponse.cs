namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public class AadTokenResponse{
        public bool IsValidate { get; set; }
        public UserTokenResponse? TokenResponse { get; set; }
        public UserTokenErrorResponse? TokenErrorResponse { get; set; }

    }
    public class UserTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class UserTokenErrorResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}