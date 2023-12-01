using TAIO;
using TAIO.GeneticAlg;
using TAIO.Parsers;

int[,] A =
{
    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0 },
    { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0 },
    { 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0 },
    { 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0 },
    { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
};

int[,] B =
{
    { 0, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
    { 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0 },
    { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
};

//int[,] A =
//{
//    { 0, 1, 0, 1, 0},
//    { 1, 0, 1, 1, 1},
//    { 0, 1, 0, 1, 1},
//    { 2, 1, 2, 0, 0},
//    { 0, 1, 1, 0, 0},
//};

//int[,] B =
//{
//    { 0, 2, 1, 0},
//    { 1, 0, 1, 0},
//    { 1, 1, 0, 1},
//    { 0, 0, 1, 0},
//};

//int[,] A =
//{
//    { 0, 0, 0, 0, 0},
//    { 0, 0, 0, 0, 0},
//    { 0, 0, 0, 0, 0},
//    { 0, 0, 0, 0, 0},
//    { 0, 0, 0, 0, 0},
//};

//int[,] B =
//{
//    { 0, 0, 0, 0},
//    { 0, 0, 0, 0},
//    { 0, 0, 0, 0},
//    { 0, 0, 0, 0},
//};

//int[,] A =
//{
//    { 0, 1, 0, 0, 0},
//    { 0, 0, 0, 1, 0},
//    { 0, 0, 0, 0, 1},
//    { 0, 1, 1, 0, 0},
//    { 0, 1, 0, 1, 0},
//};

//int[,] B =
//{
//    { 0, 0, 0, 0, 0, 0},
//    { 0, 0, 0, 0, 0, 1},
//    { 0, 0, 0, 0, 1, 0},
//    { 0, 1, 0, 0, 1, 0},
//    { 0, 0, 0, 1, 0, 0},
//    { 0, 0, 0, 0, 1, 0},
//};

#region Maximum common subgraph

//var gA = new Graph(A);
//var gB = new Graph(B);

//var mapping = MaxCommonSubgraph.FindApprox(gA, gB);

//gA.DisplaySolution(gB, mapping);
//gB.DisplaySolution(gA, mapping.ToDictionary(x => x.Value, y => y.Key));

#endregion

#region Maximum clique

//var l = 1;
//var p = 0.5;

//(var porosity, var clique) = MaxClique.Calculate(new IntermediateGraph(A), l, p, (genome) => genome.NumberOfVertices);

//var g = new Graph(A);
//g.DisplaySolution(clique, l, porosity);

#endregion

string fileName = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "asia_test.txt");

try
{
    Graph[] graphs = Parser.ParseFile(fileName);

    foreach (var graph in graphs)
    {
        Console.WriteLine($"Liczba wierzchołków: {graph.NumberOfVertices}");
        Console.WriteLine("Macierz sąsiedztwa:");

        for (int i = 0; i < graph.NumberOfVertices; i++)
        {
            for (int j = 0; j < graph.NumberOfVertices; j++)
            {
                Console.Write($"{graph.AdjustmentMatrix[i, j]} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
}