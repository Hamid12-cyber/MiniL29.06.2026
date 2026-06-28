using MiniL29._06._2026.Data;
using System.Text;

namespace MiniL29._06._2026
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
                new System.Globalization.CultureInfo("az-Latn-AZ");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture =
                new System.Globalization.CultureInfo("az-Latn-AZ");
            using (AllDbSystem db = new AllDbSystem())
            {
                db.Database.EnsureCreated();
            }
            StartMenu menu = new StartMenu();
            menu.Run();
        }
    }
}
