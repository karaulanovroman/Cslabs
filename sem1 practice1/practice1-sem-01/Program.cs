using System.Diagnostics.Tracing;
using System.Text;

namespace practice1_sem_01_
{
    internal class Program
    {
        static void Main(string[] args)
        {   static int Choise()
            {
                Console.WriteLine("Доступные фунции:\n1.Запуск ядерных боеголовок\n2.Кофе в офис\n3.Выход");
                int b = Convert.ToInt32(Console.ReadLine());
                if (b == 1)
                {
                    Console.WriteLine("Открываю панель запуска ядерных боеголовок...");
                    return Nuke();
                }
                else if (b == 2)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Галина у же несет кофе, товарищ президент!");
                    return 0;
                }
                else if (b == 3)
                {
                    Console.WriteLine("Выход из системы...");
                    Environment.Exit(0);
                    return 0;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка, попробуйте еще раз");
                    Console.ForegroundColor = ConsoleColor.White;
                    return Choise();

                }
            }
            static int Nuke()
            {
                static void wait()
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.Write("Ожидайте...\n");
                    Console.Write("\u2622 ");
                    for (int i = 0; i <= Console.WindowWidth-5; i++)
                    {
                        
                        Console.Write("▒");
                        if (i <= 0.75*Console.WindowWidth-5) Thread.Sleep(150);
                        else Thread.Sleep(55);
                    }
                    Console.Write("\u2622\n");
                }
                static bool confirm()
                {
                    string b = Console.ReadLine();
                    if (b == "Д")
                    {
                        Console.WriteLine("\nПодтверждение личности:\nВведите проверочное слово большими буквами:");
                        int i = 0;
                        while (i<3)
                        {
                            string word = Console.ReadLine();
                            if (word == "ДВИЖУХА")
                            {
                                return true;
                                
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;

                                i++;
                                Console.WriteLine($"\nНеправильно, осталось {3 - i} попытки\n");
                                Console.ForegroundColor = ConsoleColor.White;


                            }
                        }
                        return false;
                    }
                    else return false;
                }
                Console.WriteLine("\nКуда вы хотите отправить подарок?\n");
                string point = Console.ReadLine();
                Console.WriteLine($"\nВы уверены что хотите запустить ракету в {point} (Д/Н)");
                if (confirm())
                {
                    wait();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nПоздравляю, подарок отправлен!\n");
                    Console.ForegroundColor = ConsoleColor.White;

                    return 0;
                }
                else if (!confirm())
                {
                    return Choise();
                }
                else return 0;

            }
            Console.Title = "Красная кнопка";
            //Console.WindowHeight = 30;
            //Console.WindowWidth = 55;
            static void deco()
            {
                for (int i = 0; i <= Console.WindowWidth-1; i++)
                {
                    Console.Write("=");
                }
                Console.Write("\n");

            }
            deco();
            Console.WriteLine("\nВас приветствует консоль запуска ядерных боеголовок \n");
            Console.WriteLine("Введите пароль, товарищ президент");
            //подсказка кафакториал от количество ваших президентских сроков
            String password = Console.ReadLine();
            if (password != "120")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.WriteLine("Вход запрещен, на ваш адрес выслан наряд ОМОН ");
                Console.WriteLine("===================================================\n\n\n\n");
                Environment.Exit(0);

            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Добро пожаловать в систему, Пал Лаич\n");

                Console.ForegroundColor = ConsoleColor.White;
            }
            Choise();
            deco();



        }
    }
}
