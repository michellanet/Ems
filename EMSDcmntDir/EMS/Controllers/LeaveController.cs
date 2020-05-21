using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMS.DAL;
using EMS.Models;

namespace EMS.Controllers
{
    public class LeaveController : Controller
    {
        private EmployeeManagementContext db = new EmployeeManagementContext();

        // GET: Leave
        public ActionResult Index()
        {
            var leaves = db.Leaves.Include(l => l.Employee);
            ViewBag.name = AdministratorController.staticName;
            return View(leaves.ToList());
        }

        // GET: Leave/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = AdministratorController.staticName;
            return View(leave);
        }

        // GET: Leave/Create
        public ActionResult Create()
        {
            ViewBag.employeeId = new SelectList(db.Employees, "id", "firstName");
            ViewBag.name = AdministratorController.staticName;
            return View();
        }

        // POST: Leave/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,startDate,endDate,status,employeeId")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Leaves.Add(leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeId = new SelectList(db.Employees, "id", "firstName", leave.employeeId);
            ViewBag.name = AdministratorController.staticName;
            return View(leave);
        }

        // GET: Leave/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "firstName", leave.employeeId);
            ViewBag.name = AdministratorController.staticName;
            return View(leave);
        }

        // POST: Leave/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,startDate,endDate,status,employeeId")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "firstName", leave.employeeId);
            ViewBag.name = AdministratorController.staticName;
            return View(leave);
        }

        // GET: Leave/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = AdministratorController.staticName;
            return View(leave);
        }

        // POST: Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leave leave = db.Leaves.Find(id);
            db.Leaves.Remove(leave);
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
