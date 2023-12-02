
using System.Diagnostics;

namespace TAIO.MaxCliqueAlg
{
    public class LocalOptimisation
    {
        private static readonly Random Rnd = new();

        public static void LocalOptimisationGenom(CliqueGenome genome, IntermediateGraph graph, int clique_order, double porosity)
        {
            ModifiedBuiEpleyExtraction(genome, graph, clique_order, porosity);
            Debug.Assert(genome.Porosity <= porosity, "Bui-Epley extraction doesn't work!");

            CliqueRepairing(genome, graph, clique_order, porosity);
            Debug.Assert(genome.Porosity <= porosity, "Clique reparing doesn't work!");
        }

        private static void ModifiedBuiEpleyExtraction(CliqueGenome genome, IntermediateGraph graph, int clique_order, double porosity)
        {
            Vertex tracked_vertex = 0;
            int max_nr_empty_arches = -1;

            while (genome.Porosity > porosity)
            {
                foreach (var u_vertex in genome)
                {
                    int empty_arches_u = 0;
                    foreach (var v_vertex in genome)
                    {
                        if (u_vertex == v_vertex) continue;

                        int u_v = Math.Min(graph[u_vertex, v_vertex], clique_order);
                        int v_u = Math.Min(graph[v_vertex, u_vertex], clique_order);
                        int empty_arches_u_v = clique_order * 2 - u_v - v_u;

                        empty_arches_u += empty_arches_u_v;
                    }

                    if (empty_arches_u > max_nr_empty_arches)
                    {
                        max_nr_empty_arches = empty_arches_u;
                        tracked_vertex = u_vertex;
                    }
                }
                max_nr_empty_arches = -1;

                genome.NegateGene(tracked_vertex);
            }
        }

        private static void CliqueRepairing(CliqueGenome genome, IntermediateGraph graph, int clique_order, double porosity)
        {
            if (genome.Porosity == 0)
                GeneralCliqueRepairing(genome, graph, clique_order, 0f);

            if (porosity != 0)
                GeneralCliqueRepairing(genome, graph, clique_order, porosity);
        }

        private static void GeneralCliqueRepairing(CliqueGenome genome, IntermediateGraph graph, int clique_order, double porosity)
        {
            int pivot_point = Rnd.Next(0, genome.Length);
            for (int u_vertex = pivot_point; u_vertex < genome.Length; u_vertex++)
            {
                if (genome.NumberOfVertices == 0)
                {
                    genome[u_vertex] = true;
                    continue;
                }

                if (genome[u_vertex] == false)
                {
                    TryToAddGeneToClique(u_vertex, genome, graph, clique_order, porosity);
                }
            }

            for (int u_vertex = 0; u_vertex < pivot_point; u_vertex++)
            {
                if (genome[u_vertex] == false)
                {
                    TryToAddGeneToClique(u_vertex, genome, graph, clique_order, porosity);
                }
            }

        }

        private static void TryToAddGeneToClique(int u_vertex, CliqueGenome genome, IntermediateGraph graph, int clique_order, double porosity)
        {
            var empty_arches_tracker_for_u = new HashSet<(Arch, int)>();
            int empty_arches_for_u = 0;
            int not_empty_arches_for_u = 0;

            foreach (var v_vertex in genome)
            {
                if (v_vertex == u_vertex) continue;

                int u_v = Math.Min(graph[u_vertex, v_vertex], clique_order);
                int v_u = Math.Min(graph[v_vertex, u_vertex], clique_order);
                int empty_arches_for_u_v = clique_order * 2 - u_v - v_u;
                not_empty_arches_for_u += clique_order * 2 - empty_arches_for_u_v;

                if (u_v < clique_order)
                {
                    empty_arches_tracker_for_u.Add(((u_vertex, v_vertex), clique_order - u_v));
                }

                if (v_u < clique_order)
                {
                    empty_arches_tracker_for_u.Add(((v_vertex, u_vertex), clique_order - v_u));
                }

                empty_arches_for_u += empty_arches_for_u_v;
            }

            int empty_arches_in_genom = genome.CountEmptyArches();
            int existed_arches = (genome.NumberOfVertices * (genome.NumberOfVertices - 1)) * clique_order - empty_arches_in_genom;
            if (existed_arches == 0 && not_empty_arches_for_u == 0)
                return;

            if ((((double)empty_arches_in_genom + empty_arches_for_u) / (double)(existed_arches + not_empty_arches_for_u)) <= porosity)
            {
                genome[u_vertex] = true;
                genome.UnionEmptyEdges(empty_arches_tracker_for_u);
            }
        }
    }
}
