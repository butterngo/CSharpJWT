namespace CSharpJWT.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Client 
    {
        public Client()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public bool Enabled { get; set; }
        public string Secret { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
