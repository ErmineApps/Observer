using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Observer;

namespace Observer.ControllersApi
{
    public class ViolationsController : ApiController
    {
        private ObserverDBEntities db = new ObserverDBEntities();

        // GET: api/Violations
        public IQueryable<Violations> GetViolations()
        {
            return db.Violations;
        }

        // GET: api/Violations/5
        [ResponseType(typeof(Violations))]
        public IHttpActionResult GetViolations(int id)
        {
            Violations violations = db.Violations.Find(id);
            if (violations == null)
            {
                return NotFound();
            }

            return Ok(violations);
        }

        // PUT: api/Violations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutViolations(int id, Violations violations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != violations.Id)
            {
                return BadRequest();
            }

            db.Entry(violations).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViolationsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Violations
        [ResponseType(typeof(Violations))]
        public IHttpActionResult PostViolations(Violations violations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Violations.Add(violations);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = violations.Id }, violations);
        }

        // DELETE: api/Violations/5
        [ResponseType(typeof(Violations))]
        public IHttpActionResult DeleteViolations(int id)
        {
            Violations violations = db.Violations.Find(id);
            if (violations == null)
            {
                return NotFound();
            }

            db.Violations.Remove(violations);
            db.SaveChanges();

            return Ok(violations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ViolationsExists(int id)
        {
            return db.Violations.Count(e => e.Id == id) > 0;
        }
    }
}