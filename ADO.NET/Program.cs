using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace ADO.NET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello ADO");
            const int PADDING = 33;
            const string CONNECTION_STRING =
                "Data Source=(localdb)\\MSSQLLocalDB;" +
                "Initial Catalog=Movies_311;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate = False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False";
            Console.WriteLine(CONNECTION_STRING);

            //1) Создаем подключение к Базе:
            SqlConnection connection = new SqlConnection(CONNECTION_STRING);

            //2) Создаем команду, которую хотим выполнить на Сервере:
            string cmd = "SELECT * FROM Directors";
            SqlCommand command = new SqlCommand(cmd, connection);

            //3) Получаем результаты запроса с Сервера:
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            //4) Обрабатываем результаты запроса:
            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
                Console.WriteLine();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t");
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i].ToString().PadRight(PADDING));
                    }
                    Console.WriteLine();
                }
            }

            //5) Закрываем поток и соединение с Сервером:
            reader.Close();
            connection.Close();
        }
    }
}
/*

*/