namespace CSharpJWT.Models
{
    using CSharpJWT.Domain;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ClientResult
    {
        public ClientResult() { }

        public ClientResult(Client client)
        {
            Succeeded = true;
            Client = client;
        }

        public ClientResult(object error)
        {
            Succeeded = false;
            Error = error;
        }

        public bool Succeeded { get; set; }
        public Client Client { get; set; }
        public object Error { get; set; }
    }
}
