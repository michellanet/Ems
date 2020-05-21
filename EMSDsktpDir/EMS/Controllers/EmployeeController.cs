using EMS.Models;
using EMS.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EMS.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeManagementContext db = new EmployeeManagementContext();
        //to filter DB based on user
        private static int userID;

        //to pass username to the views throughout the life cycle of using the system as a user 
        private static string staticName;
        // GET: Employee
        public ActionResult Index(String id)
        {
            staticName = id;
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                List<Employee> employees = db.Employees.ToList();
                List<Shift> shifts = db.Shifts.ToList();
                Employee employee = db.Employees.Find(HomeController.userID);
                var shift = db.Shifts.Find(HomeController.userID);

                var employeeRecord = from e in employees
                                     where e.username == staticName
                                     join s in shifts on e.id equals s.employeeId
                                     into table1                                     
                                     from s in table1.ToList()
                                     select new Shift
                                     {
                                         Employee = employee,
                                         date = s.date,
                                         id = s.id,
                                         attendance = s.attendance,
                                         employeeId = s.employeeId
                                     };
                userID = employee.id;
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

        // GET: Schedule
        public ActionResult Schedule()
        {
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                List<Shift> shifts = db.Shifts.ToList();

                //get all employee shifts
                var employeeShift = from s in shifts
                                     where s.Employee.id == userID
                                     select new Shift
                                     {
                                         id = s.id,
                                         Employee = s.Employee,
                                         employeeId = s.employeeId,
                                         date = s.date,
                                         attendance = s.attendance
                                     };

                //get all employee attended shifts
                var pList = from s in shifts
                                 where s.Employee.id == userID
                                 && s.attendance == Shift.Attendance.Present
                                 select new Shift
                                 {
                                     attendance = s.attendance
                                 };

                //get all employee missed shifts
                var aList = from s in shifts
                               where s.Employee.id == userID
                               && s.attendance == Shift.Attendance.Absent
                               select new Shift
                               {
                                   attendance = s.attendance
                               };

                //calculate the ratio of work attended to work absence
                ViewBag.total = employeeShift.Count();
                ViewBag.pending = employeeShift.Count() - (pList.Count() + aList.Count());
                ViewBag.present = pList.Count();
                ViewBag.absent = aList.Count();
                if (employeeShift.Count() != 0 && pList.Count() != 0)
                {
                    ViewBag.totalPrcnt = (pList.Count()*100) / (aList.Count() + pList.Count()) + "%";
                    ViewBag.bar = ViewBag.totalPrcnt;
                }
                else {
                    ViewBag.totalPrcnt = "Not evaluable at this time";
                    ViewBag.bar = "0%";
                }
                ViewBag.id = userID;
                ViewBag.name = staticName;
                return View(employeeShift);
            }
        }

        // GET: Request
        public ActionResult Request()
        {
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                List<Employee> employees = db.Employees.ToList();
                List<DayOff> dayOffs = db.DayOffs.ToList();
                List<Leave> leaves = db.Leaves.ToList();

                var employeeDayOff = from e in employees
                                     where e.username == staticName
                                     join d in dayOffs on e.id equals d.employeeId
                                     into table1
                                     from d in table1.ToList()
                                     select new DayOff
                                     {
                                         id = d.id,
                                         Employee = e,
                                         date = d.date,
                                         employeeId = d.employeeId,
                                         status = d.status
                                     };

                var employeeLeave = from e in employees
                                     where e.username == staticName
                                     join l in leaves on e.id equals l.employeeId
                                     into table1
                                     from l in table1.ToList()
                                     select new Leave
                                     {
                                         Employee = e,
                                         startDate = l.startDate,
                                         endDate = l.endDate,
                                         status = l.status
                                     };
                ViewBag.id = userID;
                ViewBag.name = staticName;
                ViewBag.employeeLeave = employeeLeave;
                return View(employeeDayOff);
            }
        }

        // GET: Day Off Request
        public ActionResult DayOff()
        {
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                List<Employee> employees = db.Employees.ToList();
                List<Shift> shifts = db.Shifts.ToList();

                var proposedDayOff = from e in employees
                                     where e.username == staticName
                                     join s in shifts on e.id equals s.employeeId
                                     into table1
                                     from s in table1.ToList()
                                     select new DayOff
                                     {
                                         status = Models.DayOff.DayOffStatus.Pending,
                                         Employee = e,
                                         date = s.date,
                                         employeeId = s.employeeId
                                     };
                ViewBag.id = userID;
                ViewBag.name = staticName;
                return View(proposedDayOff);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DayOff([Bind(Include = "date")] DayOff dayOff)
        {
            List<DayOff> dayOffs = db.DayOffs.ToList();
            dayOff.date = dayOffs[dayOffs.Count() - 1].date;
            dayOff.employeeId = userID;
            dayOff.status = Models.DayOff.DayOffStatus.Pending;

            if (ModelState.IsValid)
            {
                db.DayOffs.Add(dayOff);
                db.SaveChanges();
                return RedirectToAction("Request");
            }
            ViewBag.name = staticName;
            return View(dayOff);
        }

        // GET: Vacation Request

        public ActionResult Vacation()
        {
            ViewBag.id = userID;
            ViewBag.name = staticName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Vacation([Bind(Include = "startDate,endDate")] Leave leave)
        {
            // use last vacation id to set new id
            List<Employee> employees = db.Employees.ToList();
            List<Leave> leaves = db.Leaves.ToList();
            var employeeLeave = from e in employees
                                where e.username == staticName
                                join l in leaves on e.id equals l.employeeId
                                into table1
                                from l in table1.ToList()
                                select new Leave
                                {
                                    Employee = e,
                                    startDate = l.startDate,
                                    endDate = l.endDate,
                                    status = l.status,
                                    id = l.id,
                                    employeeId = l.employeeId
                                };
            leave.status = Leave.LeaveStatus.Pending;
            leave.employeeId = userID;

            if (ModelState.IsValid)
            {
                db.Leaves.Add(leave);
                db.SaveChanges();
                return RedirectToAction("Request");
            }
            ViewBag.name = staticName;
            return View(leave);
        }

        public ActionResult EditDetails()
        {
            using (EmployeeManagementContext db = new EmployeeManagementContext())
            {
                if (userID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Employee employee = db.Employees.Find(userID);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id = userID;
                ViewBag.name = staticName;
                return View(employee);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails([Bind(Include = "id,firstName,lastName,phoneNo,homeAddress,email,dob,jobTitle,privilege,password,username")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Employee", new { id = staticName });
            }
            ViewBag.name = staticName;
            return View(employee);
        }
    }
}