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
    public class AdministratorController : Controller
    {
        private EmployeeManagementContext db = new EmployeeManagementContext();
        //to filter DB based on user
        public static string userID;

        //to pass username to the views throughout the life cycle of using the system as a user 
        public static string staticName;

        // GET: Administrator
        public ActionResult Index(String id)
        {
            userID = id;
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                List<Employee> employees = db.Employees.ToList();
                List<Shift> shifts = db.Shifts.ToList();

                var employeeRecord = from e in employees
                                     where e.username == userID
                                     select new Employee
                                     {
                                          id = e.id,
                                          firstName = e.firstName,
                                          lastName = e.lastName,
                                          phoneNo = e.phoneNo,
                                          homeAddress = e.homeAddress,
                                          email = e.email,
                                          dob = e.dob,
                                          jobTitle = e.jobTitle,
                                          privilege = e.privilege,
                                          password = e.password,
                                          username = e.username
                                     };
                
                if (employeeRecord == null)
                {
                    return HttpNotFound();
                }
                
                staticName = id;
                ViewBag.name = staticName;
                ViewBag.id = userID; 
                return View(employeeRecord);
            }
        }
        public ActionResult EditDetails()
        {
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                List<Employee> employees = db.Employees.ToList();

                var employeeRecord = from e in employees
                                     where e.username == userID
                                     select new Employee
                                     {
                                         phoneNo = e.phoneNo,
                                         homeAddress = e.homeAddress,
                                         email = e.email,
                                         username = e.username,
                                         password = e.password
                                     };
                if (employeeRecord == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id = userID;
                ViewBag.name = staticName;
                return View(employeeRecord.First());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails([Bind(Include = "dob,phoneNo,homeAddress,username,password")] Employee employee)
        {
            employee.username = userID;
            int n = int.Parse(employee.phoneNo.ToString());
            employee.phoneNo = n;
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = userID;
            ViewBag.name = staticName;
            return View(employee);
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
