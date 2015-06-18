using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class Composition
    {
        public int Id { get; set; }
        public float Dose { get; set; }

        public int VariantId { get; set; }
        public virtual Variant Variant { get; set; }
        public int SubstanceId { get; set; }
        public virtual Substance Substance { get; set; }
    }
}