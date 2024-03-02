using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TestRESTAPI.Data.Models
{
    public class Order
    {
        [Key] 
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
