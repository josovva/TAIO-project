using System.Collections;
using System.Diagnostics;

namespace TAIO.MaxCliqueAlg
{
    public class CliqueGenome : IEnumerable<Vertex>, ICloneable
    {
        public IntermediateGraph BaseGraph { get; set; }
        public bool[] Genes { get; set; }
        public HashSet<(Arch, int)> EmptyArches { get; set; }
        public int NumberOfVertices { get; set; }
        public int Order { get; set; }

        public int Length => Genes.Length;

        public int TotalEdgeWeight
        {
            get
            {
                var sum = 0;

                foreach (var u in this)
                {
                    //Debug.Assert(BaseGraph[u, u] == 0, "Should be always zero");

                    foreach (var v in this)
                    {
                        if (u != v)
                        {
                            sum += BaseGraph[u, v];
                        }
                    }
                }

                return sum;
            }
        }

        public double Porosity
        {
            get
            {
                if (NumberOfVertices == 1) return 0;

                int empty_arches_in_genom = CountEmptyArches();
                int existed_arches = NumberOfVertices * (NumberOfVertices - 1) * Order - empty_arches_in_genom;
                if (existed_arches <= 0) return 1;


                return Math.Min(empty_arches_in_genom / (double)existed_arches, 1);

            }
        }

        public bool this[int idex]
        {
            get => Genes[idex];
            set
            {
                if (!Genes[idex] && value) { NumberOfVertices += 1; }
                else if (Genes[idex] && !value) { NumberOfVertices -= 1; }
                Genes[idex] = value;
            }
        }

        public CliqueGenome(IntermediateGraph graph, int order)
        {
            Order = order;
            BaseGraph = graph;
            Genes = new bool[graph.NumberOfVertices];
            EmptyArches = new();

            Array.Fill(Genes, false);
            NumberOfVertices = 0;
        }

        public CliqueGenome(IntermediateGraph graph, int order, List<Vertex> vertices, HashSet<(Arch, int)> empty_arches)
        {
            Order = order;
            BaseGraph = graph;
            Genes = new bool[graph.NumberOfVertices];
            EmptyArches = empty_arches;

            Array.Fill(Genes, false);
            NumberOfVertices = 0;

            foreach (int v in vertices)
            {
                this[v] = true;
            }
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            for (Vertex vertex = 0; vertex < Genes.Length; vertex++)
            {
                if (Genes[vertex])
                {
                    yield return vertex;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void NegateGene(int gen)
        {
            if (this[gen])
            {
                EmptyArches.RemoveWhere(
                    (arch_count) => arch_count.Item1.u_vertex == gen || arch_count.Item1.v_vertex == gen
                );
            }
            else
            {
                int u_vertex = gen;
                foreach (var v_vertex in this)
                {
                    int u_v = Math.Min(BaseGraph[u_vertex, v_vertex], Order);
                    int v_u = Math.Min(BaseGraph[v_vertex, u_vertex], Order);

                    if (u_v < Order)
                    {
                        EmptyArches.Add(((u_vertex, v_vertex), Order - u_v));
                    }

                    if (v_u < Order)
                    {
                        EmptyArches.Add(((v_vertex, u_vertex), Order - v_u));
                    }
                }
            }

            this[gen] = !this[gen];
        }

        public void UnionEmptyEdges(HashSet<(Arch, int)> empty_edges)
        {
            EmptyArches.UnionWith(empty_edges);
        }

        public int CountEmptyArches()
        {
            int sum = 0;
            foreach ((var arch, var count) in EmptyArches)
            {
                sum += count;
            }
            return sum;
        }

        public static (CliqueGenome, CliqueGenome) operator +(CliqueGenome genome1, CliqueGenome genome2)
        {
            (var o1_genome, var o2_genome) = GeneticOperations.CreateOffsprintByCrossing(genome1, genome2);
            o1_genome.TrackEmptyArches();
            o2_genome.TrackEmptyArches();
            return (o1_genome, o2_genome);
        }

        public List<int> GetClique()
        {
            List<int> clique = new();
            foreach (var vertex in this)
            {
                clique.Add(BaseGraph.Unmapping[vertex]);
            }

            return clique;
        }

        public void TrackEmptyArches()
        {
            EmptyArches.Clear();
            foreach (var u_vertex in this)
            {
                foreach (var v_vertex in this)
                {
                    if (u_vertex == v_vertex) continue;
                    if (!BaseGraph.HasEdge(u_vertex, v_vertex, Order))
                    {
                        int u_v = Math.Min(BaseGraph[u_vertex, v_vertex], Order);

                        if (u_v < Order)
                        {
                            EmptyArches.Add(((u_vertex, v_vertex), Order - u_v));
                        }
                    }
                }
            }
        }

        public object Clone()
        {
            var tmp = new HashSet<(Arch, int)>();
            foreach ((var Arch, var count) in EmptyArches)
            {
                tmp.Add(((Arch.u_vertex, Arch.v_vertex), count));
            }

            var list = new List<Vertex>();
            foreach (var elem in this)
            {
                list.Add(elem);
            }


            return new CliqueGenome(BaseGraph, Order, list, tmp);
        }
    }
}
