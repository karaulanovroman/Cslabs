namespace sem_1_lab_03_report_generator
{
    public class Program
    {
        

        public static string BuildReport(string[] lines)
        {
        static string get_event_name(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("Событие началось:"))
                {
                    int start = line.IndexOf("ь:");
                    
                    string event_name = line.Substring(start+2);
                    return event_name;
                }
            }
            return "";


        }
        static string get_event_time(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("Событие началось:"))
                {
                    int end = line.IndexOf("[") - 1;
                    string datetimestr = line.Substring(0, end);
                    DateTime datetime = DateTime.Parse(datetimestr);

                    return datetime.ToString("dd.MM.yyyy");
                }
            }
            return "";
        }
        static string get_winner_team(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("объявлены победителями события"))
                {
                    int first = line.IndexOf(']')+1;
                    int start = line.IndexOf(']', first)+1;
                    int second = line.IndexOf(" ",first);
                    int len = line.IndexOf("объявлены") - second;
                    string winner_team = line.Substring(start,len);
                    return winner_team;
                }
            }
            return "";
        }
        static string get_winner_score(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("очков события"))
                {
                    int start = line.IndexOf("и");

                    int end = line.IndexOf("очков");
                    string winner_score = line.Substring(start+3,end-start-4);
                    return winner_score;
                }
            }
            return "";
        }
        static string get_event_item(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("получили ивентовый предмет:"))
                {
                    int start = line.IndexOf("т:");
                    string event_item = line.Substring(start + 2);
                    return event_item;
                }
            }
            return "";
        }
        static string get_subwinner_team(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("утешительную награду:"))
                {
                    int start = line.IndexOf("d]");
                    int end = line.IndexOf("получили");
                    int len = end - start;
                    string get_subwinner_team = line.Substring(start+3, len-4);
                    return get_subwinner_team;
                }
            }
            return "";
        }
        static string get_subwinner_score(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Contains("утешительную награду:"))
                {
                    int start = line.IndexOf("у:");
                    int end = line.IndexOf("очков");
                    int len = end - start;
                    string get_subwinner_score = line.Substring(start + 2,len-3);
                    return get_subwinner_score;
                }
            }
            return "";
        }
        static int get_warnings_count(string[] lines)
        {
            int event_start = 0;
            int event_end = 0;
            foreach (string line in lines)
            {
                if (line.Contains("Событие началось:")) { event_start = lines.IndexOf(line); }
                if (line.Contains("Событие успешно завершено:")) {event_end = lines.IndexOf(line); }
            }
            int warnings_count = 0;
            foreach (string line in lines[event_start..event_end])
            {
                if (line.Contains("[Warning]")) { warnings_count++; }
            }
            return warnings_count;
        }
        static int get_errors_count(string[] lines)
        {
            int event_start = 0;
            int event_end = 0;
            foreach (string line in lines)
            {
                if (line.Contains("Событие началось:")) { event_start = lines.IndexOf(line); }
                if (line.Contains("Событие успешно завершено:")) { event_end = lines.IndexOf(line); }
            }
            int errors_count = 0;
            foreach (string line in lines[event_start..event_end])
            {
                if (line.Contains("[Error]")) { errors_count++; }
            }
            return errors_count;
        }
            return $"#Итоги события:{get_event_name(lines)}\nДата: {get_event_time(lines)}\nПобедитель:{get_winner_team(lines)}\nОчки победителя:{get_winner_score(lines)}\nИвентовый предмет:{get_event_item(lines)}\nУтешительная награда {get_subwinner_team(lines)}:{get_subwinner_score(lines)}\nПредупреждений во время события: {get_warnings_count(lines)}\nОшибок во время события: {get_errors_count(lines)}";
        }
        static void Main(string[] args)
            {
                string[] lines = File.ReadAllLines("event_server.log");
                Console.WriteLine(BuildReport(lines));

            }
    }
}
