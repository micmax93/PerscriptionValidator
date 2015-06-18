using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PrescriptionValidator.Models;
using System.Collections;

namespace PrescriptionValidator.Controllers
{
    
    public class ValidationController : ApiController
    {
        public object Post([FromBody]Perscription perscription)
        {
            var db = new MedDb();
            var subs = new Dictionary<int, float>();
            foreach(var entry in perscription.Drugs)
            {
                var variant = db.Variants.Find(entry.DrugVariantId);
                if (variant == null) throw new Exception("Invalid DrugVariantId.");

                foreach(var part in variant.Composition)
                {
                    if (!subs.ContainsKey(part.SubstanceId))
                        subs[part.SubstanceId] = 0;
                    subs[part.SubstanceId] += part.Dose * entry.Dose * entry.TimesPerDay;
                }
            }

            var keys = subs.Keys.ToArray();
            var overdose = new List<Dosage>();
            var conflicts = new List<Contraindication>();
            for(int i=0; i<keys.Length-1; i++)
            {
                var s = db.Substances.Find(keys[i]);
                var ov = s.Dosage.Where(d => subs[s.Id] >= d.MaxDosage(perscription.PatientWeight));
                overdose.AddRange(ov);

                for(int j=i+1; j<keys.Length; j++)
                {
                    conflicts.AddRange(s.CheckSafety(keys[j]));
                }
            }

            var result = new Hashtable();
            result.Add("overdose", overdose);
            result.Add("conflicts", conflicts);
            return result;
        }
    }
}
