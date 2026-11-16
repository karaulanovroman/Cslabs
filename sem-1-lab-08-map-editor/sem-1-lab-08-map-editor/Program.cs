using System.IO.Enumeration;

namespace sem_1_lab_08_map_editor
{
    public struct Player
    {
        public Position player_pos;
        public static Player FindPlayer(Map map)
        {
            int p_x = 0;
            int p_y = 0;
            for(int y = 0; y < map.cells.Length;y++)
            {
                for (int x = 0; x < map.cells[y].Length; x++)
                {
                    if (map.cells[y][x].cell_type == 'P')
                    {
                        p_x = x;
                        p_y = y;
                        map.cells[y][x].cell_type = '.';
                    }
                    
                }
            }
            return new Player { player_pos = new Position { x = p_x, y = p_y } };
        }
        public static Player MovePlayer(Player player, Position new_pos)
        {
            player.player_pos.x = new_pos.x;
            player.player_pos.y = new_pos.y;
            return player;
        }
        public static void CreatePlayer(Map map,Position pos)
        {
            map.cells[pos.y][pos.x].cell_type = 'P';
        }
        public static bool WallCheck(Map map,Position pos)
        {
            if (map.cells[pos.y][pos.x].cell_type == '#')
            {
                return true;
            }
            return false;
        }
    }
    public struct Position
    {
        public int x;
        public int y;
    }
    public struct Cell
    {
        public static bool IsCell(char c)
        {
            if(c == '.' || c == '#' || c == 'W')
            {
                return true;
            }
            return false;
        }
        public char cell_type;
    }
    public class Map
    {
        public string name;
        public int width;
        public int height;
        public Cell[][] cells;
        public static Map LoadMap(string name)
        {
            string path = $"{name}.txt";
            Map res = new Map();
            res.name = name;
            string[] file = File.ReadAllLines(path);

            int d_index = file[0].IndexOf('|');
            res.width = Convert.ToInt32(file[0].Substring(0,d_index));
            res.height = Convert.ToInt32(file[0].Substring(d_index+1));

            Cell[][] cells = new Cell[res.height][];
            for (int i = 0; i < res.height; i++)
            {
                cells[i] = new Cell[res.width];
            }
            for (int i_y = 0; i_y < cells.Length; i_y++)
            {
                for(int i_x = 0; i_x < cells[i_y].Length; i_x++)
                {
                    cells[i_y][i_x].cell_type = file[i_y+1][i_x];
                }
            }
            res.cells = cells;
            return res;
        }
        public static void SaveMap(string path,Map map,Player player)
        {
            //Клетка под игроком стирается при сохранении, это обусловлено заданым форматом сохранения
            //Чтобы это исправить, можно отдельно сохранять местоположение игрока в файле(схоже с шириной и высотой),
            //,однако это потребовало бы изменения исходного файла map.txt (и переписывания методов загрузки и сохранения карт)
            string str_map = "";
            for (int i_y = 0; i_y < map.cells.Length; i_y++)
            {
                for (int i_x = 0; i_x < map.cells[i_y].Length; i_x++)
                {
                    if (i_x == player.player_pos.x && i_y == player.player_pos.y)
                    {
                        str_map += 'P';
                    }
                    else
                    {
                        str_map+=(map.cells[i_y][i_x].cell_type);
                    }
                    
                }
                str_map+='\n';
            }
            File.WriteAllText(path, $"{map.width}|{map.height}\n{str_map}");
        }
        public static bool MapCheck(Map map2, Map map,Player player,Player player2)
        {
            bool player_x = player2.player_pos.x == player.player_pos.x;
            bool player_y = player2.player_pos.y == player.player_pos.y;
            int cellcount = map.height * map.width;
            int cellcount2 = map2.height * map2.width;
            int samecells = 0;
            if (player_x && player_y && cellcount == cellcount2)
            {
                for (int i_y = 0; i_y < map.cells.Length; i_y++)
                {
                    for (int i_x = 0; i_x < map.cells[i_y].Length; i_x++)
                    {
                        if (map.cells[i_y][i_x].cell_type == map2.cells[i_y][i_x].cell_type)
                        {
                            samecells++;
                        }
                    }
                }
            }
            if (samecells == cellcount) { return true; }
            return false;
            

        }
        public static void RenderMap(Map map,Player player)
        {
            for (int i_y = 0; i_y < map.cells.Length; i_y++)
            {
                for (int i_x = 0; i_x < map.cells[i_y].Length; i_x++)
                {
                    if (i_y == player.player_pos.y && i_x == player.player_pos.x)
                    {
                        Console.Write('P');
                    }
                    else Console.Write(map.cells[i_y][i_x].cell_type);
                }
                Console.Write('\n');
            }
        }
        public static void NewMap(string path,int width,int height)
        {
            string emptymap = new string('#',width) + '\n';
            for (int y = 0; y < height-2; y++)
            {
                emptymap += '#' + new string('.', width - 2) + '#' + '\n';
            }
            emptymap += new string('#', width);
            File.WriteAllText(path, $"{width}|{height}\n{emptymap}");
        }
        public static void ChangeCellType(Map map,Position pos,char cellType)
        {
            map.cells[pos.y][pos.x].cell_type = cellType;
        }
        
    }

