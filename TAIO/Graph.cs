namespace TAIO
{
    public class Graph
    {
        public int[,] AdjustmentMatrix { get; private set; }
        public int NumberOfVertices => AdjustmentMatrix.GetLength(0);
        public Graph(int[,] adjustmentMatrix) => AdjustmentMatrix = adjustmentMatrix;
        public int GetWeight(int v, int u) => AdjustmentMatrix[v, u];

        public void AddEdge(int v, int u) => AdjustmentMatrix[v, u]++;

        public void RemoveEdge(int v, int u)
        {
            if (AdjustmentMatrix[v, u] > 0)
                AdjustmentMatrix[v, u]--;
        }

        public void AddVertices(int k)
        {
            if (k > 0)
            {
                int[,] newAdjustmentMatrix = new int[AdjustmentMatrix.GetLength(0) + k, AdjustmentMatrix.GetLength(0) + k];

                for (int i = 0; i < AdjustmentMatrix.GetLength(0); i++)
                    for (int j = 0; j < AdjustmentMatrix.GetLength(0); j++)
                        newAdjustmentMatrix[i, j] = AdjustmentMatrix[i, j];

                AdjustmentMatrix = newAdjustmentMatrix;
            }
        }
    }
}
