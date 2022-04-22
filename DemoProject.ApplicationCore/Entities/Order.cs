using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.ApplicationCore.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public  int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public  virtual  Product Product { get; set; }
    }
}
