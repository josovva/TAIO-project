
using System.ComponentModel;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using TAIO.MaxCliqueAlg;

namespace TAIO
{
    public class DISMACSParser
    {
        enum DISMACSTokenType
        {
            INFO,
            TEXT,
            DEFINITION,
            EDGE,
            NUMBER,
            WHITESPACE,
            NEWLINE,
            EOF,
        }

        private List<char> Numbers = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private int CurrentNumber = 0;

        public int[,] Parse(string graph_path)
        {
            int[,] graph_matrix = new int[1,1];


            var scanner = ScanText(graph_path); scanner.MoveNext();
            var token = scanner.Current;

            while (token != DISMACSTokenType.EOF)
            {
                if (token == DISMACSTokenType.DEFINITION)
                {
                    scanner.MoveNext(); // TEXT
                    scanner.MoveNext(); // NUMBER

                    int vertices = CurrentNumber;
                    graph_matrix = new int[vertices, vertices];

                    scanner.MoveNext(); // NUMBER

                }
                else if (token == DISMACSTokenType.EDGE)
                {
                    scanner.MoveNext(); // NUMBER
                    int u = CurrentNumber - 1;

                    scanner.MoveNext(); // NUMBER
                    int v = CurrentNumber - 1;
                    graph_matrix[u, v] = 1;
                    graph_matrix[v, u] = 1;
                }

                scanner.MoveNext();
                token = scanner.Current;
            }

            return graph_matrix;
        }

        private IEnumerator<DISMACSTokenType> ScanText(string graph_path)
        {
            using StreamReader file = new(graph_path);

            string? ln;
            while ((ln = file.ReadLine()) != null)
            {
                for (int i = 0; i < ln.Length; i++)
                {
                    if (ln[i] == 'c')
                    {
                        yield return DISMACSTokenType.INFO;
                        yield return DISMACSTokenType.TEXT;
                        yield return DISMACSTokenType.NEWLINE;
                        break;
                    }
                    if (ln[i] == 'p')
                    {
                        yield return DISMACSTokenType.DEFINITION;

                        i += 1;

                        SkipWhitespace(ln, ref i);
                        SkipLetters(ln, ref i);
                        yield return DISMACSTokenType.TEXT;

                        SkipWhitespace(ln, ref i);
                        ParseNumber(ln, ref i);
                        yield return DISMACSTokenType.NUMBER;

                        SkipWhitespace(ln, ref i);
                        ParseNumber(ln, ref i);
                        yield return DISMACSTokenType.NUMBER;
                        i -= 1;
                    }
                    else if (ln[i] == 'e')
                    {
                        yield return DISMACSTokenType.EDGE;

                        i += 1;
                        SkipWhitespace(ln, ref i);
                        ParseNumber(ln, ref i);
                        yield return DISMACSTokenType.NUMBER;

                        SkipWhitespace(ln, ref i);
                        ParseNumber(ln, ref i);
                        yield return DISMACSTokenType.NUMBER;
                        i -= 1;
                    }
                    else if (ln[i] == ' ' || ln[i] == '\t')
                    {
                        yield return DISMACSTokenType.WHITESPACE;
                    }
                    else if (ln[i] == '\n')
                    {
                        yield return DISMACSTokenType.NEWLINE;
                    }
                }
            }

            yield return DISMACSTokenType.EOF;
        }

        private void SkipWhitespace(string line, ref int idx)
        {
            while (idx < line.Length && (line[idx] == ' ' || (line[idx] == '\t')))
            {
                idx += 1;
            }
        }

        private void ParseNumber(string line, ref int idx)
        {

            StringBuilder str_build = new();
            str_build.Append(line[idx]);

            idx += 1;
            while (idx < line.Length && Numbers.Contains(line[idx]))
            {
                str_build.Append(line[idx]);
                idx += 1;
            }

            string number_string = str_build.ToString();
            CurrentNumber = int.Parse(number_string);
        }

        private void SkipLetters(string line, ref int idx)
        {
            while (idx < line.Length && (line[idx] >= 'A' && (line[idx] <= 'z')))
            {
                idx += 1;
            }
        }
    }
}
