using MiniL29._06._2026.Data;
using MiniL29._06._2026.Entities;
using MiniL29._06._2026.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            "5.  İşçini redaktə et",
            "6.  İşçini sil",
            "7.  Şöbəni sil",
            "8.  İşçi axtar (ad üzrə)",
            "9.  Şöbə üzrə statistika",
            "10. Maaş aralığına görə işçilər",
            "0.  Çıxış",
        };

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine(" === SYSTEM MENU === \n");
                foreach (var item in MenuItems)
                {
                    Console.WriteLine($"{item}");
                }
                Console.Write("\nSeçiminiz: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": Console.Clear(); AddDepartment(); break;
                        case "2": Console.Clear(); AddEmployee(); break;
                        case "3": Console.Clear(); ShowAllDepartments(); break;
                        case "4": Console.Clear(); ShowAllEmployees(); break;
                        case "5": Console.Clear(); UpdateEmployee(); break;
                        case "6": Console.Clear(); DeleteEmployee(); break;
                        case "7": Console.Clear(); DeleteDepartment(); break;
                        case "8": Console.Clear(); SearchEmployeeByName(); break;
                        case "9": Console.Clear(); ShowDepartmentStatistics(); break;
                        case "10": Console.Clear(); ShowEmployeesBySalaryRange(); break;
                        case "0":
                            Console.Clear();
                            Console.WriteLine("\n  Sistem bağlanır...");
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Yanlış seçim, Yenidən cəhd edin!");
                            break;
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    Console.WriteLine("\n❌ Verilənlər bazasına yazarkən xəta baş verdi!");
                    Console.WriteLine($"   Səbəb: {dbEx.InnerException?.Message ?? dbEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n❌ Gözlənilməz xəta baş verdi!");
                    Console.WriteLine($"   Mesaj: {ex.Message}");
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
                
                if (deptRepo.GetAll().Any(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Bu adda şöbə artıq mövcuddur!");
                    return;
                }

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
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary) || salary <= 0)
                {
                    Console.WriteLine("Yanlış maaş formatı! Maaş 0-dan böyük olmalıdır.");
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

        private void UpdateEmployee()
        {
            Console.WriteLine("=== İŞÇİNİ REDAKTƏ ET ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Employee> empRepo = new Repository<Employee>(context);

                var employees = empRepo.GetAll();
                if (!employees.Any())
                {
                    Console.WriteLine("Heç bir işçi tapılmadı.");
                    return;
                }

                foreach (var e in employees)
                {
                    Console.WriteLine($"ID: {e.Id} | {e.Name} {e.Surname} | Maaş: {e.Salary} AZN");
                }

                Console.Write("\nRedaktə etmək istədiyiniz işçinin ID-si: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Yanlış ID!");
                    return;
                }

                // isTracking = true, çünki Update üçün entity-ni izləməliyik
                var employee = empRepo.GetById(id, isTracking: true);
                if (employee == null)
                {
                    Console.WriteLine("Bu ID-də işçi tapılmadı!");
                    return;
                }

                Console.Write($"Yeni ad (köhnə: {employee.Name}, boş saxlasan dəyişməz): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                    employee.Name = newName;

                Console.Write($"Yeni soyad (köhnə: {employee.Surname}, boş saxlasan dəyişməz): ");
                string newSurname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newSurname))
                    employee.Surname = newSurname;

                Console.Write($"Yeni maaş (köhnə: {employee.Salary}, boş saxlasan dəyişməz): ");
                string salaryInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(salaryInput))
                {
                    if (decimal.TryParse(salaryInput, out decimal newSalary) && newSalary > 0)
                        employee.Salary = newSalary;
                    else
                        Console.WriteLine("Yanlış maaş, dəyişiklik tətbiq olunmadı.");
                }

                empRepo.Update(employee);
                empRepo.SaveChanges();

                Console.WriteLine("\nİşçi məlumatları uğurla yeniləndi!");
            }
        }

        private void DeleteEmployee()
        {
            Console.WriteLine("=== İŞÇİNİ SİL ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Employee> empRepo = new Repository<Employee>(context);

                var employees = empRepo.GetAll();
                if (!employees.Any())
                {
                    Console.WriteLine("Heç bir işçi tapılmadı.");
                    return;
                }

                foreach (var e in employees)
                {
                    Console.WriteLine($"ID: {e.Id} | {e.Name} {e.Surname}");
                }

                Console.Write("\nSilmək istədiyiniz işçinin ID-si: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Yanlış ID!");
                    return;
                }

                var employee = empRepo.GetById(id, isTracking: true);
                if (employee == null)
                {
                    Console.WriteLine("Bu ID-də işçi tapılmadı!");
                    return;
                }

                Console.Write($"'{employee.Name} {employee.Surname}' silinsin? (h/x): ");
                if (Console.ReadLine()?.ToLower() != "h")
                {
                    Console.WriteLine("Ləğv edildi.");
                    return;
                }

                empRepo.Delete(employee);
                empRepo.SaveChanges();

                Console.WriteLine("\nİşçi uğurla silindi!");
            }
        }

        private void DeleteDepartment()
        {
            Console.WriteLine("=== ŞÖBƏNİ SİL ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Department> deptRepo = new Repository<Department>(context);
                Repository<Employee> empRepo = new Repository<Employee>(context);

                var departments = deptRepo.GetAll();
                if (!departments.Any())
                {
                    Console.WriteLine("Heç bir şöbə tapılmadı.");
                    return;
                }

                foreach (var d in departments)
                {
                    int empCount = empRepo.GetAll().Count(e => e.DepartmentId == d.Id);
                    Console.WriteLine($"ID: {d.Id} | Şöbə: {d.Name} | İşçi sayı: {empCount}");
                }

                Console.Write("\nSilmək istədiyiniz şöbənin ID-si: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Yanlış ID!");
                    return;
                }

                var department = deptRepo.GetById(id, isTracking: true);
                if (department == null)
                {
                    Console.WriteLine("Bu ID-də şöbə tapılmadı!");
                    return;
                }

                int relatedEmployees = empRepo.GetAll().Count(e => e.DepartmentId == id);
                if (relatedEmployees > 0)
                {
                    Console.WriteLine($"\nDİQQƏT: Bu şöbədə {relatedEmployees} işçi var. Şöbə silinərsə, bütün işçilər də silinəcək!");
                }

                Console.Write($"'{department.Name}' şöbəsi silinsin? (h/x): ");
                if (Console.ReadLine()?.ToLower() != "h")
                {
                    Console.WriteLine("Ləğv edildi.");
                    return;
                }

                deptRepo.Delete(department);
                deptRepo.SaveChanges();

                Console.WriteLine("\nŞöbə (və əlaqəli işçilər) uğurla silindi!");
            }
        }

        private void SearchEmployeeByName()
        {
            Console.WriteLine("=== İŞÇİ AXTAR ===\n");
            Console.Write("Axtarılacaq ad/soyad: ");
            string keyword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Axtarış sözü boş ola bilməz!");
                return;
            }

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Employee> empRepo = new Repository<Employee>(context);
                Repository<Department> deptRepo = new Repository<Department>(context);

                var results = empRepo.GetAll()
                    .Where(e =>
                        (e.Name != null && e.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (e.Surname != null && e.Surname.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                if (!results.Any())
                {
                    Console.WriteLine("Heç bir nəticə tapılmadı.");
                    return;
                }

                var departments = deptRepo.GetAll();
                Console.WriteLine($"\n{results.Count} nəticə tapıldı:\n");

                foreach (var e in results)
                {
                    var deptName = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name ?? "Yoxdur";
                    Console.WriteLine($"ID: {e.Id} | {e.Name} {e.Surname} | Maaş: {e.Salary} AZN | Şöbə: {deptName}");
                }
            }
        }

        private void ShowDepartmentStatistics()
        {
            Console.WriteLine("=== ŞÖBƏ ÜZRƏ STATİSTİKA ===\n");

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Department> deptRepo = new Repository<Department>(context);
                Repository<Employee> empRepo = new Repository<Employee>(context);

                var departments = deptRepo.GetAll();
                var employees = empRepo.GetAll();

                if (!departments.Any())
                {
                    Console.WriteLine("Heç bir şöbə tapılmadı.");
                    return;
                }

                foreach (var d in departments)
                {
                    var deptEmployees = employees.Where(e => e.DepartmentId == d.Id).ToList();

                    Console.WriteLine($"\n📁 Şöbə: {d.Name}");
                    Console.WriteLine($"   İşçi sayı: {deptEmployees.Count}");

                    if (deptEmployees.Any())
                    {
                        Console.WriteLine($"   Ortalama maaş: {deptEmployees.Average(e => e.Salary):F2} AZN");
                        Console.WriteLine($"   Maksimum maaş: {deptEmployees.Max(e => e.Salary):F2} AZN");
                        Console.WriteLine($"   Minimum maaş: {deptEmployees.Min(e => e.Salary):F2} AZN");
                        Console.WriteLine($"   Ümumi maaş fondu: {deptEmployees.Sum(e => e.Salary):F2} AZN");
                    }
                    else
                    {
                        Console.WriteLine("   Bu şöbədə işçi yoxdur.");
                    }
                }

                if (employees.Any())
                {
                    var topDept = departments
                        .Select(d => new { d.Name, Count = employees.Count(e => e.DepartmentId == d.Id) })
                        .OrderByDescending(x => x.Count)
                        .First();

                    Console.WriteLine($"\n🏆 Ən çox işçisi olan şöbə: {topDept.Name} ({topDept.Count} işçi)");
                    Console.WriteLine($"💰 Bütün şirkət üzrə ümumi maaş fondu: {employees.Sum(e => e.Salary):F2} AZN");
                }
            }
        }

        private void ShowEmployeesBySalaryRange()
        {
            Console.WriteLine("=== MAAŞ ARALIĞINA GÖRƏ İŞÇİL�ər ===\n");

            Console.Write("Minimum maaş: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal min))
            {
                Console.WriteLine("Yanlış format!");
                return;
            }

            Console.Write("Maksimum maaş: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal max))
            {
                Console.WriteLine("Yanlış format!");
                return;
            }

            if (min > max)
            {
                Console.WriteLine("Minimum maksimum dan böyük ola bilməz!");
                return;
            }

            using (AllDbSystem context = new AllDbSystem())
            {
                Repository<Employee> empRepo = new Repository<Employee>(context);
                Repository<Department> deptRepo = new Repository<Department>(context);

                var results = empRepo.GetAll()
                    .Where(e => e.Salary >= min && e.Salary <= max)
                    .OrderByDescending(e => e.Salary)
                    .ToList();

                if (!results.Any())
                {
                    Console.WriteLine("\nBu aralıqda işçi tapılmadı.");
                    return;
                }

                var departments = deptRepo.GetAll();
                Console.WriteLine($"\n{results.Count} işçi tapıldı:\n");

                foreach (var e in results)
                {
                    var deptName = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name ?? "Yoxdur";
                    Console.WriteLine($"ID: {e.Id} | {e.Name} {e.Surname} | Maaş: {e.Salary} AZN | Şöbə: {deptName}");
                }
            }
        }
    }
}