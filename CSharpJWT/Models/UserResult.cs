namespace CSharpJWT.Models
{
    using System.Collections.Generic;

    public class UserResult
    {
        public UserResult() { }

        public UserResult(IDictionary<string, object> token)
        {
            Successed = true;
            Token = token;
        }

        public UserResult(object error)
        {
            Successed = false;
            Error = error;
        }

        public bool Successed { get; set; }
        public IDictionary<string, object> Token { get; set; }
        public object Error { get; set; }
    }
}
