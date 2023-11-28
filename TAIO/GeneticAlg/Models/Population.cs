
using System.Collections;
using System.Diagnostics;

namespace TAIO.GeneticAlg
{
    internal class IcreaseComparer : IComparer<CliqueGenome>
    {
        private readonly Func<CliqueGenome, double> FitnessFun;
        public IcreaseComparer(Func<CliqueGenome, double> fitness_fun) => FitnessFun = fitness_fun;

        public int Compare(CliqueGenome? x, CliqueGenome? y)
        {
            if (x is null || y is null) return 0;
            return (int)(FitnessFun(x) - FitnessFun(y));
        }
    }

    internal class DecreaseComparer : IComparer<CliqueGenome>
    {
        private readonly Func<CliqueGenome, double> FitnessFun;
        public DecreaseComparer(Func<CliqueGenome, double> fitness_fun) => FitnessFun = fitness_fun;

        public int Compare(CliqueGenome? x, CliqueGenome? y)
        {
            if (x is null || y is null) return 0;
            return (int)(FitnessFun(y) - FitnessFun(x));
        }
    }

    public class Population : IEnumerable<CliqueGenome>
    {
        private static int SizeOfBasePopulation => 10;
        private readonly Func<CliqueGenome, double> FitnessFun;

        public List<CliqueGenome> BasePopulation {  get; set; }
        public CliqueGenome? WorstCliqueGenome {  get; set; }
        public CliqueGenome? BestCliqueGenome {  get; set; }
        public bool WasBestCliqueGenomeChanged { get; set; }
        public int Count => BasePopulation.Count;

        public double ProbabilityOfSelectingIndividual = 0.2d;
        public double ProbalilityOfGeneModification = 0.2d;
        public int NrOfCrossingPoints = 2;

        public Population(Func<CliqueGenome, double> fitness_fun, double probability_of_selecting_individual = 0.2d, double probability_of_gene_modification = 0.2d, int nr_of_crossing_points = 2)
        {
            ProbabilityOfSelectingIndividual = probability_of_selecting_individual;
            ProbalilityOfGeneModification = probability_of_gene_modification;
            NrOfCrossingPoints = nr_of_crossing_points;

            FitnessFun = fitness_fun;
            WasBestCliqueGenomeChanged = false;
            BasePopulation = new List<CliqueGenome>(SizeOfBasePopulation);
        }

        public Population(IntermediateGraph graph, int clique_order, double porosity, Func<CliqueGenome, double> fitness_fun)
        {
            FitnessFun = fitness_fun;
            WasBestCliqueGenomeChanged = false;

            BasePopulation = new List<CliqueGenome>(SizeOfBasePopulation);
            var vertices = new RandomList(graph.NumberOfVertices);

            // iterate through genoms in base_polulation
            for (int i = 0; i < SizeOfBasePopulation; i++)
            {
                (var v_0, var A_set) = CreateCliqueWithoutEmptyEdges(vertices, graph, clique_order);

                var genome = TryToAddEmptyEdges(graph, A_set, v_0, clique_order, porosity);
                BasePopulation.Add(genome);

                // mutate genom
                GeneticOperations.TryToMutateGenome(genome);
            }
        }

        private static (Vertex, List<Vertex>) CreateCliqueWithoutEmptyEdges(
            RandomList vertices, 
            IntermediateGraph graph, 
            int clique_order)
        {
            var A_set = new List<Vertex>();
            var v_0 = vertices.GetRandomElement();
            A_set.Add(v_0);

            var neighbours = graph.GetAllNeighbours(v_0, clique_order);
            while (!neighbours.IsEmpty())
            {
                var v_j = neighbours.GetRandomElementOnce();
                var check_v_j = true;

                foreach (var vertex in A_set)
                {
                    if (!graph.HasEdge(v_j, vertex, clique_order))
                    {
                        check_v_j = false;
                        break;
                    }
                }

                if (check_v_j)
                {
                    A_set.Add(v_j);
                }
            }

            return (v_0, A_set);
        }

