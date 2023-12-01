using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAIO.Parsers
{
    public static class Parser
    {
        public static Graph[] ParseFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            int numberOfGraphs = int.Parse(lines[0]);
            int currentIndex = 1;
            Graph[] graphs = new Graph[numberOfGraphs];

            for (int graphIndex = 0; graphIndex < numberOfGraphs; graphIndex++)
            {
                int size = int.Parse(lines[currentIndex]);
                currentIndex++;

                int[,] matrix = new int[size, size];

                for (int i = 0; i < size; i++)
                {
                    string[] rowValues = lines[currentIndex].Split(' ');
                    currentIndex++;

                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = int.Parse(rowValues[j]);
                    }
                }

                graphs[graphIndex] = new Graph(matrix);

                while (currentIndex < lines.Length && !string.IsNullOrWhiteSpace(lines[currentIndex]))
                {
                    currentIndex++;
                }

                currentIndex++;
            }

            return graphs;
        }
    }
}
