
using System.Collections;
using System.Diagnostics;

namespace TAIO.MaxCliqueAlg
{
    public class GeneticOperations
    {
        private static readonly Random Rnd = new();
        public static double ProbabilityOfSelectingIndividual = 0.2d;
        public static double ProbalilityOfGeneModification = 0.2d;
        public static int NrOfCrossingPoints = 2;

        private static readonly double SimilarityRadius = 2;

        public static void TryToMutateGenome(CliqueGenome genome)
        {
            int probability_select = Rnd.Next(0, 101);

            if (ProbabilityOfSelectingIndividual * 100 >= probability_select)
            {
                for (int i = 0; i < genome.Length; i++)
                {
                    int probability_modify = Rnd.Next(0, 101);
                    if (ProbalilityOfGeneModification * 100 >= probability_modify)
                    {
                        genome.NegateGene(i);
                    }
                }
            }
        }

        public static (CliqueGenome, CliqueGenome) SelectParesntGenomes(Population base_population, Func<CliqueGenome, double> fitness_fun)
        {
            // roulette build
            double fintess_sharing_fun(CliqueGenome genome)
            {
                var nominator = fitness_fun(genome);
                var denominator = NicheSize(base_population, genome);
                return nominator / denominator;
            }
            int p1_idex = -1, p2_idex = -1;

            List<double> fitness_scores = base_population.Select(fintess_sharing_fun).ToList();
            double total_fittnes = fitness_scores.Sum();

            List<double> probabilities = fitness_scores.Select(score => score / total_fittnes).ToList();

            // roulette start
            double p1_probability = Rnd.NextDouble();
            double p2_probability = Rnd.NextDouble();

            double cumulate_prop1 = 0, cumulate_prop2 = 0;
            for (int i = 0; i < base_population.Count; i++)
            {
                cumulate_prop1 += probabilities[i];
                cumulate_prop2 += probabilities[i];

                if (p1_probability <= cumulate_prop1) p1_idex = i;
                if (p2_probability <= cumulate_prop2) p2_idex = i;

                if (p1_idex != -1 && p2_idex != -1) break;
            }

            if (p1_idex == p2_idex)
            {
                p2_idex = (p2_idex + 1) % base_population.Count;
            }

            // get parents
            return (base_population[p1_idex], base_population[p2_idex]);
        }

        private static double NicheSize(Population base_population, CliqueGenome individual)
        {
            double niche_size = 0;
            foreach (var indi in base_population)
            {
                niche_size += MeasureOfGenomeSimilarity(
                    HammingDistance(
                        individual,
                        indi
                    )
                );

            }

            Debug.Assert(!Double.IsInfinity(niche_size), "The niche size is infinit!");
            Debug.Assert(niche_size > 0, "The niche size is not greater than zero!");
            Debug.Assert(!Double.IsNaN(niche_size), "The niche size is NaN!");

            return niche_size;
        }

        private static double MeasureOfGenomeSimilarity(double distance_between_i_j)
        {
            if (distance_between_i_j < SimilarityRadius) return 1 - ( distance_between_i_j / SimilarityRadius);
            else return 0;
        }

        private static int HammingDistance(CliqueGenome individual_i, CliqueGenome individual_j)
        {
            Debug.Assert(individual_i.Length == individual_j.Length, "Genomes must have the same length for Hamming distance!");

            int distance_counter = 0;
            for (int i = 0; i < individual_i.Length; i++)
            {
                if (individual_i[i] != individual_j[i])
                {
                    distance_counter += 1;
                }
            }

            return distance_counter;
        }

        public static (CliqueGenome, CliqueGenome) CreateOffsprintByCrossing(CliqueGenome p1_genome, CliqueGenome p2_genome)
        {
            Queue<int> crossing_points = new();
            CliqueGenome p1 = p1_genome, p2 = p2_genome;
            var possible_crossing_points = new RandomList(p1_genome.Length - 1);

            for (int i = 0; i < NrOfCrossingPoints; i++)
            {
                var idx = possible_crossing_points.GetRandomElementOnce();
                crossing_points.Enqueue(idx);
            }

            CliqueGenome o1_genome = new(p1_genome.BaseGraph, p1_genome.Order), o2_genome = new(p1_genome.BaseGraph, p1_genome.Order);
            for (int i = 0; i < p1_genome.Length; i++)
            {
                o1_genome[i] = p1[i];
                o2_genome[i] = p2[i];
                if (crossing_points.Count != 0 && i == crossing_points.Peek())
                {
                    crossing_points.Dequeue();
                    (p2, p1) = (p1, p2); // swap parents
                }
            }

            return (o1_genome, o2_genome);
        }

        public static void GenomeNaturalSelection(Population base_population, CliqueGenome o1_genome, CliqueGenome o2_genome, CliqueGenome p1_genome, CliqueGenome p2_genome, Func<CliqueGenome, double> fitness_fun)
        {
            var score_1 = fitness_fun(o1_genome);
            var score_2 = fitness_fun(o2_genome);

            var better_offspring = o1_genome;
            var better_score = score_1;
            if (score_2 > score_1)
            {
                better_offspring = o2_genome;
                better_score = score_2;
            }

            var distance_offsprint_p1 = HammingDistance(better_offspring, p1_genome);
            var distance_offsprint_p2 = HammingDistance(better_offspring, p2_genome);

            var more_similar_parrent = p1_genome;
            var less_similar_parrent = p1_genome;
            if (distance_offsprint_p2 < distance_offsprint_p1) (more_similar_parrent, less_similar_parrent) = (p2_genome, p1_genome);

            CliqueGenome? to_replace_genome = null;
            if (better_score > fitness_fun(more_similar_parrent))
            {
                to_replace_genome = more_similar_parrent;
            }
            else if (better_score > fitness_fun(less_similar_parrent))
            {
                to_replace_genome = less_similar_parrent;
            }
            else if (better_score > fitness_fun(base_population.WorstCliqueGenome!))
            {
                to_replace_genome = base_population.WorstCliqueGenome;
            }

            base_population.ExchagneGenomes(to_replace_genome, better_offspring);
        }
    }
}
