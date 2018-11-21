using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
        

        public List<Edge> prim(ref List<Node> mnodes, ref List<Edge> medges, ref int mnumber_of_edges)
        {
            bool nodea = false;
            bool nodeb = false;
            int testfrom, testto;
            List<Edge> mst = new List<Edge>();
            List<Node> visited = new List<Node>();
            medges.Sort((x, y) => x.weight.CompareTo(y.weight)); //sortowanie listy według wag rosnąco
            visited.Add(mnodes[medges[0].from-1]); // dodajemy do odwiedzanych wiercholek poczatkowy krawedzi o najmniejszej wadze (indeks w liście zaczyna się od 0 a id od 1, dlatego -1) 
            int j = 0;
            int i = 0;
            while (i < mnumber_of_edges) //dla wszystkich krawedzi
            {
               
                testfrom = medges[i].from; 
                testto = medges[i].to;
                nodea = visited.Exists(node => node.id == testfrom);
                nodeb = visited.Exists(node => node.id == testto);
                //powyzsze linijki sprawdzaja czy wierzcholki danej krawedzi byly juz odwiedzone
                if (nodea == false && nodeb == true) // jesli koniec krawedzi byl odwiedzony to dodaj poczatek krawedzi do odwiedzonych i sama krawedz do drzewa
                {
                    visited.Add(mnodes[medges[i].from-1]);
                    mst.Add(medges[i]);
                    j = 0;
                    //Console.WriteLine("testa");
                    
                }
                else if (nodea == true && nodeb == false) // jesli poczatek krawedzi byl juz odwiedzony to dodaj koniec krawedzi do odwiedzonych i sama krawedz do drzewa
                {
                    visited.Add(mnodes[medges[i].to-1]);
                    mst.Add(medges[i]);
                    j = 0;
                   //Console.WriteLine("testb");
                }
                else if (nodea == false && nodeb == false) // jesli zaden z wierzcholkow krawedzi nie byl odwiedzony to zamieniamy ta krawedz miejscami z nastepna
                {                                          // robimy to tak dlugo, az w koncu natrafimy na sasiada
                    Edge temp = medges[i];
                    medges[i] = medges[i+j+1];
                    medges[i+j+1] = temp;
                    i--;
                    j++;
                    
                    //Console.WriteLine("testc");
                }
                i++;
            }
            return mst; 
        }

        public int dijkstra(ref List<Node> mnodes, ref List<Edge> medges, int start, int end)
        {
            List<Edge> mp = new List<Edge>();
            Dictionary<Node, int> totalcosts = new Dictionary<Node, int>();
            Dictionary<Node, Node> previousnode = new Dictionary<Node, Node>();
            Queue<Node> priorityq = new Queue<Node>();
            List<Node> visited = new List<Node>();
            List<Node> neighbours = new List<Node>();
            Node beginfrom;
            Node endto;
            Node newsmallest;
            
            mnodes.Sort((x, y) => x.id.CompareTo(y.id)); //zabezpieczenie przed zmiana kolejnosci wierzcholkow(tak zeby mozna bylo zastosowac to co jest linijke nizej) (bez tego i tak powinno dzialac)
            beginfrom = mnodes[start - 1];
            endto = mnodes[end - 1];
            totalcosts.Add(beginfrom, 0); //koszt dojscia do wierzcholka poczatkowego to 0
            priorityq.Enqueue(beginfrom); //wrzucamy wierzcholek poczatkowy do kolejki rozpatrywanych wierzcholkow
            previousnode.Add(beginfrom, beginfrom);

            foreach (Node node in mnodes)
            {
                if (node != beginfrom) 
                {
                    totalcosts.Add(node, 10000);// koszt dojscia do pozostalych wierzcholkow to nieskonczonosc (10000 raczej wystarczy, no chyba ze zrobimy gigantyczne grafy)
                    previousnode.Add(node, node);
                }
            }

            while(priorityq.Count != 0) //robimy dopoki mamy cos w kolejce
            {
                foreach (KeyValuePair<Node, int> kvp in totalcosts) //te wypisywania sa po to zeby wiedziec co sie dzieje z kazdzym wywolaniem petli, zrobilem je do testow
                {
                   
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key.id, kvp.Value);
                    
                }
                Console.WriteLine("prvnode");
                foreach (KeyValuePair<Node, Node> kvp in previousnode)
                {
                    
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key.id, kvp.Value.id);

                }
                Console.WriteLine("test");

                newsmallest = priorityq.Dequeue(); // nasz wierzcholek o  obecnie najmniejszym koszcie dojscia usuwany z kolejki i dodajemy do odwiedzonych
                visited.Add(newsmallest); 
                medges.Sort((x, y) => x.weight.CompareTo(y.weight)); //sortujemy krawedzie wedlug wag, zeby lista sasiadow tez byla uporzadkowana
                for (int i= 0; i<medges.Count; i++) // ta petla tworzy liste sasiadow wierzcholka rozpatrywanego posortowana wedlug odleglosci od tego wierzcholka
                {
                    if (medges[i].from == newsmallest.id)
                    {
                        int idneeded;
                        idneeded = medges[i].to;
                        neighbours.Add(mnodes[idneeded - 1]);
                    }
                    else if (medges[i].to == newsmallest.id)
                    {
                        int idneeded;
                        idneeded = medges[i].from;
                        neighbours.Add(mnodes[idneeded - 1]);

                    }
                }

                for (int i=0; i<neighbours.Count; i++ ) //sprawdzamy sasiadow
                {
                    if (!visited.Contains(neighbours[i])) //ale tylko jesli juz ich nie badalismy wczesniej
                    {
                        double pitagoras = Math.Sqrt((neighbours[i].x - newsmallest.x) * (neighbours[i].x - newsmallest.x) + ((neighbours[i].y - newsmallest.y) * (neighbours[i].y - newsmallest.y)));
                        int newweight = totalcosts[newsmallest] + Convert.ToInt32(pitagoras); //nowa waga to waga dojscia do wierzcholka, ktora juz mamy wpisana wczesniej w tabelke kosztow + odleglosc do sasiada

                        if (!priorityq.Contains(neighbours[i])) //dodajemy sasiada do kolejki rozpatrywania(ale tylko jesli jeszcze go nie ma w tej kolejce)
                        {
                            priorityq.Enqueue(neighbours[i]);
                        }

                        if (newweight < totalcosts[neighbours[i]]) //jesli nowa waga jest mniejsza niz dotychczasowa to aktualizujemy tabelke kosztow i to skad przyszlismy
                        {
                            totalcosts[neighbours[i]] = newweight;
                            previousnode[neighbours[i]] = newsmallest;
                        }

                    }
                    
                }
            }

            return totalcosts[endto];
        }

        public void drawedges(ref List<Node> mnodes, ref List<Edge> medges)
        {

            Bitmap myBitmap = new Bitmap(1200, 1200);
            Graphics g = Graphics.FromImage(myBitmap);
            Pen pen = new Pen(Brushes.AliceBlue, 2);
            Pen pen2 = new Pen(Brushes.DeepSkyBlue, 20);
            Pen pen3 = new Pen(Brushes.Red, 2);
            for (int i = 0; i < mnodes.Count(); i++)
            {
                g.DrawEllipse(pen2, (mnodes[i].x) * 10, (mnodes[i].y) * 10, 20, 20);
                g.DrawEllipse(pen3, (mnodes[i].x) * 10 - 10, (mnodes[i].y) * 10 - 10, 40, 40);
            }
            for (int i = 0; i < medges.Count(); i++)
            {
                g.DrawLine(pen, (mnodes[medges[i].from - 1].x) * 10 + 10, (mnodes[medges[i].from - 1].y) * 10 + 10, (mnodes[medges[i].to - 1].x) * 10 + 10, (mnodes[medges[i].to - 1].y) * 10 + 10);
            }
            myBitmap.Save(@"D:\graf.bmp");
        }
           
    }

    class Program
    {
        static void Main(string[] args)
        {
           List<Node> nodes = new List<Node>();
           List<Edge> edges = new List<Edge>();
           List<Edge> mintree = new List<Edge>();
           Network network = new Network(nodes, edges);
           int number_of_nodes = 0;
           int number_of_edges = 0;
           int shortestpath;

           network.readfile(ref nodes, ref edges, ref number_of_nodes, ref number_of_edges);
           network.calculateweight(ref nodes, ref edges, ref number_of_nodes, ref number_of_edges);
           network.drawedges(ref nodes, ref edges);

            foreach (Edge edge in edges)
            {
                int i = edges.IndexOf(edge);
                Console.WriteLine(edges[i].id + " " + edges[i].from + " " + edges[i].to + " " + edges[i].weight);
            }

            mintree = network.prim(ref nodes, ref edges, ref number_of_edges);
            Console.WriteLine("mst:");
            foreach (Edge edge in mintree)
            {
                int i = mintree.IndexOf(edge);
              
                Console.WriteLine(mintree[i].id + " " + mintree[i].from + " " + mintree[i].to + " " + mintree[i].weight);
                
            }
            shortestpath = network.dijkstra(ref nodes, ref edges, 2, 3);
            Console.WriteLine("shortestpath:" + shortestpath);

            Console.ReadKey();

        }
    }
    
    
}
