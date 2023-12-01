namespace TAIO.CommonSubgraph.Utils;

public class ListComparison : IComparer<List<int>>
{
    public int Compare(List<int> x, List<int> y)
    {
        return y.Count - x.Count;
    }
}
