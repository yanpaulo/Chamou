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
using Chamou.Web.Models.Entities;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;

namespace Chamou.Web.ApiControllers
{
    public class PlacesController : ApiController
    {
        private ChamouContext db = new ChamouContext();

        // GET: api/Places
        /// <summary>
        /// Returns a list of all available Places
        /// </summary>
        /// <returns></returns>
        public IQueryable<Place> GetPlaces()
        {
            return db.Places;
        }

        // GET: api/Places/5
        /// <summary>
        /// Returns the specified Place
        /// </summary>
        /// <param name="id">Place's Id</param>
        /// <returns></returns>
        [ResponseType(typeof(Place))]
        public IHttpActionResult GetPlace(int id)
        {
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        /// <summary>
        /// Returns the Place which location encloses the specified point.
        /// </summary>
        /// <param name="latitude">Latitude for the point</param>
        /// <param name="longitude">Longitude for the point</param>
        /// <returns>Place, if it is found. Null otherwise.</returns>
        [Route("api/Places/ByCoordinates/")]
        public IHttpActionResult GetByCoordinates(double latitude, double longitude)
        {
            var geoPoint = DbGeography.FromText($"POINT ({GeoFormat(longitude)} {GeoFormat(latitude)})");
            var place = db.Places.FirstOrDefault(p => p.Location.Intersects(geoPoint));
            if (place == null)
            {
                return Ok();
            }

            return Ok(place);
        }

        // PUT: api/Places/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlace(int id, Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.Id)
            {
                return BadRequest();
            }

            db.Entry(place).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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


        // POST: api/Places
        [ResponseType(typeof(Place))]
        public IHttpActionResult PostPlace(Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Places.Add(place);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = place.Id }, place);
        }

        // DELETE: api/Places/5
        [ResponseType(typeof(Place))]
        public IHttpActionResult DeletePlace(int id)
        {
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return NotFound();
            }

            db.Places.Remove(place);
            db.SaveChanges();

            return Ok(place);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GeoFormat(double d) => d.ToString().Replace(',', '.');

        private bool PlaceExists(int id)
        {
            return db.Places.Count(e => e.Id == id) > 0;
        }
    }
}