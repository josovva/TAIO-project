namespace TAIO.Metric.Utils.Extensions;

public static class PermutationsExtensions
{
    public static IEnumerable<IEnumerable<int>> GetPermutations(this IEnumerable<int> enumerable)
    {
        var array = enumerable as int[] ?? enumerable.ToArray();

        var factorials = Enumerable.Range(0, array.Length + 1)
            .Select(Factorial)
            .ToArray();

        for (var i = 0L; i < factorials[array.Length]; i++)
        {
            var sequence = GenerateSequence(i, array.Length - 1, factorials);

            yield return GeneratePermutation(array, sequence);
        }
    }

    private static IEnumerable<int> GeneratePermutation(int[] array, IReadOnlyList<int> sequence)
    {
        var clone = (int[])array.Clone();

        for (var i = 0; i < clone.Length - 1; i++)
        {
            (clone[i], clone[i + sequence[i]]) = (clone[i + sequence[i]], clone[i]);
        }

        return clone;
    }

    private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
    {
        var sequence = new int[size];

        for (var j = 0; j < sequence.Length; j++)
        {
            var facto = factorials[sequence.Length - j];

            sequence[j] = (int)(number / facto);
            number = (int)(number % facto);
        }

        return sequence;
    }

    private static long Factorial(int n)
    {
        long result = n;

        for (var i = 1; i < n; i++)
        {
            result *= i;
        }

        return result;
    }
}