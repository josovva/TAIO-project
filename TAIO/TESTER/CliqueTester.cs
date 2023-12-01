
namespace TAIO
{
    public class CliqueTester
    {
        public static bool IsThisCliqueCorrect(List<int> clique, int[,] matrix, int clique_order = 1, int porosity = 0)
        {
            int empty_arches = 0;
            int non_empty_arches = 0;

            foreach (var u_vertex in clique)
            {
                foreach (var v_vertex in clique)
                {
                    if (u_vertex == v_vertex) continue;

                    int u_v = Math.Min(matrix[u_vertex, v_vertex], clique_order);
                    empty_arches += clique_order - u_v;
                    non_empty_arches += u_v;
                }
            }

            return (empty_arches / (double)non_empty_arches) <= porosity;
        }
    }
}
