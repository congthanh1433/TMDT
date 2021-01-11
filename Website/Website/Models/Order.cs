using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime Date { get; set; }
        public string Descriptions { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public string PhuongThuc { get; set; }
        public string TinhTrang { get; set; }
    }
}