using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining.Entities
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

    }
}
