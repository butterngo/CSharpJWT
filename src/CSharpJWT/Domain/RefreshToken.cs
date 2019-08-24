namespace CSharpJWT.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RefreshToken
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
