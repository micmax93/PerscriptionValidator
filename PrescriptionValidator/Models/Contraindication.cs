using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class Contraindication
    {
        public int Id { get; set; }
        public ThreatType Type { get; set; }
        public string Comment { get; set; }

        public int SubstanceAId { get; set; }
        public virtual Substance SubstanceA { get; set; }
        public int SubstanceBId { get; set; }
        public virtual Substance SubstanceB { get; set; }
    }
}