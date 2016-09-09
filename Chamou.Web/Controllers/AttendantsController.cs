using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chamou.Web.Models.Entities;

namespace Chamou.Web.Controllers
{
    public class AttendantsController : Controller
    {
        private ChamouContext db = new ChamouContext();

        // GET: Attendants
        public ActionResult Index()
        {
            var attendants = db.Attendants.Include(a => a.Place);
            return View(attendants.ToList());
        }

        // GET: Attendants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendant attendant = db.Attendants.Find(id);
            if (attendant == null)
            {
                return HttpNotFound();
            }
            return View(attendant);
        }

        // GET: Attendants/Create
        public ActionResult Create()
        {
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "Name");
            return View();
        }

        // POST: Attendants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PlaceId")] Attendant attendant)
        {
            if (ModelState.IsValid)
            {
                db.Attendants.Add(attendant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceId = new SelectList(db.Places, "Id", "Name", attendant.PlaceId);
            return View(attendant);
        }

        // GET: Attendants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendant attendant = db.Attendants.Find(id);
            if (attendant == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "Name", attendant.PlaceId);
            return View(attendant);
        }

        // POST: Attendants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PlaceId")] Attendant attendant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaceId = new SelectList(db.Places, "Id", "Name", attendant.PlaceId);
            return View(attendant);
        }

        // GET: Attendants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendant attendant = db.Attendants.Find(id);
            if (attendant == null)
            {
                return HttpNotFound();
            }
            return View(attendant);
        }

        // POST: Attendants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendant attendant = db.Attendants.Find(id);
            db.Attendants.Remove(attendant);
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
