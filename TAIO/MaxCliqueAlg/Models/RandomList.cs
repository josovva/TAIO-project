using System.Collections;
using System.Collections.Generic;

namespace TAIO.MaxCliqueAlg

{
    public class RandomList : IEnumerable<Vertex>
    {
        private List<Vertex> Elements { get; set; }
        private Random Rnd { get; set; }

        public RandomList(int number_of_elements, int start = 0)
        {
            Elements = new List<Vertex>();
            for (int i = start; i < number_of_elements; i++)
            {
                Elements.Add(i);
            }
            Rnd = new Random();
        }

        public RandomList(List<Vertex> elements)
        {
            Elements = elements;
            Rnd = new Random();
        }

        public Vertex GetRandomElementOnce()
        {
            int random_index = Rnd.Next(Elements.Count);
            var elem = Elements[random_index];

            Elements.RemoveAt(random_index);
            return elem;
        }

        public Vertex GetRandomElement()
        {
            return Elements[Rnd.Next(Elements.Count)];
        }

        public bool IsEmpty() => Elements.Count == 0;

        public void Remove(Vertex elem) => Elements.Remove(elem);

        public IEnumerator<Vertex> GetEnumerator()
        {
            foreach (var elem in Elements)
            {
                yield return elem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public HashSet<Vertex> ToHashSet() => new(Elements);
    }
}
