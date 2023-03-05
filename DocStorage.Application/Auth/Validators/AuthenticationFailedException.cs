using DocStorage.Application.Resources;

namespace DocStorage.Application.Auth.Validators
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException() : base(Messages.ResourceManager.GetString("AuthenticationFailedException"))
        {

        }
    }
}

