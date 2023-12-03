using System.Reflection;
using CommandLine;
using TAIO.CommonSubgraph;
using TAIO.MaxCliqueAlg;
using TAIO.Metric;

namespace TAIO.CLI;

public static class AlgorithmRunner
{
    public static int Run(IEnumerable<string> args)
    {
        return Parser.Default.ParseArguments(args, LoadCommands())
            .MapResult(RunAlgorithm, _ => -1);
    }

    private static Type[] LoadCommands()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<VerbAttribute>() is not null).ToArray();
    }

    private static int RunAlgorithm(object obj)
    {
        switch (obj)
        {
            case MetricOptions m:
                RunMetricAlgorithm(m);
                break;
            case MaxCliqueOptions c:
                RunMaxCliqueAlgorithm(c);
                break;
            case MaxCommonSubgraphOptions s:
                RunMaxCommonSubgraph(s);
                break;
            default:
                return -1;
        }

        return 0;
    }

    private static void RunMetricAlgorithm(MetricOptions options)
    {
        var g = Parsers.Parser.ParseFile(options.GPath).SingleOrDefault();
        var h = Parsers.Parser.ParseFile(options.HPath).SingleOrDefault();

        if (g is null || h is null)
            return;

        IMetricCalculator calculator = options.RunExact ? new ExactMetricCalculator() : new ApproximateMetricCalculator();
        Console.WriteLine($"Calculated metric value: {calculator.Calculate(g, h)}");
    }

    private static void RunMaxCliqueAlgorithm(MaxCliqueOptions options)
    {
        var g = Parsers.Parser.ParseFile(options.GPath).SingleOrDefault();

        if (g is null)
            return;
        
        if (options.RunExact)
        {
            var (porosity, clique) = BrutalMaxClique.Calcuate(g.AdjustmentMatrix, options.L, options.Porosity);
            g.DisplaySolution(clique, options.L, porosity);
        }
        else
        {
            // you can replace temp fitness function
            var intermediateG = new IntermediateGraph(g.AdjustmentMatrix);
            var (porosity, clique) = MaxClique.Calculate(intermediateG, options.L, options.Porosity, Size.FitnessFunctionMaxClique);

            g.DisplaySolution(clique, options.L, porosity);
        }
    }

    private static void RunMaxCommonSubgraph(MaxCommonSubgraphOptions options)
    {
        var g = Parsers.Parser.ParseFile(options.GPath).SingleOrDefault();
        var h = Parsers.Parser.ParseFile(options.HPath).SingleOrDefault();

        if (g is null || h is null)
            return;

        var result = options.RunExact ? MaxCommonSubgraph.FindExact(g, h) : MaxCommonSubgraph.FindApprox(g, h);

        g.DisplaySolution(h, result);
        h.DisplaySolution(g, result.ToDictionary(x => x.Value, y => y.Key));
    }
}