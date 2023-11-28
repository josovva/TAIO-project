
using System.Runtime.CompilerServices;

namespace TAIO.GeneticAlg
{
    public class MaxClique
    {
        private static int StagnationFactor => 20;

        public static (double,List<int>) Calculate(IntermediateGraph graph, int clique_order, double porosity, Func<CliqueGenome, double> fitness_fun)
        {
            var population = new Population(graph, clique_order, porosity, fitness_fun);
            foreach (var genome in population)
            {
                LocalOptimisation.LocalOptimisationGenom(genome, graph, clique_order, porosity);
            }

            population.StartTrackingBestAndWorstCliqueGenome();

            // divide to subpopulation

            int stagnation_counter = 0;
            do
            {
                // foreach P_i in {P_a, P_b, P_c}
                // {
                (var p1_genome, var p2_genome) = GeneticOperations.SelectParesntGenomes(population, fitness_fun);
                (var o1_genome, var o2_genome) = p1_genome + p2_genome;

                GeneticOperations.TryToMutateGenome(o1_genome);
                GeneticOperations.TryToMutateGenome(o2_genome);

                LocalOptimisation.LocalOptimisationGenom(o1_genome, graph, clique_order, porosity);
                LocalOptimisation.LocalOptimisationGenom(o2_genome, graph, clique_order, porosity);

                GeneticOperations.GenomeNaturalSelection(population, o1_genome, o2_genome, p1_genome, p2_genome, fitness_fun);

                if (!population.WasBestCliqueGenomeChanged)
                    stagnation_counter++;
                else
                    stagnation_counter = 0;
                // }

            } while (stagnation_counter < StagnationFactor);

            return (population.BestCliqueGenome!.Porosity, population.BestCliqueGenome!.GetClique());
        }

        public static (double, List<int>) CalculateWithMigration(IntermediateGraph graph, int clique_order, double porosity, Func<CliqueGenome, double> fitness_fun)
        {
            var base_population = new Population(graph, clique_order, porosity, fitness_fun);
            foreach (var genome in base_population)
            {
                LocalOptimisation.LocalOptimisationGenom(genome, graph, clique_order, porosity);
            }

            base_population.StartTrackingBestAndWorstCliqueGenome();

            // divide to subpopulation
            var populations = Population.DividePopulation(base_population);

            int stagnation_counter = 0;
            do
            {
                foreach(var population in  populations)
                {
                    (var p1_genome, var p2_genome) = GeneticOperations.SelectParesntGenomes(population, fitness_fun);
                    (var o1_genome, var o2_genome) = p1_genome + p2_genome;

                    GeneticOperations.TryToMutateGenome(o1_genome);
                    GeneticOperations.TryToMutateGenome(o2_genome);

                    LocalOptimisation.LocalOptimisationGenom(o1_genome, graph, clique_order, porosity);
                    LocalOptimisation.LocalOptimisationGenom(o2_genome, graph, clique_order, porosity);

                    GeneticOperations.GenomeNaturalSelection(population, o1_genome, o2_genome, p1_genome, p2_genome, fitness_fun);
                }

                // migrate between population
                Population.MigrationMechanism(populations[0], populations[1], fitness_fun);
                Population.MigrationMechanism(populations[0], populations[2], fitness_fun);

                if (!populations[0].WasBestCliqueGenomeChanged)
                    stagnation_counter++;
                else
                    stagnation_counter = 0;
            } while (stagnation_counter < StagnationFactor);

            return (populations[0].BestCliqueGenome!.Porosity, populations[0].BestCliqueGenome!.GetClique());
        }
    }


}
