using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using PrescriptionValidator.Models;

namespace PrescriptionValidator.Controllers.DataAPI
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using PrescriptionValidator.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Dosage>("Dosage");
    builder.EntitySet<Substance>("Substances"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DosageController : ODataController
    {
        private MedDb db = new MedDb();

        // GET odata/Dosage
        [Queryable]
        public IQueryable<Dosage> GetDosage()
        {
            return db.Dosage;
        }

        // GET odata/Dosage(5)
        [Queryable]
        public SingleResult<Dosage> GetDosage([FromODataUri] int key)
        {
            return SingleResult.Create(db.Dosage.Where(dosage => dosage.Id == key));
        }

        // PUT odata/Dosage(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Dosage dosage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != dosage.Id)
            {
                return BadRequest();
            }

            db.Entry(dosage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DosageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dosage);
        }

        // POST odata/Dosage
        public async Task<IHttpActionResult> Post(Dosage dosage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dosage.Add(dosage);
            await db.SaveChangesAsync();

            return Created(dosage);
        }

        // PATCH odata/Dosage(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Dosage> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dosage dosage = await db.Dosage.FindAsync(key);
            if (dosage == null)
            {
                return NotFound();
            }

            patch.Patch(dosage);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DosageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dosage);
        }

        // DELETE odata/Dosage(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Dosage dosage = await db.Dosage.FindAsync(key);
            if (dosage == null)
            {
                return NotFound();
            }

            db.Dosage.Remove(dosage);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Dosage(5)/Substance
        [Queryable]
        public SingleResult<Substance> GetSubstance([FromODataUri] int key)
        {
            return SingleResult.Create(db.Dosage.Where(m => m.Id == key).Select(m => m.Substance));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DosageExists(int key)
        {
            return db.Dosage.Count(e => e.Id == key) > 0;
        }
    }
}