    internal class Program
    {
        static void LoadMenu()
        {

            Console.WriteLine("Редактор карт\n1.Загрузить карту\n2.Создать пустую карту\n0.Выйти");
            var c = Console.ReadLine();
            if (c == "0") { Environment.Exit(0); }
            if (c == "1")
            {
                MapMenu();
            }
            if (c == "2")
            {
                NewMapMenu();
            }
        }
        static void NewMapMenu()
        {
            try
            {
                Console.WriteLine("Введите название карты:");
                string name = Console.ReadLine();
                Console.WriteLine("Введите высоту");
                int height = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите ширину");
                int width = Convert.ToInt32(Console.ReadLine());
                if (height>=3 && width >= 3)
                {
                    Map.NewMap($"{name}.txt", width, height);
                    Map map = Map.LoadMap(name);
                    map.name = name;
                    CreatePlayerMenu(map);
                }
                else
                {
                    Console.WriteLine("Минимальный размер карты:3x3 (Стены по краям)");
                    NewMapMenu();
                }
                
            }
            catch (FormatException) { Console.WriteLine("Неправильный формат ввода!"); LoadMenu(); }
            catch (OverflowException) { Console.WriteLine("Слишком большое значение!"); LoadMenu(); }

        }
        static void CreatePlayerMenu(Map map)
        {
            try
            {
                Console.WriteLine("Разместите игрока:\nЕго координата X:");
                int player_x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Его координата Y:");
                int player_y = Convert.ToInt32(Console.ReadLine());
                Position player_pos = new Position { x = player_x - 1, y = player_y - 1 };
                if (!Player.WallCheck(map, player_pos))
                {
                    Player.CreatePlayer(map, player_pos);
                    Player player = Player.FindPlayer(map);
                    Map.RenderMap(map, player);
                    MapRedMenu(map, player);
                }
                else
                {
                    Console.WriteLine("По краям карты стены, тут стена!");
                    CreatePlayerMenu(map);
                }
            }
            catch (FormatException) { Console.WriteLine("Неправильный формат ввода!"); CreatePlayerMenu(map); }
            catch (OverflowException) { Console.WriteLine("Слишком большое значение!"); CreatePlayerMenu(map); }
            catch (IndexOutOfRangeException) { Console.WriteLine("Это за границами карты!"); CreatePlayerMenu(map); }
        }
        static void MapMenu()
        {
            try
            {
                Console.WriteLine("Введите название карты");
                string name = Console.ReadLine();
                Map map = Map.LoadMap(name);
                Player player = Player.FindPlayer(map);
                Map.RenderMap(map, player);
                MapRedMenu(map, player);
            }
            catch (FileNotFoundException) { Console.WriteLine("Такое сохранение не найдено!"); LoadMenu(); }
            catch (IndexOutOfRangeException) { Console.WriteLine("Ошибка чтения файла карты!"); LoadMenu(); }
        }
        static void MapRedMenu(Map map,Player player)
        {
            Console.WriteLine("Выберите действие:\n0.Выйти\n1.Переместить игрока\n2.Изменить клетку\n3.Сохранить изменения\n4.сделать снимок карты\n5.Загрузить снимок\n6.Отобразить карту");
            var c = Console.ReadLine();
            if (c == "0")
            {
                Map map2 = Map.LoadMap(map.name);
                Player player2 = Player.FindPlayer(map2);

                if (Map.MapCheck(map, map2, player, player2))
                {
                    LoadMenu();
                }
                else
                {
                    ExitWithoutSave(map, player);
                }
            }
            if (c == "1")
            {
                MovePlayerMenu(map,player);
            }
            if (c == "2")
            {
                ChangeCellMenu(map,player);
            }
            if (c == "2")
            {
                ChangeCellMenu(map, player);
            }
            if (c == "3")
            {
                SaveMenu(map, player);
            }
            if (c == "4")
            {
                ShotSaveMenu(map, player);
            }
            if (c == "5")
            {
                ShotLoadMenu(map, player);
            }
            if (c == "6")
            {
                Map.RenderMap(map, player);
            }
            MapRedMenu(map, player);
        }
        static void ShotSaveMenu(Map map, Player player)
        {
            if (!Directory.Exists("shots"))
            {
                Directory.CreateDirectory("shots");
            }
            Map.SaveMap($"shots/{map.name}.txt", map, player);
            Console.WriteLine("Снимок сделан");
            MapRedMenu(map, player);
        }
        static void ShotLoadMenu(Map map,Player player)
        {
            map = Map.LoadMap($"shots/{map.name}");
            player = Player.FindPlayer(map);
            Console.WriteLine("Снимок загружен");
            Map.RenderMap(map, player);
            MapRedMenu(map, player);
        }
        static void SaveMenu(Map map,Player player)
        {
            Map.SaveMap($"{map.name}.txt", map, player);
            Console.WriteLine("Сохранено");
            MapRedMenu(map, player);
        }
        static void MovePlayerMenu(Map map,Player player)
            {
            try
            {
                Console.WriteLine("Введите новые координаты игрока:\nX=");
                int x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Y=");
                int y = Convert.ToInt32(Console.ReadLine());
                Position new_pos = new Position { x = x - 1, y = y - 1 };
                if (!Player.WallCheck(map, new_pos))
                {
                    player = Player.MovePlayer(player, new_pos);
                    Map.RenderMap(map, player);
                    MapRedMenu(map, player);
                }
                else
                {
                    Console.WriteLine("Тут стена!");
                    MovePlayerMenu(map, player);
                }
            }
            catch (OverflowException) { Console.WriteLine("Слишком большие значения!"); }
            catch (FormatException) { Console.WriteLine("Неправильный формат ввода!"); }
            catch (IndexOutOfRangeException) { Console.WriteLine("Это за пределами карты!"); }
            }
        static void ChangeCellMenu(Map map,Player player)
        {
            try
            {
                Console.WriteLine("Введите координаты клетки:\nX=");
                int x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Y=");
                int y = Convert.ToInt32(Console.ReadLine());
                Position cell_pos = new Position { x = x - 1, y = y - 1 };
                Console.WriteLine("Введите новый тип клетки");
                char new_cell_type = Convert.ToChar(Console.ReadLine());
                if (Cell.IsCell(new_cell_type))
                {
                    Map.ChangeCellType(map, cell_pos, new_cell_type);
                    Map.RenderMap(map, player);
                    MapRedMenu(map, player);
                }
                else
                {
                    Console.WriteLine("Такого типа клетки нет!");
                    ChangeCellMenu(map, player);
                }
            }
            catch (OverflowException) { Console.WriteLine("Слишком большие значения!"); }
            catch (FormatException) { Console.WriteLine("Неправильный формат ввода!"); }
            catch (IndexOutOfRangeException) { Console.WriteLine("Это за пределами карты!"); }
        }
        static void ExitWithoutSave(Map map,Player player)
        {
            Console.WriteLine("Вы уверены что хотите выйти без сохранения?(Y/N)(default N)");
            var c = Console.ReadLine();
            if (c == "Y") { LoadMenu(); }
            if (c == "N") { MapRedMenu(map,player); }
            MapRedMenu(map, player);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                LoadMenu();
            }
        }
    }
}
