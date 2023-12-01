﻿using TAIO;
using TAIO.GeneticAlg;

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

var l = 1;
var p = 0.5;

(var porosity, var clique) = MaxClique.Calculate(new IntermediateGraph(A), l, p, (genome) => genome.NumberOfVertices);

var g = new Graph(A);
g.DisplaySolution(clique, l, porosity);

#endregion