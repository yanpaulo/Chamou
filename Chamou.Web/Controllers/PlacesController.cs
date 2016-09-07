using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chamou.Web.Models.Entities;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;

namespace Chamou.Web.Controllers
{
    public class PlacesController : Controller
    {
        private ChamouContext db = new ChamouContext();

        // GET: Places
        public ActionResult Index()
        {
            return View(db.Places.ToList());
        }

        // GET: Places/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            ViewBag.__Data = JsonConvert.SerializeObject(place);
            return View(place);
        }

        // GET: Places/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Location,CenterLatitude,CenterLongitude,LocationPoints")] Place place, string locationWellKnownText)
        {
            ModelState.Remove("Location");
            place.Location = DbGeography.FromText(locationWellKnownText);
            
            if (ModelState.IsValid)
            {
                db.Places.Add(place);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Response.TrySkipIisCustomErrors = true;

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Places/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            ViewBag.__Data = JsonConvert.SerializeObject(place);
            return View(place);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Location,CenterLatitude,CenterLongitude,LocationPoints")] Place model, string locationWellKnownText)
        {
            ModelState.Remove("Location");
            if (ModelState.IsValid)
            {
                var place = db.Places.Find(model.Id);
                place.Name = model.Name;
                if (!string.IsNullOrWhiteSpace(locationWellKnownText))
                {
                    db.GeoPoints.RemoveRange(place.LocationPoints);
                    place.Location = DbGeography.FromText(locationWellKnownText);
                    place.CenterLatitude = model.CenterLatitude;
                    place.CenterLongitude = model.CenterLongitude;
                    model.LocationPoints.ToList().ForEach(p => place.LocationPoints.Add(p));
                }
                //db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            Response.TrySkipIisCustomErrors = true;

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Places/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Place place = db.Places.Find(id);
            db.Places.Remove(place);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
