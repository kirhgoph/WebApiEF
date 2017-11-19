using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiEF
{
    public abstract class MyAccountRequestBase
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid RequestId { get; set; }
    }
}
