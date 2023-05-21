using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesApp1.Models;

namespace EmployeesApp1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly CompanyContext _context;

        public EmployeesController(CompanyContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {

            return _context.Employees != null ? 
                        View(await _context.Employees.Include(e => e.Manager).ToListAsync()) :
                        Problem("Entity set 'CompanyContext.Employees'  is null.");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Include(e => e.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {

            var managers = _context.Employees.Where(e => e.Role == 0).ToList();
            var viewModel = new Employee
            {
                Managers = managers,
                Role = Role.Employee
            };
            return View(viewModel);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Role,ManagerId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Managers = _context.Employees.Where(e => e.Role == 0).ToList();
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Role,ManagerId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = await _context.Employees.FindAsync(id);
                    if (existingEmployee == null)
                    {
                        // Handle employee not found
                        return NotFound();
                    }

                    var previousRole = existingEmployee.Role;

                    // Update the existing employee with the new values
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Address = employee.Address;
                    existingEmployee.Role = employee.Role;
                    existingEmployee.ManagerId = employee.ManagerId;
                    //_context.Update(employee);
                    await _context.SaveChangesAsync();

                    if (previousRole == Role.Manager && employee.Role == Role.Employee)
                    {
                        var employeesToUpdate = await _context.Employees.Where(e => e.ManagerId == employee.Id).ToListAsync();
                        foreach (var employeeToUpdate in employeesToUpdate)
                        {
                            employeeToUpdate.ManagerId = null;
                        }
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Include(e => e.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'CompanyContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                if (employee.Role == Role.Manager)
                {
                    var employeesToUpdate = await _context.Employees.Where(e => e.ManagerId == employee.Id).ToListAsync();
                    foreach (var employeeToUpdate in employeesToUpdate)
                    {
                        employeeToUpdate.ManagerId = null;
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
