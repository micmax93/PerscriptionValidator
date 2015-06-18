using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class Dosage
    {
        public Dosage()
        {
            this.PerKg = false;
        }

        public int Id { get; set; }
        public double Value { get; set; }
        public bool PerKg { get; set; }
        public ThreatType Type { get; set; }
        public string Comment { get; set; }
        public int SubstanceId { get; set; }
        public virtual Substance Substance { get; set; }

        public double MaxDosage(double weight)
        {
            if (!PerKg) return Value;
            else return weight * Value;
        }
    }
}