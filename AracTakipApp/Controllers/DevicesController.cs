using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AracTakipApp.Models;
using PagedList;

namespace AracTakipApp.Controllers
{
    public class DevicesController : Controller
    {
        private AracTakipDBOEntities db = new AracTakipDBOEntities();

        [Route("AracListele")]
        public ActionResult Index(int sayfa = 1)
        {
            var device = db.Device.Include(d => d.Brand).Include(d => d.Unit);
            return View(device.OrderBy(x => x.LicenseNo).ToPagedList(sayfa, 2));
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Device.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }


        [Route("AracKayit")]
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brand, "BrandID", "BrandName");
            ViewBag.UnitID = new SelectList(db.Unit, "UnitID", "UnitName");
            return View();
        }


        [Route("AracKayit/{device?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeviceId,LicenseNo,UnitID,BrandID,Model,Kind,Type,DeviceSituation,ModelYear")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Device.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brand, "BrandID", "BrandName", device.BrandID);
            ViewBag.UnitID = new SelectList(db.Unit, "UnitID", "UnitName", device.UnitID);
            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Device.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brand, "BrandID", "BrandName", device.BrandID);
            ViewBag.UnitID = new SelectList(db.Unit, "UnitID", "UnitName", device.UnitID);
            return View(device);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeviceId,LicenseNo,UnitID,BrandID,Model,Kind,Type,DeviceSituation,ModelYear")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brand, "BrandID", "BrandName", device.BrandID);
            ViewBag.UnitID = new SelectList(db.Unit, "UnitID", "UnitName", device.UnitID);
            return View(device);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Device.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = db.Device.Find(id);
            db.Device.Remove(device);
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
