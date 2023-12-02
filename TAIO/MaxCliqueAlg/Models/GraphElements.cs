
namespace TAIO.MaxCliqueAlg
{
    public struct Vertex
    {
        public int v;

        public static implicit operator int(Vertex v) => v.v;
        public static implicit operator Vertex(int vertex) => new(vertex);

        public Vertex(int v) => this.v = v;
    }

    public struct Arch
    {
        public Vertex u_vertex;
        public Vertex v_vertex;

        public static implicit operator (int, int)(Arch edge) => (edge.u_vertex, edge.v_vertex);
        public static implicit operator Arch((int u_vertex, int v_vertex) edge) => new(edge.u_vertex, edge.v_vertex);

        public Arch(Vertex u_vertex, Vertex v_vertex)
        {
            this.u_vertex = u_vertex;
            this.v_vertex = v_vertex;
        }

        public Arch(int u_vertex, int v_vertex)
        {
            this.u_vertex = u_vertex;
            this.v_vertex = v_vertex;
        }
    }

    public struct Edge
    {
        public Vertex u_vertex;
        public Vertex v_vertex;
        public int weight;

        public static implicit operator (int, int, int)(Edge edge) => (edge.u_vertex, edge.v_vertex, edge.weight);
        public static implicit operator Edge((int u_vertex, int v_vertex, int weight) edge) => new(edge.u_vertex, edge.v_vertex, edge.weight);

        public Edge(Vertex u_vertex, Vertex v_vertex, int weight)
        {
            this.u_vertex = u_vertex;
            this.v_vertex = v_vertex;
            this.weight = weight;
        }

        public Edge(int u_vertex, int v_vertex, int weight)
        {
            this.u_vertex = u_vertex;
            this.v_vertex = v_vertex;
            this.weight = weight;
        }
    }
}
