using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔐 Helper: Check Admin
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // 📊 INDEX (Role-based)
        public IActionResult Index(string searchString)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            IQueryable<Employee> employees = _context.Employees;

            // 🔍 SEARCH FILTER
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e =>
                    e.Name.Contains(searchString) ||
                    e.Department.Contains(searchString));
            }

            // 👨‍💼 ADMIN → all
            if (role == "Admin")
            {
                return View(employees.ToList());
            }

            // 👨‍💻 EMPLOYEE → own data
            var user = _context.Users
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id.ToString() == userId);

            if (user?.Employee == null)
            {
                return NotFound();
            }

            var list = new List<Employee> { user.Employee };

            // Apply search on own data
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(e =>
                    e.Name.Contains(searchString) ||
                    e.Department.Contains(searchString)).ToList();
            }

            return View(list);
        }

        // ➕ CREATE
        public IActionResult Create()
        {
            if (!IsAdmin())
                return Unauthorized();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if (!IsAdmin())
                return Unauthorized();

            _context.Employees.Add(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ✏️ EDIT
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return Unauthorized();

            var emp = _context.Employees.Find(id);
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            if (!IsAdmin())
                return Unauthorized();

            _context.Employees.Update(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ❌ DELETE
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return Unauthorized();

            var emp = _context.Employees.Find(id);
            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return Unauthorized();

            var emp = _context.Employees.Find(id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}