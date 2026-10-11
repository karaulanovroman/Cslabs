namespace sem_1_lab_04_event_log
{
    public class LogEntry
    {
        public DateTime Timestamp;
        public string Level;
        public string Category;
        public string Message;

    }
    public class Program
    {   
        public static LogEntry[] ParseLog(string[] lines)
        {
            static DateTime get_time(string line)
            {
                int end = line.IndexOf("[") - 1;
                string datetimestr = line.Substring(0, end);
                DateTime datetime = DateTime.Parse(datetimestr);
                return datetime;
            }
            static string get_level(string line)
            {
                int index1 = line.IndexOf("[")+1;
                int index2 = line.IndexOf("]");

                string LogLevel = line.Substring(index1, index2 - index1);
                return LogLevel;
            }
            static string get_category(string line)
            {
                int index1 = line.IndexOf("]") + 2;
                int index2 = line.IndexOf(" ", index1)-1;
                string logtag = line.Substring(index1, index2 - index1);
                return logtag;
            }
            static string get_message(string line)
            {
                int index = line.IndexOf("] ");
                int len = line.Length - index;
                string logtext = line.Substring(index + 2, len - 2);
                return logtext;
            }
            LogEntry[] type_logs = new LogEntry[lines.Length];
            for (int i = 0; i < type_logs.Length; i++)
            {
                LogEntry log = new LogEntry()
                {
                    Timestamp = get_time(lines[i]),
                    Level = get_level(lines[i]),
                    Category = get_category(lines[i]),
                    Message = get_message(lines[i])
                };
                type_logs[i] = log;
            }
            return type_logs;
        }
        public static LogEntry[] FilterByDate(LogEntry[] entries, DateTime date)
        {
            LogEntry[] date_sorted_temp = new LogEntry[entries.Length];
            int j = 0;
            for (int i = 0;i<entries.Length;i++)
            {
                if (entries[i].Timestamp.Date == date)
                {
                    date_sorted_temp[j] = entries[i];
                    j++;
                }
            }
            LogEntry[] date_sorted = new LogEntry[j];
            Array.Copy(date_sorted_temp, date_sorted, j);
            return date_sorted;
        }
        public static LogEntry[] FilterByLevel(LogEntry[] entries, string level)
        {
            LogEntry[] level_sorted_temp = new LogEntry[entries.Length];
            int j = 0;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Level == level)
                {
                    level_sorted_temp[j] = entries[i];
                    j++;
                }
            }
            LogEntry[] level_sorted = new LogEntry[j];
            Array.Copy(level_sorted_temp, level_sorted, j);
            return level_sorted;
        }
        public static LogEntry[] FilterByCategory(LogEntry[] entries, string category)
        {
            LogEntry[] category_sorted_temp = new LogEntry[entries.Length];
            int j = 0;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Category == category)
                {
                    category_sorted_temp[j] = entries[i];
                    j++;
                }
            }
            LogEntry[] category_sorted = new LogEntry[j];
            Array.Copy(category_sorted_temp, category_sorted, j);
            return category_sorted;
        }
        public static LogEntry[] Search(LogEntry[] entries, string text)
        {
            LogEntry[] text_sorted_temp = new LogEntry[entries.Length];
            int j = 0;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Message.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    text_sorted_temp[j] = entries[i];
                    j++;
                }
            }
            LogEntry[] text_sorted = new LogEntry[j];
            Array.Copy(text_sorted_temp, text_sorted, j);
            return text_sorted;
        }
        public static int CountByLevel(LogEntry[] entries, string level)
        {
            return FilterByLevel(entries, level).Length;
        }
        public static string GetServerStatus(LogEntry[] entries)
        {
            static int CountServerFatal(LogEntry[] entries)
            {
                int count = 0;
                foreach (LogEntry entry in entries)
                {
                    if ((entry.Level == "Fatal") && (entry.Category == "Server")) count++;
                }
                return count;
            }
            if (CountServerFatal(entries) != 0) return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
            else if (CountByLevel(entries, "Error") != 0) return "Есть ошибки: требуется проверка";
            else if (CountByLevel(entries, "Error") + CountByLevel(entries, "Fatal") == 0) return "Сервер работает штатно";
            else return null;
        }
        public static void Main(string[] args)
        {
            string[] logs = File.ReadAllLines("event_server.log");
            DateTime date = DateTime.Parse("2026-09-02");
            foreach (LogEntry line in FilterByDate(ParseLog(logs),date))
            {
                Console.WriteLine(line.Message);
            }
            //string c = CountByLevel(ParseLog(logs), level).ToString();
            //Console.WriteLine(c);
            //Console.WriteLine(GetServerStatus(ParseLog(logs)));
        }
    }
}
