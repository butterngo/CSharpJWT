namespace CSharpJWT.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Scope
    {
        public Scope()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public Scope(string name):this()
        {
            Name = name;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
