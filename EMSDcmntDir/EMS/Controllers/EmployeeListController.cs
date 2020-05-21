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
    public class EmployeeListController : Controller
    {
        private EmployeeManagementContext db = new EmployeeManagementContext();

        // GET: EmployeeList
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.JobSortParm = sortOrder == "Job" ? "job_desc" : "Job";
            var employees = from e in db.Employees
                           select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.lastName.Contains(searchString)
                                       || e.firstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.lastName);
                    break;
                case "Job":
                    employees = employees.OrderBy(e => e.jobTitle);
                    break;
                case "job_desc":
                    employees = employees.OrderByDescending(e => e.jobTitle);
                    break;
                default:
                    employees = employees.OrderBy(s => s.lastName);
                    break;
            }
            ViewBag.name = AdministratorController.staticName;
            return View(db.Employees.ToList());
        }

        // GET: EmployeeList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = AdministratorController.staticName;
            return View(employee);
        }

        // GET: EmployeeList/Create
        public ActionResult Create()
        {
            ViewBag.name = AdministratorController.staticName;
            return View();
        }

        // POST: EmployeeList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,firstName,lastName,phoneNo,homeAddress,email,dob,jobTitle,privilege,password,username")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.name = AdministratorController.staticName;
            return View(employee);
        }

        // GET: EmployeeList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = AdministratorController.staticName;
            return View(employee);
        }

        // POST: EmployeeList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,firstName,lastName,phoneNo,homeAddress,email,dob,jobTitle,privilege,password,username")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.name = AdministratorController.staticName;
            return View(employee);
        }

        // GET: EmployeeList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = AdministratorController.staticName;
            return View(employee);
        }

        // POST: EmployeeList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
