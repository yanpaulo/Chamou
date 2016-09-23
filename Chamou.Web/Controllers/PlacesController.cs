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
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using AutoMapper;
using Chamou.Web.Models.DTOs;

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
            ViewBag.__Data = JsonConvert.SerializeObject(Mapper.Map<PlaceDTO>(place));
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

            if (ModelState.IsValid)
            {
                place.Location = FixedGeomFromText(locationWellKnownText);

                db.Places.Add(place);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(ms => ms.Value.Errors).Select(ms => ms.ErrorMessage));
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
            ViewBag.__Data = JsonConvert.SerializeObject(Mapper.Map<PlaceDTO>(place));
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
                    place.CenterLatitude = model.CenterLatitude;
                    place.CenterLongitude = model.CenterLongitude;
                    model.LocationPoints.ToList().ForEach(p => place.LocationPoints.Add(p));
                    place.Location = FixedGeomFromText(locationWellKnownText);
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
            db.Attendants.RemoveRange(place.Attendants);
            db.GeoPoints.RemoveRange(place.LocationPoints);
            db.Places.Remove(place);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private DbGeography FixedGeomFromText(string locationWellKnownText)
        {
            //First, get the area defined by the well-known text using left-hand rule
            var sqlGeography =
            SqlGeography.STGeomFromText(new SqlChars(locationWellKnownText), DbGeography.DefaultCoordinateSystemId)
            .MakeValid();

            //Now get the inversion of the above area
            var invertedSqlGeography = sqlGeography.ReorientObject();

            //Whichever of these is smaller is the enclosed polygon, so we use that one.
            if (sqlGeography.STArea() > invertedSqlGeography.STArea())
            {
                sqlGeography = invertedSqlGeography;
            }

            //AddedLine
            return DbSpatialServices.Default.GeographyFromProviderValue(sqlGeography);
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
