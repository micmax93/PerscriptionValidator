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
    builder.EntitySet<Contraindication>("Contraindication");
    builder.EntitySet<Substance>("Substances"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ContraindicationController : ODataController
    {
        private MedDb db = new MedDb();

        // GET odata/Contraindication
        [Queryable]
        public IQueryable<Contraindication> GetContraindication()
        {
            return db.Contraindication;
        }

        // GET odata/Contraindication(5)
        [Queryable]
        public SingleResult<Contraindication> GetContraindication([FromODataUri] int key)
        {
            return SingleResult.Create(db.Contraindication.Where(contraindication => contraindication.Id == key));
        }

        // PUT odata/Contraindication(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Contraindication contraindication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != contraindication.Id)
            {
                return BadRequest();
            }

            db.Entry(contraindication).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContraindicationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(contraindication);
        }

        // POST odata/Contraindication
        public async Task<IHttpActionResult> Post(Contraindication contraindication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contraindication.Add(contraindication);
            await db.SaveChangesAsync();

            return Created(contraindication);
        }

        // PATCH odata/Contraindication(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Contraindication> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Contraindication contraindication = await db.Contraindication.FindAsync(key);
            if (contraindication == null)
            {
                return NotFound();
            }

            patch.Patch(contraindication);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContraindicationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(contraindication);
        }

        // DELETE odata/Contraindication(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Contraindication contraindication = await db.Contraindication.FindAsync(key);
            if (contraindication == null)
            {
                return NotFound();
            }

            db.Contraindication.Remove(contraindication);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Contraindication(5)/SubstanceA
        [Queryable]
        public SingleResult<Substance> GetSubstanceA([FromODataUri] int key)
        {
            return SingleResult.Create(db.Contraindication.Where(m => m.Id == key).Select(m => m.SubstanceA));
        }

        // GET odata/Contraindication(5)/SubstanceB
        [Queryable]
        public SingleResult<Substance> GetSubstanceB([FromODataUri] int key)
        {
            return SingleResult.Create(db.Contraindication.Where(m => m.Id == key).Select(m => m.SubstanceB));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContraindicationExists(int key)
        {
            return db.Contraindication.Count(e => e.Id == key) > 0;
        }
    }
}
