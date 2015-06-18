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
    builder.EntitySet<Variant>("Variant");
    builder.EntitySet<Composition>("Composition"); 
    builder.EntitySet<Product>("Products"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class VariantController : ODataController
    {
        private MedDb db = new MedDb();

        // GET odata/Variant
        [Queryable]
        public IQueryable<Variant> GetVariant()
        {
            return db.Variants;
        }

        // GET odata/Variant(5)
        [Queryable]
        public SingleResult<Variant> GetVariant([FromODataUri] int key)
        {
            return SingleResult.Create(db.Variants.Where(variant => variant.Id == key));
        }

        // PUT odata/Variant(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Variant variant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != variant.Id)
            {
                return BadRequest();
            }

            db.Entry(variant).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(variant);
        }

        // POST odata/Variant
        public async Task<IHttpActionResult> Post(Variant variant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Variants.Add(variant);
            await db.SaveChangesAsync();

            return Created(variant);
        }

        // PATCH odata/Variant(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Variant> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Variant variant = await db.Variants.FindAsync(key);
            if (variant == null)
            {
                return NotFound();
            }

            patch.Patch(variant);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(variant);
        }

        // DELETE odata/Variant(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Variant variant = await db.Variants.FindAsync(key);
            if (variant == null)
            {
                return NotFound();
            }

            db.Variants.Remove(variant);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Variant(5)/Composition
        [Queryable]
        public IQueryable<Composition> GetComposition([FromODataUri] int key)
        {
            return db.Variants.Where(m => m.Id == key).SelectMany(m => m.Composition);
        }

        // GET odata/Variant(5)/Product
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Variants.Where(m => m.Id == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VariantExists(int key)
        {
            return db.Variants.Count(e => e.Id == key) > 0;
        }
    }
}
