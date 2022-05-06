using System;
using System.Data.SqlClient;
using System.Threading;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Starting ...");
            try
            {
                using (var connection = new SqlConnection("Server=kerbtestvm.kerbtest.com;Database=kubetestdb;Trusted_Connection=True;"))
                {
                    connection.Open();
                    Console.WriteLine("Connection opened ...");

                    while (true)
                    {
                        Console.WriteLine($"{DateTime.Now} Querying for data ...");
                        var command = new SqlCommand("SELECT TOP 10 * FROM dbo.mydata", connection);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"# {reader[0]} {reader[1]}");
                            }
                        }

                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}