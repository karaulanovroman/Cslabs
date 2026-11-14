using System;
using System.IO;
using System.Runtime.CompilerServices;

internal class Program
{
    // Current line in file
    private static int _lineIndex;

    // Current nesting level
    private static int _depth;

    // File content
    private static string[] _lines = null!;

    private static string[] Purification(string[] _lines)
    {
        var result = new List<string>();
        for (int i = 0; i<_lines.Length;i++)
        {
            if (_lines[i] != "")
            {
                result.Add(_lines[i]);
            }
        }
        return result.ToArray();
    } 
    private static void TreeStick(int ex,string line)
    {
        if (ex == 1)
        {
            var name = line.Substring(6);

            Console.Write(GetIndent());
            Console.WriteLine("└─" + name);

            _depth++;
            _lineIndex++;

            PrintTree();
        } else if (ex == 0)
        {
            var name = line.Substring(6);

            Console.Write(GetIndent());
            Console.WriteLine("├─" + name);

            _depth++;
            _lineIndex++;

            PrintTree();
        }
    }
    private static string GetIndent()
    {
        var result = "";

        for (var i = 0; i < _depth; i++)
        {
            result += "│ ";
        }

        return result;
    }
    private static void PrintTree()
    {
        while (_lineIndex < _lines.Length)
        {
            var line = _lines[_lineIndex].Trim();
            var prev_line = "";
            var next_line = "";
            if (_lineIndex < _lines.Length + 1) {prev_line = _lines[_lineIndex-1].Trim(); }
            if (_lineIndex < _lines.Length - 1) {next_line = _lines[_lineIndex + 1].Trim(); }
            if (line == "END")
            {
                _depth--;

                _lineIndex++;

                return;
            }

            if (line.StartsWith("BEGIN "))
            {
                if (!(prev_line.StartsWith("BEGIN ")) && !(next_line.StartsWith("BEGIN ")))
                {
                    TreeStick(1, line);
                }
                else
                {
                    TreeStick(0, line);
                }
            }
        }
    }
    private static void Main()
    {
        _lines = Purification(File.ReadAllLines("tree.txt"));

        Console.WriteLine(_lines[0]);

        _lineIndex = 1;

        PrintTree();

        Console.ReadLine();
    }
}
//Что измениось?
//1. В методе GetIndent перебор идет до глубины, не включая само значение(строго меньше)
//2. В переменную name попадал лишний пробел, исправлено
//3. В методе PrintTree добавлен учет предыдущей и следующей строки для корректного отображения крайних ветвей дерева
//4. В методе PrintTree исправлен переход на следующий уровень рекурсии
