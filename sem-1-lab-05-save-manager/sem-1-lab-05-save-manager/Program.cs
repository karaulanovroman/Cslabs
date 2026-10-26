using Newtonsoft.Json;
namespace sem_1_lab_05_save_manager
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }
    public class SaveEntry
    {
        public string name { get; set; }
        public string file { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class SaveData
    {
        public string Character { get; set; }
        public string Map { get; set; }
        public Position Position { get; set; }
        public int Health { get; set; }
        public List<string> Inventory { get; set; }
    }

    public class Program
    {
        static List<SaveEntry> LoadIndex(string indexPath)
        {
            try
            {
                string jsonindex = File.ReadAllText(indexPath);
                List<SaveEntry> listindex = JsonConvert.DeserializeObject<List<SaveEntry>>(jsonindex);
                return listindex;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw;
            }
        }
        static SaveData LoadSave(string savesDirectory, SaveEntry entry)
        {
            try
            {
                string jsonsave = File.ReadAllText($"{savesDirectory}/{entry.file}");
                SaveData save = JsonConvert.DeserializeObject<SaveData>(jsonsave);
                return save;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw;
            }
        }
        static void PrintSave(SaveEntry entry, SaveData save)
        {
            Console.Write($"Имя сохранения - {entry.name}\nПерсонаж - {save.Character}\nКарта - {save.Map}\nКоординаты - ({save.Position.x},{save.Position.y})\nЗдоровье - {save.Health}\nПредметы инвентаря - {string.Join(", ",save.Inventory)}\n");
        }
        static SaveEntry FindByName(List<SaveEntry> entries, string name)
        {
            foreach (SaveEntry entry in entries)
            {
                if (string.Equals(entry.name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return entry;
                }
            }
            return null;
        }
        static  void SaveGame(string savesDirectory, SaveEntry entry, SaveData save)
        {
            string jsonsave = JsonConvert.SerializeObject(save, Formatting.Indented);
            File.WriteAllText($"{savesDirectory}/{entry.file}", jsonsave);            
        }
        static void SaveIndex(string indexPath, List<SaveEntry> entries)
        {
            File.WriteAllText(indexPath, JsonConvert.SerializeObject(entries, Formatting.Indented));
        }
        static void CreateBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
        {
            File.Copy($"{savesDirectory}/{entry.file}", $"{backupsDirectory}/{entry.file}", overwrite: true);
            Console.WriteLine("Бэкап создан");
        }
        static void RestoreBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
        {
            File.Copy($"{backupsDirectory}/{entry.file}", $"{savesDirectory}/{entry.file}", overwrite: true);
            Console.WriteLine("Бэкап восстановлен");
        }
        static void DeleteSave(string savesDirectory, string indexPath, List<SaveEntry> entries, SaveEntry entry)
        {
            File.Delete($"{savesDirectory}/{entry.file}");
            entries.Remove(entry);
            SaveIndex(indexPath, entries);
            Console.WriteLine("Сохранение удалено!");
        }
        static void SavesMenu(List<SaveEntry> indexes,string choice)
        {
            if (choice == "1")
            {
                Console.WriteLine("\nДля просмотра данных сохранения ввести имя");
                int i = 1;
                foreach (SaveEntry index in indexes)
                {
                    Console.WriteLine($"{Convert.ToString(i)}. {index.name} {index.file} {index.updatedAt}");
                    i++;
                    //i существует т.к. изначально случайно сделал переход к сохранению по номеру
                }
                Console.WriteLine("0. Назад");
                Console.Write("Ввод: ");
                string savemenu_choice = Console.ReadLine();
                if (savemenu_choice == "0") return;
                else
                {
                    SaveActionsMenu(savemenu_choice, indexes);
                }
                ;

            }
            else if (choice == "0")
            {
                Environment.Exit(0);
            }
            else
            {
                WarningColor("Некорректный ввод!", ConsoleColor.Red);
            }

        }
        static void SaveActionsMenu(string savemenu_choice,List<SaveEntry> indexes)
        {
            try
            {
                SaveEntry index = FindByName(indexes, savemenu_choice);
                int current_index = indexes.IndexOf(index);
                PrintSave(index, LoadSave("saves", index));
                Console.WriteLine("\nВыберите действие\n1. Изменить данные сохранения\n2. Создать резервную копию\n3. Восстановить резервную копию\n4. Удалить сохранение\n0. Назад");
                Console.Write("Ввод: ");
                string saveaction_choice = Console.ReadLine();
                SaveActions(saveaction_choice, indexes, current_index, savemenu_choice);
            }
            catch (NullReferenceException)
            {
                WarningColor("Файл сохранения не найден!", ConsoleColor.Red);
                SavesMenu(indexes, "1");
            }
            catch (JsonException)
            {
                WarningColor("Ошибка чтения файла сохранения!", ConsoleColor.Red);
                SavesMenu(indexes, "1");
            }
            catch (FileNotFoundException)
            {
                WarningColor("Ошибка в указании пути до файла в индексе!", ConsoleColor.Red);
            }

        }
        static void SaveActions(string saveaction_choice,List<SaveEntry> indexes,int current_index, string savemenu_choice)
        {
            switch (saveaction_choice)
            {
                case "1":
                    ChangeSave(indexes, current_index,saveaction_choice,savemenu_choice);
                    break;
                case "2":
                    CreateBackup("saves", "backups", indexes[current_index]);
                    SaveActionsMenu(savemenu_choice, indexes);
                    break;
                case "3":
                    RestoreBackup("saves", "backups", indexes[current_index]);
                    SaveActionsMenu(savemenu_choice, indexes);
                    break;
                case "4":
                    DeleteSave("saves", "index.json", indexes, indexes[current_index]);
                    SaveActionsMenu(savemenu_choice,indexes);
                    break;
                case "0":
                    SavesMenu(indexes, "1");
                    break;
                default:
                    WarningColor("Некорректный ввод!", ConsoleColor.Red);
                    SaveActionsMenu(savemenu_choice, indexes);
                    break;
                
            }
        }
        static void ChangeSave(List<SaveEntry> indexes, int current_index,string saveaction_choice,string savemenu_choice)
        {
            Console.WriteLine("\nИзменение данных сохранения:\nЧто вы хотите изменить в сохранении?\n1. Карту\n2. Координаты\n3. Здоровье\n4. Предметы инвентаря\n0. Ничего");
            Console.Write("Ввод: ");
            string choice = Console.ReadLine();
            SaveData current_save = LoadSave("saves", indexes[current_index]);
            switch (choice)
            {
                case "1":
                    
                    Console.WriteLine($"Текущая карта: {current_save.Map}\nВведите новое значение:");
                    Console.Write("Ввод: ");
                    string new_map = Console.ReadLine();
                    if (new_map != "")
                    {
                        current_save.Map = new_map;
                        SaveGame("saves", indexes[current_index], current_save);
                        indexes[current_index].updatedAt = DateTime.Now;
                        SaveIndex("index.json", indexes);
                        SaveActionsMenu(savemenu_choice, indexes);
                    } else ChangeSave(indexes,current_index,saveaction_choice,savemenu_choice);
                    break;
                case "2":
                    Console.WriteLine($"Текущие координаты: Х=({LoadSave("saves", indexes[current_index]).Position.x}),Y=({LoadSave("saves", indexes[current_index]).Position.y})\nВыбор координаты (x/y)");
                    Console.Write("Ввод: ");
                    string xy = Console.ReadLine();
                    try
                    {
                        if (xy == "x")
                        {
                            Console.WriteLine("Новый X:");
                            Console.Write("Ввод: ");
                            string new_x = Console.ReadLine();
                            if (new_x != "")
                            {
                                current_save.Position.x = Convert.ToInt32(new_x);
                                SaveGame("saves", indexes[current_index], current_save);
                                indexes[current_index].updatedAt = DateTime.Now;
                                SaveIndex("index.json", indexes);
                                ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                            }
                            else
                                ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                        }
                        else if (xy == "y")
                        {
                            Console.WriteLine("Новый Y:");
                            Console.Write("Ввод: ");
                            string new_y = Console.ReadLine();
                            if (new_y != "")
                            {
                                current_save.Position.y = Convert.ToInt32(new_y);
                                SaveGame("saves", indexes[current_index], current_save);
                                indexes[current_index].updatedAt = DateTime.Now;
                                SaveIndex("index.json", indexes);
                                ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                            }
                            else
                                ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                        }
                        else
                        {
                            WarningColor("Некорректный ввод!", ConsoleColor.Red);
                            ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        WarningColor("Некорректный ввод!", ConsoleColor.Red);
                        ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                    }
                    break;
                case "3":
                    Console.WriteLine($"Текущее здоровье: {LoadSave("saves", indexes[current_index]).Health}\nВведите новое значение:");
                    Console.Write("Ввод: ");
                    string new_HP = Console.ReadLine();
                    try
                    {
                        if (new_HP != "")
                        {
                            current_save.Health = Convert.ToInt32(new_HP);
                            SaveGame("saves", indexes[current_index], current_save);
                            indexes[current_index].updatedAt = DateTime.Now;
                            SaveIndex("index.json", indexes);
                            ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                        }
                        else
                            ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                        break;
                    }
                    catch (FormatException)
                    {
                        WarningColor("Некорректный ввод!", ConsoleColor.Red);
                        ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                    }
                    break;
                case "4":
                    Console.WriteLine($"Текущий инвентарь: {string.Join(",",LoadSave("saves", indexes[current_index]).Inventory)}\nВведите чере запятую:");
                    Console.Write("Ввод: ");
                    string new_inv = Console.ReadLine();
                    if (new_inv != "")
                    {
                        List<string> new_list = new List<string>(new_inv.Split(","));
                        current_save.Inventory = new_list;
                        SaveGame("saves", indexes[current_index], current_save);
                        indexes[current_index].updatedAt = DateTime.Now;
                        SaveIndex("index.json", indexes);
                        ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                    }
                    else
                        ChangeSave(indexes, current_index, saveaction_choice, savemenu_choice);
                    break;
                case "0":
                    SaveActionsMenu(savemenu_choice,indexes);break;
                default:
                    WarningColor("Некорректный ввод!", ConsoleColor.Red);
                    SaveActions(saveaction_choice, indexes, current_index, savemenu_choice);
                    break;

            }
        }
        static void WarningColor(string text,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{text}\n");
            Console.ForegroundColor = ConsoleColor.White;

        }

        static void Main(string[] args)
        {
            if (Directory.Exists("saves") == false)
            {
                WarningColor("Папка сохранений не найдена, создаю новую...", ConsoleColor.Green);
                Directory.CreateDirectory("saves");
            }
            if (Directory.Exists("backups") == false)
            {
                WarningColor("Папка бэкапов не найдена, создаю новую...", ConsoleColor.Green);
                Directory.CreateDirectory("backups");

            }
            try
            { 
                List<SaveEntry> indexes = LoadIndex("index.json");

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("||===================||");
                    Console.WriteLine("||Менеджер сохранений||");
                    Console.WriteLine("||===================||\n");

                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("Выберите действие: \n1. Просмотр сохранений\n0. Выйти");
                    Console.Write("Ввод: ");
                    string choice = Console.ReadLine();
                    SavesMenu(indexes, choice);
                }
            }
            catch (FileNotFoundException) 
            {
                WarningColor("Файл индекса не найден!", ConsoleColor.Red);
            }
            catch (JsonException)
            {
                WarningColor("Ошибка чтения файла индекса", ConsoleColor.Red);
            }
        }
    }
}
