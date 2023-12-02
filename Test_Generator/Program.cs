using System.Text;
using TAIO;
using TAIO.GeneticAlg;
using Test_Generator;

var n = 10;
var k = 6;
var l = 3;
var p = 0.1;

var g = new int[n, n];

var r = new Randomer(n, k).Enumerate().ToList();
foreach (var i in r)
{
    foreach (var j in r)
    {
        if (i != j)
        {
            g[i, j] = l;
        }
    }
}

var result = new Graph(g);
for (var m = 0; m < new Random().Next((k + 4) * (k + 3) * l, n * (n - 1) * 2 * l - k * (k - 1) * l); m++)
{
    var i = new Random().Next(0, n);
    var j = new Random().Next(0, n);
    while (i == j)
    {
        j = new Random().Next(0, n);
    }

    result.AddEdge(i, j);
}

var maxEdgeWeight = 0;
for (var i = 0; i < result.NumberOfVertices; ++i)
{
    for (var j = 0; j < result.NumberOfVertices; ++j)
    {
        if (result.GetWeight(i, j) > maxEdgeWeight)
            maxEdgeWeight = result.GetWeight(i, j);
    }
}
var (porosity, clique) = MaxClique.Calculate(new IntermediateGraph(g), l, p, (genome) => genome.NumberOfVertices + (double)genome.TotalEdgeWeight / (n * (n - 1) * maxEdgeWeight));

result.DisplaySolution(r, l, 0);
result.DisplaySolution(clique, l, porosity);

Console.ReadKey();

using var writer = new StreamWriter("./../../../../TAIO/Tests/Clique_Test1.txt", false);
writer.WriteLine(1);
writer.WriteLine(n);
for (var i = 0; i < n; ++i)
{
    var sb = new StringBuilder();

    for (int j = 0; j < n; ++j)
    {
        sb.Append($"{g[i, j]} ");
    }

    writer.WriteLine(sb.ToString().TrimEnd(' '));
}