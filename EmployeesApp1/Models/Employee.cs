using System;
using System.Collections.Generic;

namespace EmployeesApp1.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public Role Role { get; set; }

    public int? ManagerId { get; set; }

    public virtual Employee? Manager { get; set; }

    public List<Employee>? Managers { get; set; }
}
