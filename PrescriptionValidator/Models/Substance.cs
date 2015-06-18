using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class Substance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    
        public virtual ICollection<Composition> Composition { get; set; }
        public virtual ICollection<Dosage> Dosage { get; set; }
        public virtual IQueryable<Contraindication> Contraindications
        {
            get
            {
                return new MedDb().Contraindication.Where(c => c.SubstanceAId == this.Id || c.SubstanceBId == this.Id);
            }
        }

        public virtual IQueryable<Contraindication> CheckSafety(int subId)
        {
            return this.Contraindications.Where(c => c.SubstanceAId == subId || c.SubstanceBId == subId);
        }
    }
}