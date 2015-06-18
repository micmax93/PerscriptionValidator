using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<Variant> Variants { get; set; }
        public int ProducerId { get; set; }
        public virtual Producer Producer { get; set; }
    }
}