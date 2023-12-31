﻿namespace TAIO;

public static class GraphDisplayer
{
    const string UNDERLINE = "\x1B[4m";
    const string RESET_UNDERLINE = "\x1B[0m";

    const ConsoleColor SOLUTION_VERTEX_COLOR = ConsoleColor.Blue;
    const ConsoleColor SOLUTION_EDGE_COLOR = ConsoleColor.Blue;
    private const ConsoleColor DEFAULT_COLOR = ConsoleColor.DarkGray;

    /// <summary>
    /// Max clique problem
    /// </summary>
    public static void DisplaySolution(this Graph g1, List<int> solutionVertices, int l, double porosity)
    {
        Console.WriteLine();
        Console.WriteLine("Objaśnienie wizualizacji:");
        Console.WriteLine("1. Niebieski kolor oznacza wierzchołki i krawędzie należące do rozwiązania problemu.");
        Console.WriteLine("2. Szary kolor oznacza pozostałe wierzchołki i krawędzie.");
        Console.WriteLine("3. Zapis X|Y oznacza, że spośród X+Y krawędzi X z nich należy do rozwiązania problemu ");
        Console.WriteLine("   oraz Y z nich nie należy do rozwiązania.");
        Console.WriteLine();

        Console.ForegroundColor = DEFAULT_COLOR;

        Console.Write($"      | ");
        for (int i = 0; i < g1.NumberOfVertices; i++)
        {
            if (solutionVertices.Contains(i))
            {
                Console.ForegroundColor = SOLUTION_VERTEX_COLOR;

                Console.Write($"{i}".PadLeft(7));

                Console.ForegroundColor = DEFAULT_COLOR;
            }
            else
            {
                Console.Write($"{i}".PadLeft(7));
            }
        }
        Console.WriteLine();

        Console.Write($"  {UNDERLINE}    | ");
        for (int i = 0; i < g1.NumberOfVertices; i++)
        {
            Console.Write("       ");
        }
        Console.WriteLine(RESET_UNDERLINE);

        Console.ForegroundColor = DEFAULT_COLOR;

        for (int i = 0; i < g1.NumberOfVertices; i++)
        {
            Console.WriteLine($"      |");

            if (solutionVertices.Contains(i))
            {
                Console.ForegroundColor = SOLUTION_VERTEX_COLOR;

                Console.Write($"{i}".PadLeft(4));

                Console.ForegroundColor = DEFAULT_COLOR;
            }
            else
            {
                Console.Write($"{i}".PadLeft(4));
            }

            Console.Write("  | ");

            for (int j = 0; j < g1.NumberOfVertices; j++)
            {
                var weight = g1.GetWeight(i, j);
                if (weight == 0)
                {
                    Console.Write($"       ");
                }
                else if (solutionVertices.Contains(i) && solutionVertices.Contains(j))
                {
                    var solutionWeight = Math.Min(weight, l);

                    var col = $"|{weight - solutionWeight}";
                    var s = $"{solutionWeight}{col}".PadLeft(7);

                    Console.ForegroundColor = SOLUTION_EDGE_COLOR;

                    var idx = s.IndexOf(col);
                    Console.Write(s[..idx]);

                    Console.ForegroundColor = DEFAULT_COLOR;

                    Console.Write(s[idx..]);
                }
                else
                {
                    Console.Write($"{weight}".PadLeft(7));
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        var k = solutionVertices.Count;
        var empty = porosity * l * k * (k - 1) / (1 + porosity);
        Console.WriteLine($"Liczba łuków pustych: {Math.Round(empty)}");
        Console.WriteLine($"Liczba łuków w pełnej klice (k={k}, l={l}): {l * k * (k - 1)}");
    }

    /// <summary>
    /// Max common subgraph problem
    /// </summary>
    public static void DisplaySolution(this Graph g1, Graph g2, Dictionary<int, int> solutionVertices)
    {
        Console.WriteLine();
        
        Console.ForegroundColor = DEFAULT_COLOR;

        Console.Write($"      | ");
        for (int i = 0; i < g1.NumberOfVertices; i++)
        {
            if (solutionVertices.ContainsKey(i))
            {
                Console.ForegroundColor = SOLUTION_VERTEX_COLOR;

                Console.Write($"{i}".PadLeft(7));

                Console.ForegroundColor = DEFAULT_COLOR;
            }
            else
            {
                Console.Write($"{i}".PadLeft(7));
            }
        }
        Console.WriteLine();

        Console.Write($"  {UNDERLINE}    | ");
        for (int i = 0; i < g1.NumberOfVertices; i++)
        {
            Console.Write("       ");
        }
        Console.WriteLine(RESET_UNDERLINE);

        Console.ForegroundColor = DEFAULT_COLOR;

        for (int i = 0; i < g1.NumberOfVertices; i++)
        {
            Console.WriteLine($"      |");

            if (solutionVertices.ContainsKey(i))
            {
                Console.ForegroundColor = SOLUTION_VERTEX_COLOR;

                Console.Write($"{i}".PadLeft(4));

                Console.ForegroundColor = DEFAULT_COLOR;
            }
            else
            {
                Console.Write($"{i}".PadLeft(4));
            }

            Console.Write("  | ");

            for (int j = 0; j < g1.NumberOfVertices; j++)
            {
                var weight = g1.GetWeight(i, j);
                if (weight == 0)
                {
                    Console.Write($"       ");
                }
                else if (solutionVertices.TryGetValue(i, out var k) && solutionVertices.TryGetValue(j, out var l))
                {
                    var solutionWeight = Math.Min(weight, g2.GetWeight(k, l));

                    var col = $"|{weight - solutionWeight}";
                    var s = $"{solutionWeight}{col}".PadLeft(7);

                    Console.ForegroundColor = SOLUTION_EDGE_COLOR;
                    
                    var idx = s.IndexOf(col);
                    Console.Write(s[..idx]);

                    Console.ForegroundColor = DEFAULT_COLOR;

                    Console.Write(s[idx..]);
                }
                else
                {
                    Console.Write($"{weight}".PadLeft(7));
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
