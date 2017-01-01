using System.Collections.Generic;

namespace AutoMapperComparison.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
