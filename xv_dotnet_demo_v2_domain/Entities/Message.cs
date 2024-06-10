using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace xv_dotnet_demo_v2_domain.Entities
{
    [Table("message")]
    public class Message
    {
        [Key]
        [Required]
        public int id { get; set; }
        [StringLength(50)]
        public string message { get; set; }
    }
}
