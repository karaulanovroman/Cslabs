namespace lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string servername1 = "RU1";
            string servername2 = "EU1";
            byte users2 = 120;
            byte users1 = 194;
            int objects2 = 40856;
            int objects1 = 936746;
            long donatesummary2 = 83284675768;
            long donatesummary1 = 83456684689;
            float ping2 = 146.2f;
            float ping1 = 156.8f;
            double hardwaretemp2 = 65.35;
            double hardwaretemp1= 80.48;
            decimal admins_mid_iq2 = 0.648M;
            decimal admins_mid_iq1 = 0.487M;

            Console.WriteLine("Server status check console starting...");
            Console.WriteLine("Choose server");
            Console.WriteLine($"1. {servername1}");
            Console.WriteLine($"2. {servername2}");
            char ch = Convert.ToChar(Console.ReadLine());
            if (ch == '2')
            {
                Console.Write("RKN has blocked your request...");
                return;
            }
            Console.WriteLine($"Current server status:\n Number of: \n  Users = {users1} \n  Objects = {objects1} \n  Admins mid iq = {admins_mid_iq1} \n Tech: \n  Ping = {ping1} \n  Hardware temperature = {hardwaretemp1}");
            Console.WriteLine($"Congrats, those fools donated {donatesummary1}$ already!!!");
            Console.WriteLine("Compare with last check? (Y/N)");
            if (Console.ReadLine() == "Y")
            {
                Console.WriteLine("Comparing with last check...");
                bool moreuser = false;
                if (users1 > users2)
                {
                    moreuser = true;
                }
                bool moremoney = false;
                if (donatesummary1 > donatesummary2)
                {
                    moremoney = true;
                }
                bool lessping = false;
                if (ping2 > ping1)
                {
                    lessping = true;
                }
                bool better = false;
                if ((moreuser || lessping) && moremoney)
                {
                    better = true;
                }
           
                Console.WriteLine($"Server status comparison:\n Number of: \n  Users = {users2} ---> {users1}\n  Objects = {objects2} ---> {objects1} \n  Admins mid iq = {admins_mid_iq2} ---> {admins_mid_iq1} \n Tech: \n  Ping = {ping2} ---> {ping1} \n  Hardware temperature = {hardwaretemp2} ---> {hardwaretemp1}");
                if (better)
                {

                    Console.WriteLine("Your server became better!!!");
                    Console.WriteLine("Server check ending...");

                }
            }
            else
            {

                Console.WriteLine("Server check ending...");
            }






        }
    }
}
