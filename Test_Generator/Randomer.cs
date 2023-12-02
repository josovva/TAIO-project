namespace Test_Generator;

public class Randomer
{
    private int MaxRandom { get; }
    private int Count { get; }
    private HashSet<int> Ints { get; } = new HashSet<int>();

    public Randomer(int maxRandom, int count)
    {
        MaxRandom = maxRandom;
        Count = count;
    }

    public IEnumerable<int> Enumerate()
    {
        Ints.Clear();

        var count = Count;
        do
        {
            var i = new Random().Next(0, MaxRandom);

            if (!Ints.Contains(i))
            {
                Ints.Add(i);
                --count;
                yield return i;
            }
        } while (count > 0);
    }
}
