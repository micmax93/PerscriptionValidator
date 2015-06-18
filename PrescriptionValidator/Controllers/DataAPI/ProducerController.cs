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
    builder.EntitySet<Producer>("Producer");
    builder.EntitySet<Product>("Product"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProducerController : ODataController
    {
        private MedDb db = new MedDb();

        // GET odata/Producer
        [Queryable]
        public IQueryable<Producer> GetProducer()
        {
            return db.Producers;
        }

        // GET odata/Producer(5)
        [Queryable]
        public SingleResult<Producer> GetProducer([FromODataUri] int key)
        {
            return SingleResult.Create(db.Producers.Where(producer => producer.Id == key));
        }

        // PUT odata/Producer(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Producer producer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != producer.Id)
            {
                return BadRequest();
            }

            db.Entry(producer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(producer);
        }

        // POST odata/Producer
        public async Task<IHttpActionResult> Post(Producer producer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Producers.Add(producer);
            await db.SaveChangesAsync();

            return Created(producer);
        }

        // PATCH odata/Producer(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Producer> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Producer producer = await db.Producers.FindAsync(key);
            if (producer == null)
            {
                return NotFound();
            }

            patch.Patch(producer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(producer);
        }

        // DELETE odata/Producer(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Producer producer = await db.Producers.FindAsync(key);
            if (producer == null)
            {
                return NotFound();
            }

            db.Producers.Remove(producer);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Producer(5)/Products
        [Queryable]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Producers.Where(m => m.Id == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProducerExists(int key)
        {
            return db.Producers.Count(e => e.Id == key) > 0;
        }
    }
}
