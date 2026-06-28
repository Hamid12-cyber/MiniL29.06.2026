using MiniL29._06._2026.Data;
using MiniL29._06._2026.Entities;
using MiniL29._06._2026.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MiniL29._06._2026
{
    internal class StartMenu
    {
        private static readonly string[] MenuItems =
        {
            "1.  Yeni şöbə əlavə et",
            "2.  Yeni işçi əlavə et",
            "3.  Bütün şöbələri göstər",
            "4.  Bütün işçiləri göstər",
            "0.  Çıxış",

        };
        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear(); // Menyunun təmiz ekranda gəlməsi üçün bura mütləq lazımdır!
                Console.WriteLine(" === SYSTEM MENU === \n");
                foreach (var item in MenuItems)
                {
                    Console.WriteLine($"{item}");
                }
                Console.Write("\nSeçiminiz: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        AddDepartment(); 
                        break;

                    case "2":
                        Console.Clear();
                        AddEmployee();  
                        break;

                    case "3":
                        Console.Clear();
                        ShowAllDepartments(); 
                        break;

                    case "4":
                        Console.Clear();
                        ShowAllEmployees();   
                        break;

                    case "0":
                        Console.Clear();
                        Console.WriteLine("\n  Sistem bağlanır...");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim, Yenidən cəhd edin!");
                        break;

                }
                if (isRunning && choice != "0")
                {
                    Console.Write("\n  [ Enter - menyuya qayıt ]");
                    Console.ReadLine();
                }
            }
        }
        public void AddDepartment() 
        {
            Console.WriteLine("=== YENİ ŞÖBƏ ƏLAVƏ ET ===\n");
            Console.Write("Şöbə adı: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Şöbə adı boş ola bilməz!");
                return;
            }

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Department> deptRepo = new Repository<Department>(context);

                Department dept = new Department { Name = name };
                deptRepo.Add(dept);
                deptRepo.SaveChanges(); 
            }

            Console.WriteLine("\nŞöbə uğurla əlavə edildi!");

        }
        private void AddEmployee()
        {
            Console.WriteLine("=== YENİ İŞÇİ ƏLAVƏ ET ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Department> deptRepo = new Repository<Department>(context);
                Repository<Employee> empRepo = new Repository<Employee>(context);

                var departments = deptRepo.GetAll();
                if (!departments.Any())
                {
                    Console.WriteLine("Xəta: Əvvəlcə şöbə yaratmalısınız!");
                    return;
                }

                Console.Write("İşçinin adı: ");
                string name = Console.ReadLine();

                Console.Write("İşçinin soyadı: ");
                string surname = Console.ReadLine();

                Console.Write("Maaşı: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                {
                    Console.WriteLine("Yanlış maaş formatı!");
                    return;
                }

                Console.WriteLine("\nMövcud Şöbələr:");
                foreach (var d in departments)
                {
                    Console.WriteLine($"ID: {d.Id} | Şöbə: {d.Name}");
                }

                Console.Write("\nŞöbə ID seçin: ");
                if (!int.TryParse(Console.ReadLine(), out int deptId) || !departments.Any(d => d.Id == deptId))
                {
                    Console.WriteLine("Yanlış Şöbə ID-si!");
                    return;
                }

                Employee emp = new Employee
                {
                    Name = name,
                    Surname = surname,
                    Salary = salary,
                    DepartmentId = deptId
                };

                empRepo.Add(emp);
                empRepo.SaveChanges();
            }

            Console.WriteLine("\nİşçi uğurla əlavə edildi!");
        }

        private void ShowAllDepartments()
        {
            Console.WriteLine("=== BÜTÜN ŞÖBƏLƏR ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Department> deptRepo = new Repository<Department>(context);
                var departments = deptRepo.GetAll();

                if (!departments.Any())
                {
                    Console.WriteLine("Heç bir şöbə tapılmadı.");
                    return;
                }

                foreach (var d in departments)
                {
                    Console.WriteLine($"ID: {d.Id} | Şöbə: {d.Name}");
                }
            }
        }

        private void ShowAllEmployees()
        {
            Console.WriteLine("=== BÜTÜN İŞÇİLƏR ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Employee> empRepo = new Repository<Employee>(context);
                Repository<Department> deptRepo = new Repository<Department>(context);

                var employees = empRepo.GetAll();
                var departments = deptRepo.GetAll(); 

                if (!employees.Any())
                {
                    Console.WriteLine("Heç bir işçi tapılmadı.");
                    return;
                }

                foreach (var e in employees)
                {
                    var deptName = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name ?? "Yoxdur";
                    Console.WriteLine($"ID: {e.Id} | {e.Name} {e.Surname} | Maaş: {e.Salary} AZN | Şöbə: {deptName}");
                }
            }
        }
    }
}


