
using TAIO.MaxCliqueAlg;

namespace TAIO
{
    public class Size
    {
        public static double FitnessFunctionMaxClique(CliqueGenome genome)
        {
            if (genome.BaseGraph.MaxEdgeWeigh == 0) return genome.NumberOfVertices;

            var n = genome.BaseGraph.NumberOfVertices;
            return genome.NumberOfVertices + (double)genome.TotalEdgeWeight / (n * (n - 1) * genome.BaseGraph.MaxEdgeWeigh);
        }
    }
}
