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

    using System.WDefault1eb.Http.OData.Builder;
    using PrescriptionValidator.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Composition>("Composition");
    builder.EntitySet<Substance>("Substances"); 
    builder.EntitySet<Variant>("Variants"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CompositionController : ODataController
    {
        private MedDb db = new MedDb();

        // GET odata/Composition
        [Queryable]
        public IQueryable<Composition> GetComposition()
        {
            return db.Composition;
        }

        // GET odata/Composition(5)
        [Queryable]
        public SingleResult<Composition> GetComposition([FromODataUri] int key)
        {
            return SingleResult.Create(db.Composition.Where(composition => composition.Id == key));
        }

        // PUT odata/Composition(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Composition composition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != composition.Id)
            {
                return BadRequest();
            }

            db.Entry(composition).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompositionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(composition);
        }

        // POST odata/Composition
        public async Task<IHttpActionResult> Post(Composition composition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Composition.Add(composition);
            await db.SaveChangesAsync();

            return Created(composition);
        }

        // PATCH odata/Composition(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Composition> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Composition composition = await db.Composition.FindAsync(key);
            if (composition == null)
            {
                return NotFound();
            }

            patch.Patch(composition);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompositionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(composition);
        }

        // DELETE odata/Composition(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Composition composition = await db.Composition.FindAsync(key);
            if (composition == null)
            {
                return NotFound();
            }

            db.Composition.Remove(composition);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Composition(5)/Substance
        [Queryable]
        public SingleResult<Substance> GetSubstance([FromODataUri] int key)
        {
            return SingleResult.Create(db.Composition.Where(m => m.Id == key).Select(m => m.Substance));
        }

        // GET odata/Composition(5)/Variant
        [Queryable]
        public SingleResult<Variant> GetVariant([FromODataUri] int key)
        {
            return SingleResult.Create(db.Composition.Where(m => m.Id == key).Select(m => m.Variant));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompositionExists(int key)
        {
            return db.Composition.Count(e => e.Id == key) > 0;
        }
    }
}
