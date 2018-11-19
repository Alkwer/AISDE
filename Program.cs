using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Node
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Node(int mid, int mx, int my)
        {
            id = mid;
            x = mx;
            y = my;

        }
    }
    class Edge
    {
        public int id { get; set; }
        public int weight { get; set; }
        public int from { get; set; }
        public int to { get; set; }

        public Edge(int mid, int mfrom, int mto, int mweight)
        {
            id = mid;
            from = mfrom;
            to = mto;
            weight = mweight;
        }

    }

    class Path
    {
       List<Edge> path = new List<Edge>();
    }

    class Network
    {

        public List<Node> nodes;
        public List<Edge> edges;

        public Network(List<Node> nnodes, List<Edge> nedge)
        {
            nodes = nnodes;
            edges = nedge;
        }

        public void readfile(ref List<Node> mnodes, ref List<Edge> medges, ref int mnumber_of_nodes, ref int mnumber_of_edges)
        {
            string[] data = System.IO.File.ReadAllLines(@"D:\test.txt");
            string temp;
            string[] splitedtemp = null;
           

            temp = data[0];
            mnumber_of_nodes = Convert.ToInt32(temp);

            for (int i = 1; i < mnumber_of_nodes + 1; i++)
            {
                temp = data[i];
                splitedtemp = temp.Split(' ');
                mnodes.Add(new Node(Convert.ToInt32(splitedtemp[0]), Convert.ToInt32(splitedtemp[1]), Convert.ToInt32(splitedtemp[2])));
                
            }

            temp = data[mnumber_of_nodes + 1];
            mnumber_of_edges = Convert.ToInt32(temp);

            for (int i = 1; i < mnumber_of_edges +1; i++)
            {
                temp = data[mnumber_of_nodes + i + 1];
                splitedtemp = temp.Split(' ');
                medges.Add(new Edge(Convert.ToInt32(splitedtemp[0]), Convert.ToInt32(splitedtemp[1]), Convert.ToInt32(splitedtemp[2]), 1));
            }

        }

        public void calculateweight(ref List<Node> mnodes, ref List<Edge> medges, ref int mnumber_of_nodes, ref int mnumber_of_edges)
        {
            int node1id = 0;
            int node2id = 0;
            int node1x = 0;
            int node1y = 0;
            int node2x = 0;
            int node2y = 0;
            double pitagoras = 0;

            for (int i=0; i<mnumber_of_edges; i++)
            {
                node1id = medges[i].from;
                node2id = medges[i].to;

                for (int j=0; j<mnumber_of_nodes; j++)
                {
                    if (node1id == mnodes[j].id)
                    {
                        node1x = mnodes[j].x;
                        node1y = mnodes[j].y;

                    }

                    if (node2id == mnodes[j].id)
                    {
                        node2x = mnodes[j].x;
                        node2y = mnodes[j].y;

                    }
                }

                double dnode1x = Convert.ToDouble(node1x);
                double dnode1y = Convert.ToDouble(node1y);
                double dnode2x = Convert.ToDouble(node2x);
                double dnode2y = Convert.ToDouble(node2y);

                pitagoras = Math.Sqrt((dnode2x-dnode1x)*(dnode2x-dnode1x)+((dnode2y-dnode1y)*(dnode2y-dnode1y)));
                medges[i].weight = Convert.ToInt32(pitagoras);
            }

        }

        public List<Edge> prim(ref List<Node> mnodes, ref List<Edge> medges)
        {
            List<Edge> mst = new List<Edge>();
            List<Node> visited = new List<Node>();
            medges.Sort((a, b) => a.weight.CompareTo(b.weight)); //sortowanie listy według wag rosnąco
            visited.Add(mnodes[medges[0].from-1]); // dodajemy do odwiedzanych node poczatkowy edga o najmniejszej wadze (indeks w liście zaczyna się od 0 a id od 1, dlatego -1)


            return mst; 
        }
           
    }
    class Program
    {
        static void Main(string[] args)
        {
           List<Node> nodes = new List<Node>();
           List<Edge> edges = new List<Edge>();
           Network network = new Network(nodes, edges);
           int number_of_nodes = 0;
           int number_of_edges = 0;

           network.readfile(ref nodes, ref edges, ref number_of_nodes, ref number_of_edges);
           network.calculateweight(ref nodes, ref edges, ref number_of_nodes, ref number_of_edges);
        
           Console.WriteLine(edges[0].weight);
           Console.ReadKey();

        }
    }
    
    
}
