namespace CSharpJWT.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientScope
    {
        [Key]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public string ScopeId { get; set; }
        [ForeignKey("ScopeId")]
        public virtual Scope Scope { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
