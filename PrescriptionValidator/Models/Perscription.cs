using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class PerscriptionEntry
    {
        public int DrugVariantId { get; set; }
        public float Dose { get; set; }
        public int TimesPerDay { get; set; }
    }

    public class Perscription
    {
        public float PatientWeight { get; set; }
        public List<PerscriptionEntry> Drugs { get; set; }
    }
}