namespace CSharpJWT.Models
{
    using System.Collections.Generic;

    public class UserResult
    {
        public UserResult() => Succeeded = true;

        public UserResult(IDictionary<string, object> token)
        {
            Succeeded = true;
            Token = token;
        }

        public UserResult(object error)
        {
            Succeeded = false;
            Error = error;
        }

        public bool Succeeded { get; set; }
        public IDictionary<string, object> Token { get; set; }
        public object Error { get; set; }
    }
}
