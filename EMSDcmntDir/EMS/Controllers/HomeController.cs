using EMS.Models;
using EMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EMS.Controllers
{
    public class HomeController : Controller
    {
        //to filter DB based on user
        public static int userID;
        string msg = "";
        public ActionResult Index()
        {
            ViewBag.notice = msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "username,password")] Employee empInput)
        {
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                // login setup
                List<Employee> employees = db.Employees.ToList();
                if (empInput.username == null || empInput.password == null)
                {
                    msg = "Username and Password is required!";
                    ViewBag.notice = msg;
                    return View();
                }

                Employee emp = db.Employees
                    .Where(e => e.username == empInput.username)
                    .FirstOrDefault();
                //Redirect to login page if user is not found in database
                if (emp == null)
                {
                    msg = "Employee not found, input correct username";
                    ViewBag.notice = msg;
                    return View(emp);
                }

                // Redirect to employee system if user privilege is user
                if (emp.username == empInput.username && emp.password == empInput.password && emp.privilege == Employee.Privilege.User)
                {
                    msg = "";
                    userID = emp.id;
                    return RedirectToAction("Index", "Employee", new { id = emp.username });
                }

                // Redirect to administrator system if user privilege is admin
                else if (emp.username == empInput.username && emp.password == empInput.password && emp.privilege == Employee.Privilege.Admin)
                {
                    msg = "";
                    userID = emp.id;
                    return RedirectToAction("Index", "Administrator", new { id = emp.username });
                }
                else
                {
                    msg = "Authentification Failed. Make sure your UserName and Password is correct";
                    ViewBag.notice = msg;
                    return View(emp);
                }
                ViewBag.notice = msg;
                return View(emp);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Employee Management System";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Address";

            return View();
        }
    }
}