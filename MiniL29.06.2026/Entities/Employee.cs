using MiniL29._06._2026.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniL29._06._2026.Entities
{
    internal class Employee:BaseEntity
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
