using CommandLine;

namespace TAIO.CLI;

[Verb("metric", HelpText = "Calculates metric value between two given multigraphs.")]
public class MetricOptions
{
    [Option('g', Required = true, HelpText = "Path to the .txt file containing multigraph G.")]
    public string GPath { get; set; } = default!;

    [Option('h', Required = true, HelpText = "Path to the .txt file containing multigraph H.")]
    public string HPath { get; set; } = default!;

    [Option('e', Required = false, Default = false, HelpText = "If specified, runs an exact algorithm instead of an approximate one.")]
    public bool RunExact { get; set; }
}

[Verb("clique", HelpText = "Finds the maximum clique in the given multigraph.")]
public class MaxCliqueOptions
{
    [Option('g', Required = true, HelpText = "Path to the .txt file containing multigraph G.")]
    public string GPath { get; set; } = default!;

    [Option('l', Required = false, Default = 1, HelpText = "Minimal expected thickness (order) of the searched clique.")]
    public int L { get; set; }

    [Option('p', Required = false, Default = 0.0, HelpText = "Maximum porosity (ratio of empty arcs to non-empty arcs) of the found clique.")]
    public double Porosity { get; set; }

    [Option('e', Required = false, Default = false, HelpText = "If specified, runs an exact algorithm instead of an approximate one.")]
    public bool RunExact { get; set; }
}

[Verb("mcs", HelpText = "Finds the maximum common subgraph of two given multigraphs.")]
public class MaxCommonSubgraphOptions
{
    [Option('g', Required = true, HelpText = "Path to the .txt file containing multigraph G.")]
    public string GPath { get; set; } = default!;

    [Option('h', Required = true, HelpText = "Path to the .txt file containing multigraph H.")]
    public string HPath { get; set; } = default!;

    [Option('e', Required = false, Default = false, HelpText = "If specified, runs an exact algorithm instead of an approximate one.")]
    public bool RunExact { get; set; }
}