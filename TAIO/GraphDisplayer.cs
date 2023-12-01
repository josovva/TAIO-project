namespace TAIO;

public static class GraphDisplayer
{
    const string UNDERLINE = "\x1B[4m";
    const string RESET_UNDERLINE = "\x1B[0m";

    const ConsoleColor SOLUTION_VERTEX_COLOR = ConsoleColor.Blue;
    const ConsoleColor SOLUTION_EDGE_COLOR = ConsoleColor.Blue;
    private const ConsoleColor DEFAULT_COLOR = ConsoleColor.DarkGray;

    public static void DisplaySolution(this Graph g, List<int> solutionVertices)
    {
        Console.ForegroundColor = DEFAULT_COLOR;

        Console.Write($"     | ");
        for (int i = 0; i < g.NumberOfVertices; i++)
        {
            if (solutionVertices.Contains(i))
            {
                Console.ForegroundColor = SOLUTION_VERTEX_COLOR;

                Console.Write(i < 10 ? $" {i} " : $"{i} ");

                Console.ForegroundColor = DEFAULT_COLOR;
            }
            else
            {
                Console.Write(i < 10 ? $" {i} " : $"{i} ");
            }
        }
        Console.WriteLine();

        Console.Write($"  {UNDERLINE}   |");
        for (int i = 0; i < g.NumberOfVertices; i++)
        {
            Console.Write("   ");
        }
        Console.WriteLine(RESET_UNDERLINE);

        Console.ForegroundColor = DEFAULT_COLOR;

        for (int i = 0; i < g.NumberOfVertices; i++)
        {
            if (solutionVertices.Contains(i))
            {
                Console.ForegroundColor = SOLUTION_VERTEX_COLOR;

                Console.Write(i < 10 ? $"  {i}" : $" {i}");

                Console.ForegroundColor = DEFAULT_COLOR;
            }
            else
            {
                Console.Write(i < 10 ? $"  {i}" : $" {i}");
            }

            Console.Write("  | ");

            for (int j = 0; j < g.NumberOfVertices; j++)
            {
                var weight = g.GetWeight(i, j);
                if (weight == 0)
                {
                    Console.Write($"   ");
                }
                else if (solutionVertices.Contains(i) && solutionVertices.Contains(j))
                {
                    Console.ForegroundColor = SOLUTION_EDGE_COLOR;

                    Console.Write(weight < 10 ? $" {weight} " : $"{weight} ");

                    Console.ForegroundColor = DEFAULT_COLOR;
                }
                else
                {
                    Console.Write(weight < 10 ? $" {weight} " : $"{weight} ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
