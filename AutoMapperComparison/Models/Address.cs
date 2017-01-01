using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMapperComparison.Models
{
    public class Address
    {
        public int Id { get; set; }

        public double? Code { get; set; }

        public string Title { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
