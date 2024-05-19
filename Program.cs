using VClassLibrary;

namespace LogLoader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || args.Length < 2)
                {
                    Console.WriteLine("Hiba!");
                    Console.WriteLine("Helyes használat: LogLoader.exe -InputFile systemlog.log");
                    return;
                }

                if (!args[0].Equals("-InputFile"))
                {
                    Console.WriteLine("Hiba!");
                    Console.WriteLine("Helyes használat: LogLoader.exe -InputFile systemlog.log");
                    return;
                }

                StreamReader sr = new StreamReader(args[1]);

                if (sr == null)
                {
                    Console.WriteLine("Hiba!");
                    Console.WriteLine("Ez a fájl nem található!");
                    return;
                }

                int sorok = 0;
                int beolvasotts = 0;

                string sor = "";

                sor = sr.ReadLine();

                while ((sor = sr.ReadLine()) != null)
                {
                    sorok++;

                    using (Sql sql = new Sql())
                    {
                        LogEntry bejegyzes = new LogEntry();
                        string[] szetvagva = sor.Split(";");

                        bejegyzes.Id = int.Parse(szetvagva[0]);
                        bejegyzes.CorrelationId = szetvagva[1];
                        bejegyzes.DateUtc = DateTime.Parse(szetvagva[2]);
                        bejegyzes.Thread = int.Parse(szetvagva[3]);

                        bejegyzes.Level = szetvagva[4];
                        bejegyzes.Logger = szetvagva[5];
                        bejegyzes.Message = szetvagva[6];
                        bejegyzes.Exception = szetvagva[7];

                        sql.logentries.Add(bejegyzes);

                        sql.SaveChanges();

                        beolvasotts++;
                    }
                }

                Console.WriteLine("Fájl neve: " + args[1]);
                Console.WriteLine("Fájlban lévp sorok száma: " + sorok);
                Console.WriteLine("Beolvasott sorok száma: " +  beolvasotts);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba!");
                Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}