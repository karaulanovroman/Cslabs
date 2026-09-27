using System.Reflection.Metadata;

namespace sem_1_lab_02_server_config
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static void is_server_ready(int users,int mem,bool pass,bool publ)
            {
                bool is_empty = false;
                bool no_mem = false;
                bool low_mem = false;
                bool is_publ_pass = false;
                if (users == 0) is_empty = true;
                if ((users * 2) + 1 >= mem) no_mem = true; // на одного пользователя 2ГБ и 1ГБ для самого сервера
                if (mem < 1) no_mem = true;
                if ((users * 4) + 1 >= 0.75 * mem) low_mem = true;
                if (pass == true && publ == true) is_publ_pass = true;
                Console.WriteLine("Результат проверки конфигурации сервера:");
                if (is_empty || no_mem)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Выявлены ошибки, запуск невозможен");
                    if (is_empty) Console.WriteLine("Сервер пуст, убедитесь что на нем есть пользователи");
                    else Console.WriteLine("Серверу не хватает оперативной памяти для запуска");
                }
                else if (is_publ_pass || low_mem)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Запуск возможен, но есть проблемы");
                    if (low_mem) Console.WriteLine("Сервер тратит много оперативной памяти, возможны ошибки");
                    else Console.WriteLine("Публичный сервер защищен паролем");
                }
                else Console.WriteLine("Сервер готов к запуску");
               

            }
            Console.WriteLine("Ввод  текущей конфигурации сервера:\n1.Сколько сейчас на сервере пользователей?");
            int users = int.Parse(Console.ReadLine());
            Console.WriteLine("Выделенная оперативная память (ГБ)");
            int mem1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Установлен ли на сервере пароль?(Д/Н)");
            string b1 = Console.ReadLine();
            Console.WriteLine("Отмечен ли сервер как публичный?(Д/Н)");
            string b2 = Console.ReadLine();
            bool has_password = false;
            bool is_public = false;
            if (b1 == "Д") has_password = true;
            if (b2 == "Д") is_public = true;
            is_server_ready(users, mem1, has_password, is_public);

        }
    }
}
