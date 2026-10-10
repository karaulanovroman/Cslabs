using System.Security.Cryptography.X509Certificates;

namespace sem_1_practice_2_log_parse
{
    internal class Program
    {
        //2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox
        public static DateTime GetDateTime(string line)
        {
            int index = line.IndexOf("[") - 1;
            string datetimestr = line.Substring(0, index);
            DateTime datetime = DateTime.Parse(datetimestr);
            return datetime.Date;
        }
        public static string GetLogLevel(string line)
        {
            int index1 = line.IndexOf("[");
            int index2 = line.IndexOf("]")+1;

            string LogLevel = line.Substring(index1,index2-index1);
            return LogLevel;
        }
        public static string GetLogTag(string line)
        {
            int index1 = line.IndexOf("]")+1;
            int index2 = line.IndexOf(" ",index1);
            string logtag = line.Substring(index1,index2-index1);
            return logtag;
        }
        public static string GetLogText(string line)
        {
            int index = line.IndexOf("] ");
            int len = line.Length - index;
            string logtext = line.Substring(index+2,len-2);
            return logtext;
        }
        public static (DateTime,string,string,string) GetLogParts(string line)
        {
            return (GetDateTime(line), GetLogLevel(line), GetLogTag(line), GetLogText(line));
        }

        static void Main(string[] args)
        {

            string[] lines = File.ReadAllLines("event_server.log");
            foreach (string line in lines)
            {
                Console.WriteLine(GetLogParts(line));

            }
        }
    }
}
