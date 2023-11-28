namespace TAIO.GeneticAlg
{
    public class IntermediateGraph
    {
        public int[] Mapping 
        { 
            get;
            private set;
        }
        public int[] Unmapping
        { 
            get; 
            private set;
        }

        private int[,] AdjustmentMatrix { get; set; }

        public int this[int index_u, int index_v]
        {
            get
            {
                var index_uu = Unmapping[index_u];
                var index_vv = Unmapping[index_v];

                return AdjustmentMatrix[index_uu, index_vv];
            }

            set
            {
                var index_uu = Unmapping[index_u];
                var index_vv = Unmapping[index_v];

                AdjustmentMatrix[index_uu, index_vv] = value;
            }

        }

        public int NumberOfVertices => AdjustmentMatrix.GetLength(0);

        public IntermediateGraph(int[,] adjustmentMatrix)
        {
            AdjustmentMatrix = adjustmentMatrix;
            Mapping = new int[NumberOfVertices];
            Unmapping = new int[NumberOfVertices];

            //                   (v,|N(v)|)
            var neighbours = new (int, int)[NumberOfVertices];

            for (int u = 0; u < NumberOfVertices; u++)
            {
                for (int v = 0; v < NumberOfVertices; v++)
                {
                    if (u == v) continue;

                    (_, var value) = neighbours[u];
                    neighbours[u] = (u, Math.Min(AdjustmentMatrix[u, v], AdjustmentMatrix[v, u]) + value);
                }
            }

            Array.Sort(neighbours, (p1, p2) => p2.Item2 - p1.Item2);
            for (int i = 0; i < NumberOfVertices; i++)
            {
                Mapping[neighbours[i].Item1] = i;
                Unmapping[i] = neighbours[i].Item1;
            }
        }

        public RandomList GetAllNeighbours(int vertex, int weight)
        {
            var neighbours = new List<Vertex>();
            for (int i = 0; i < NumberOfVertices; i++)
            {
                if (this[vertex, i] >= weight && this[i, vertex] >= weight)
                {
                    neighbours.Add(i);
                }
            }

            return new RandomList(neighbours);
        }

        public RandomList GetAllNeighboursWithout(Vertex vertex, int weight, List<Vertex> without_vertices)
        {
            var neighbours = new List<Vertex>();
            for (int i = 0; i < NumberOfVertices; i++)
            {
                if (this[vertex, i] >= weight && this[i, vertex] >= weight && !without_vertices.Contains(i))
                {
                    neighbours.Add(i);
                }
            }

            return new RandomList(neighbours);
        }

        public bool HasEdge(int u, int v, int weight)
        {
            return this[u, v] >= weight && this[v, u] >= weight;
        }
    }
}
