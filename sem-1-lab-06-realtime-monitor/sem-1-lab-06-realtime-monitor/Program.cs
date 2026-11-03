namespace sem_1_lab_06_realtime_monitor;

internal class Program
{
    static int[] cpu_mem = new int[20];
    static int[] ram_mem = new int[20];
    static int[] tem_mem = new int[20];
    
    static void OnDataReceived(double cpu, double ram, double temperature)
    {
        Console.WindowHeight = Console.WindowHeight + 1;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();
        Console.WriteLine($"CPU: {cpu}%");
        Console.WriteLine($"RAM: {ram}%");
        Console.WriteLine($"Temperature: {temperature} °C");
        RoundnMove(cpu_mem, cpu);
        RoundnMove(ram_mem, ram);
        RoundnMove(tem_mem, temperature);        
        Console.WriteLine("CPU");
        RenderGraph(10, 60,10, cpu_mem);
        Console.WriteLine("\nRAM");
        RenderGraph(10, 60,10, ram_mem);
        Console.WriteLine("\nTemperature");
        RenderGraph(10, 100,20, tem_mem);
        Console.WriteLine("\nНажмите Enter для остановки мониторинга");
        Console.WindowHeight = Console.WindowHeight -1;
    }
    static void RenderGraph(int mincap,int maxcap,int leg, int[] data)
    {        
        int buffsize = Convert.ToString(maxcap).Length;
        int GraphLenght = GraphString(data, 0).Length+1;
        for (int i = maxcap; i >= mincap; i-=leg)
        {
            Console.WriteLine($"{i+new string(' ',buffsize-Convert.ToString(i).Length)}| {GraphString(data, i)}");
        }
        Console.WriteLine($"{new string(' ', buffsize)}+{new string('-',GraphLenght+buffsize-2)}");
        Console.Write($"{new string(' ',buffsize)}  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20");


    }
    static void RoundnMove(int[] numbas,double num)
    {
        for (int i = numbas.Length-1; i > 0; i--)
        {
            numbas[i] = numbas[i - 1];
        }
        numbas[0] = Convert.ToInt32(num);
    }
    static string GraphString(int[] data, int cap)
    {
        char[] chars = new char[data.Length];
        for (int i=0;i<data.Length;i++)
        {
            if (data[i] >= cap)
            {
                chars[i] = '✞';
            }
            else chars[i] = ' ';
        }
        string res = string.Join("  ", chars);
        return res;
    }
    static void MonitoringStart()
    {
        var monitor = new ServerMonitor();
        monitor.DataReceived += OnDataReceived;
        monitor.Start();
        Console.ReadLine();
        monitor.Stop();

    }
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Realtime monitor\n1.Начать мониторинг\n0.Выйти");
            string userchoise = Console.ReadLine();
            if (userchoise == "1")
            {
                MonitoringStart();
            }
            else if (userchoise == "0") Environment.Exit(0);
            else Console.WriteLine("Неправильный ввод");
            

        }
        
    }
}
