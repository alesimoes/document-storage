namespace DocStorage.Application.Adapters
{
    public class AuthenticatedUser
    {
        public string Token { get; set; }

        public AuthenticatedUser(string token)
        {
            Token = token;
        }
    }
}
