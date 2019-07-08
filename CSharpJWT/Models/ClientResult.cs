namespace CSharpJWT.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ClientResult
    {
        public ClientResult() { }

        public ClientResult(string id)
        {
            Successed = true;
            Id = id;
        }

        public ClientResult(object error)
        {
            Successed = false;
            Error = error;
        }

        public bool Successed { get; set; }
        public string Id { get; set; }
        public object Error { get; set; }
    }
}
