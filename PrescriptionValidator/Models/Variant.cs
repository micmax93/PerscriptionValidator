using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Form { get; set; }
        public string Comment { get; set; }
        public string Amount { get; set; }

        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual ICollection<Composition> Composition { get; set; }
    }
}