        private static CliqueGenome TryToAddEmptyEdges(
            IntermediateGraph graph,
            List<Vertex> A_set,
            Vertex v_0,
            int clique_order,
            double porosity)
        {
            // add elements according p-parameter to genom
            // modify Genom class to support p-parameter per Genom
            var B_set = graph.GetAllNeighboursWithout(v_0, clique_order, A_set);
            int empty_arches_in_genom = 0;
            var empty_arches_tracker = new HashSet<(Arch, int)>();

            while (!B_set.IsEmpty())
            {
                var u_vertex = B_set.GetRandomElementOnce();

                var empty_arches_tracker_for_u = new HashSet<(Arch, int)>();
                int empty_arches_for_u = 0;
                int not_empty_arches_for_u = 0;

                foreach (var v_vertex in A_set)
                {
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

                int existed_arches = (A_set.Count * (A_set.Count - 1)) * clique_order - empty_arches_in_genom;
                if (existed_arches == 0 && not_empty_arches_for_u == 0)
                {
                    break;
                }

                if ((((double)empty_arches_in_genom + empty_arches_for_u) / (double)(existed_arches + not_empty_arches_for_u)) < porosity)
                {
                    empty_arches_in_genom += empty_arches_for_u;
                    A_set.Add(u_vertex);
                    empty_arches_tracker.UnionWith(empty_arches_tracker_for_u);
                }
            }

            // convert A_set to genom
            return new CliqueGenome(graph, clique_order, A_set, empty_arches_tracker);
        }

        public void StartTrackingBestAndWorstCliqueGenome()
        {
            var comparer = new IcreaseComparer(FitnessFun);
            BestCliqueGenome = BasePopulation.Max(comparer);
            WorstCliqueGenome = BasePopulation.Min(comparer);

            Debug.Assert(
                BestCliqueGenome is not null && WorstCliqueGenome is not null,
                "The initial best/worst clique gene is null. Something went wrong!"
            );
        }

        public void TrackingBestAndWorstCliqueGenome()
        {
            var comparer = new IcreaseComparer(FitnessFun);
            var best_clique_genome = BasePopulation.Max(comparer);
            WorstCliqueGenome = BasePopulation.Min(comparer);

            if (best_clique_genome != BestCliqueGenome)
                WasBestCliqueGenomeChanged = true;

            BestCliqueGenome = best_clique_genome;
            if (BestCliqueGenome is null || WorstCliqueGenome is null)
                throw new Exception("The initial best/worst clique gene is null. Something went wrong!");
        }

        public void ExchagneGenomes(CliqueGenome? old_genome, CliqueGenome new_genome)
        {
            WasBestCliqueGenomeChanged = false;
            if (old_genome is null) return;
            if (FitnessFun(new_genome) > FitnessFun(BestCliqueGenome!))
            {
                BestCliqueGenome = new_genome;
                WasBestCliqueGenomeChanged = true;
            }

            BasePopulation.Remove(old_genome);
            BasePopulation.Add(new_genome);
        }

        public IEnumerator<CliqueGenome> GetEnumerator()
        {
            foreach (var genome in BasePopulation)
            {
                yield return genome;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public CliqueGenome this[int idx]
        {  
            get { return BasePopulation[idx]; }
            set { BasePopulation[idx] = value; }
        }
   
        public void AddCliqueGenome(CliqueGenome genome)
        {
            BasePopulation.Add(genome);
        }
    
        public static List<Population> DividePopulation(Population base_population)
        {
            var list = new List<Population>();

            var comparer = new DecreaseComparer(base_population.FitnessFun);
            base_population.BasePopulation.Sort(comparer);

            int number_of_population = base_population.Count / 3;
            list.Add(new Population(base_population.FitnessFun, 0.2, 0.2, 2));
            for (var index = 0; index < number_of_population; index++)
            {
                list[0].AddCliqueGenome(base_population[index]);
            }

            list.Add(new Population(base_population.FitnessFun, 0.4, 0.4, 2));
            list.Add(new Population(base_population.FitnessFun, 0.2, 0.2, 4));
            var index_list = new RandomList(base_population.Count, (base_population.Count + 1) / 3);
            while (!index_list.IsEmpty())
            {
                var index1 = index_list.GetRandomElementOnce();
                list[1].AddCliqueGenome(base_population[index1]);

                if (index_list.IsEmpty()) break;

                var index2 = index_list.GetRandomElementOnce();
                list[2].AddCliqueGenome(base_population[index2]);
            }

            list[0].StartTrackingBestAndWorstCliqueGenome();
            list[1].StartTrackingBestAndWorstCliqueGenome();
            list[2].StartTrackingBestAndWorstCliqueGenome();

            return list;
        }
    
        public static void MigrationMechanism(Population dst_population, Population src_population, Func<CliqueGenome, double> fitness_fun)
        {
            double score = fitness_fun(src_population.BestCliqueGenome!);
            for (int i = 0; i < dst_population.Count; i++)
            {
                if (fitness_fun(dst_population[i]) < score)
                {
                    (dst_population[i], src_population.BestCliqueGenome!) = (src_population.BestCliqueGenome!, dst_population[i]);
                    src_population.TrackingBestAndWorstCliqueGenome();
                    dst_population.TrackingBestAndWorstCliqueGenome();

                    return;
                }
            }
        }
    }
}
