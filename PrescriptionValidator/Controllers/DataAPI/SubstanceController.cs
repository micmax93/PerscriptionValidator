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
    builder.EntitySet<Substance>("Substance");
    builder.EntitySet<Composition>("Composition"); 
    builder.EntitySet<Dosage>("Dosage"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SubstanceController : ODataController
    {
        private MedDb db = new MedDb();

        // GET odata/Substance
        [Queryable]
        public IQueryable<Substance> GetSubstance()
        {
            return db.Substances;
        }

        // GET odata/Substance(5)
        [Queryable]
        public SingleResult<Substance> GetSubstance([FromODataUri] int key)
        {
            return SingleResult.Create(db.Substances.Where(substance => substance.Id == key));
        }

        // PUT odata/Substance(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Substance substance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != substance.Id)
            {
                return BadRequest();
            }

            db.Entry(substance).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubstanceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(substance);
        }

        // POST odata/Substance
        public async Task<IHttpActionResult> Post(Substance substance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Substances.Add(substance);
            await db.SaveChangesAsync();

            return Created(substance);
        }

        // PATCH odata/Substance(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Substance> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Substance substance = await db.Substances.FindAsync(key);
            if (substance == null)
            {
                return NotFound();
            }

            patch.Patch(substance);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubstanceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(substance);
        }

        // DELETE odata/Substance(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Substance substance = await db.Substances.FindAsync(key);
            if (substance == null)
            {
                return NotFound();
            }

            db.Substances.Remove(substance);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Substance(5)/Composition
        [Queryable]
        public IQueryable<Composition> GetComposition([FromODataUri] int key)
        {
            return db.Substances.Where(m => m.Id == key).SelectMany(m => m.Composition);
        }

        // GET odata/Substance(5)/Dosage
        [Queryable]
        public IQueryable<Dosage> GetDosage([FromODataUri] int key)
        {
            return db.Substances.Where(m => m.Id == key).SelectMany(m => m.Dosage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubstanceExists(int key)
        {
            return db.Substances.Count(e => e.Id == key) > 0;
        }
    }
}
