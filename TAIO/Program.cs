﻿using TAIO;
using TAIO.CommonSubgraph;

//int[,] A =
//{
//    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//    { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
//    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//    { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
//    { 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0 },
//    { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
//    { 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0 },
//    { 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0 },
//    { 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0 },
//    { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1 },
//    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
//    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
//};

//int[,] B =
//{
//    { 0, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
//    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//    { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
//    { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
//    { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
//    { 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0 },
//    { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0 },
//    { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0 },
//    { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
//    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1 },
//    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
//    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
//};

int[,] A =
{
    { 0, 1, 0, 1, 0},
    { 1, 0, 1, 1, 1},
    { 0, 1, 0, 1, 1},
    { 2, 1, 2, 0, 0},
    { 0, 1, 1, 0, 0},
};

int[,] B =
{
    { 0, 2, 1, 0},
    { 1, 0, 1, 0},
    { 1, 1, 0, 1},
    { 0, 0, 1, 0},
};

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

MaxCommonSubgraph.FindApprox(new Graph(A), new Graph(B));

//var n = A.GetLength(0);
//var maxEdgeWeight = Enumerable.Range(0, n * n).Aggregate(0, (y, x) =>
//{
//    var temp = A[x / n, x % n];
//    return y < temp ? temp : y;
//});

//var test = new TAIO.GeneticAlg.IntermediateGraph(A);
//(var porosity, var clique) = TAIO.GeneticAlg.MaxClique.Calculate(test, 1, 0, (genome) => genome.NumberOfVertices + (double)genome.TotalEdgeWeight / (n * (n - 1) * maxEdgeWeight));

//foreach (var v in clique)
//{
//    Console.Write($"{v} ");
//}
//Console.Write("\n");
//Console.WriteLine($"Porosity {porosity}");