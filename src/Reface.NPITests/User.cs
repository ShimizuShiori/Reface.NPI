using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reface.NPITests
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
