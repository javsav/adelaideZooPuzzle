/* This is a program that uses elements of graph theory to solve a puzzle
analogous to the "traveling salesman problem". However, the premise
is that a group is visiting the Adelaide Zoo, and wants to find the shortest
distance that they can take from the entrance (represented by the
letter D) to the exit (represented by the letter F) while visiting all
of the animal enclosures at the zoo (represented by letters A, B, C, D, E, F,
G, H, and I), which are known as vertices (single vertex). There are different
distances between each letter, which in graph theory are called the edges; the
paths between the vertices. The paths are bi-directional; the group
can travel on either direction along each path. This program creates a
dictionary to represent each vertex (animal enclosure), the connections between
each vertex, and the distances of each path between vertices (edges).
It then uses a brute-force algorithm to check all possible paths from the
entrance vertex ('D') to the exit vertex ('F') without bouncing between vertices
more than twice (maxVertexVisits = 2), for n attempts. It discards a route if
the current distance is greater than the best distance found at that stage. The
program then returns the shortest distance found for n attempts, and gives the
order of the vertices visited, in addition to any equivalent solutions that are
found with the same length. Additionally, the program returns all other unique
solutions found, their total distances and vertex orders. The value for attempts
was methodically increased until no shorter solutions were found. */

// Importing necessary namespaces for the script:

using System;                      // Used for console log.
using System.Collections.Generic;  // For creating vertex and edge dictionaries.
using System.Linq;                 // For sorting list.

// Defining the Program class.
public class Program {
  // Entry point of the script.
  public static void Main() {
    // Creating an instance of the Zoo class and running it.
    var adelaideZoo = new Zoo();
    adelaideZoo.Run();
  }
  // Defining the Zoo class
  public class Zoo {
    // Declaring edgeDict and vertexDict to store edge and vertex information.
    Dictionary<string, Edge> edgeDict = new Dictionary<string, Edge>();
    Dictionary<char, Vertex> vertexDict = new Dictionary<char, Vertex>();

    /* Declaring variables for entry and exit vertices, number of attempts, and
    maximum vertex visits.*/
    char entryVertex = 'D';   // Entry to the zoo is at vertex D.
    char exitVertex = 'F';    // Exit from the zoo is at vertex F.
    int attempts = 1000000;   // Number of attempts (brute-force algorithm).
    int maxVertexVisits = 2;  // Don't visit any vertex more than twice.

    // Initialise Zoo Graph by setting up the vertices and edges.
    void initialiseZooGraph() {
      /* Populate edge dictionary with all bi-directional edge distances
      between vertices. */

      edgeDict.Add("AB", new Edge(200));  // Dist A <-> B = 200
      edgeDict.Add("AD", new Edge(150));  // Dist A <-> D = 150
      edgeDict.Add("BC", new Edge(200));  // Dist B <-> C = 200
      edgeDict.Add("BE", new Edge(150));  // Dist B <-> E = 150
      edgeDict.Add("BF", new Edge(250));  // Dist B <-> F = 250
      edgeDict.Add("BD", new Edge(250));  // Dist B <-> D = 250
      edgeDict.Add("CF", new Edge(150));  // Dist C <-> F = 150
      edgeDict.Add("DG", new Edge(150));  // Dist D <-> G = 150
      edgeDict.Add("DH", new Edge(250));  // Dist D <-> H = 250
      edgeDict.Add("DE", new Edge(200));  // Dist D <-> E = 200
      edgeDict.Add("EH", new Edge(150));  // Dist E <-> H = 150
      edgeDict.Add("EF", new Edge(200));  // Dist E <-> F = 200
      edgeDict.Add("FI", new Edge(150));  // Dist F <-> I = 150
      edgeDict.Add("FH", new Edge(250));  // Dist F <-> H = 250
      edgeDict.Add("GH", new Edge(200));  // Dist G <-> H = 200
      edgeDict.Add("HI", new Edge(200));  // Dist H <-> I = 200

      /* Populate vertex dictionary with references to the edge objects that
      are stored in the above edge dictionary. */

      // Vertex A is connected to vertices B and D.
      vertexDict.Add('A', new Vertex(
                            new Edge[] { 
                               edgeDict["AB"],
                               edgeDict["AD"] },
                                'A'));
      // Vertex B is connected to vertices A, C, E, F, and D
      vertexDict.Add('B', new Vertex(
                            new Edge[] { 
                                edgeDict["AB"],
                                edgeDict["BC"], 
                                edgeDict["BE"],
                                edgeDict["BF"],
                                edgeDict["BD"] },
                                'B'));

      // Vertex C is connected to vertices B and F
      vertexDict.Add('C', new Vertex(
                            new Edge[] { 
                                edgeDict["BC"], 
                                edgeDict["CF"] },
                                'C'));

      // Vertex D is connected to vertices A, G, B, H, and E
      vertexDict.Add('D', new Vertex(
                            new Edge[] { 
                                edgeDict["AD"],
                                edgeDict["BD"],
                                edgeDict["DG"],
                                edgeDict["DH"],
                                edgeDict["DE"] },
                                'D'));

      // Vertex E is connected to vertices B, H, F, and D
      vertexDict.Add('E', new Vertex(
                              new Edge[] {
                                edgeDict["BE"],
                                edgeDict["EF"],
                                edgeDict["DE"],
                                edgeDict["EH"] },
                                'E'));

      // Vertex F is connected to vertices C, I, E, B and H
      vertexDict.Add('F', new Vertex(
                              new Edge[] {
                                edgeDict["CF"],
                                edgeDict["FI"],
                                edgeDict["FH"],
                                edgeDict["EF"],
                                edgeDict["BF"] },
                                'F'));

      // Vertex G is connected to vertices D and H
      vertexDict.Add('G', new Vertex(
                              new Edge[] { 
                                edgeDict["GH"],
                                edgeDict["DG"] },
                                'G'));

      // Vertex H is connected to vertices G, I, and E, F and D
      vertexDict.Add('H', new Vertex(
                              new Edge[] {
                                edgeDict["GH"],
                                edgeDict["DH"],
                                edgeDict["HI"],
                                edgeDict["EH"],
                                edgeDict["DH"] },                           
                                'H'));

      // Vertex I is connected to vertices F and H
      vertexDict.Add('I', new Vertex(
                              new Edge[] { 
                                edgeDict["FI"],
                                edgeDict["HI"] },
                                'I'));

      /* Iterate over edge dictionary and assign references of the vertices to
       * their edges. */
      foreach (var entry in edgeDict) {
        entry.Value.setVertices(vertexDict[entry.Key[0]],
                                vertexDict[entry.Key[1]]);
      }
    }

    // initialiseCountsDictionary to track visit counts of vertices.
    Dictionary<char, int> initialiseCountsDictionary() {

      /*Create visitCounts dictiomary where each vertex char ('key)' is
      associated with the number of vists to that vertex, initialise as 0.*/
      Dictionary<char, int> visitCounts = new Dictionary<char, int>();    
      visitCounts.Add('A', 0);
      visitCounts.Add('B', 0);
      visitCounts.Add('C', 0);
      visitCounts.Add('D', 0);
      visitCounts.Add('E', 0);
      visitCounts.Add('F', 0);
      visitCounts.Add('G', 0);
      visitCounts.Add('H', 0);
      visitCounts.Add('I', 0);
      return visitCounts;
    }

    // Define edge class: represents edges between vertices.
    class Edge {
      public int length;
      public Vertex vertex1, vertex2;

      // Constructor to initalise the length of the edge.
      public Edge(int length) {
        this.length = length;
      }
      // getOtherVertex returns the vertex on the other end of the edge.
      public Vertex getOtherVertex(Vertex sourceVertex) {
        return sourceVertex == vertex1 ? vertex2 : vertex1;
      }
      // setVertices sets the vertices of the edge.
      public void setVertices(Vertex vertex1, Vertex vertex2) {
        this.vertex1 = vertex1;
        this.vertex2 = vertex2;
      }
    }
    /*The Vertex class represents the animal enclosures in the zoo, and contains
    an associated list of connecting edges */
    class Vertex {
      // Define edges as an array of the Edge class.
      public Edge[] edges;
      // Name char represents the vertices (A, B, C....I).
      public char name;

      /* Define Vertex function  (1st input arg is edges - array of
      Edge class, 2nd input argument is the vertex char (A...I). */
      public Vertex(Edge[] edges, char name) {
        // Set this.edges equal to edges
        this.edges = edges;
        // Set this.name equal to vertex char
        this.name = name;
      }
    }

    /* Random variable is used when picking edge to move to from a vertex. */
    Random randInt = new Random();

    // Placeholder best distance.
    int bestDistance = Int32.MaxValue;

    /* List of pairs called resultsList; Each comprises a string and an int. */
    List<KeyValuePair<string, int>> resultsList =
        new List<KeyValuePair<string, int>>();

    /* Create resultsDict to append numbers of unique solutions found */
    Dictionary<string, int> resultsDict = new Dictionary<string, int>();

    // Run method for pathfinding algorith.
    public void Run() {

      // Initialise the Zoo map by setting up the vertices and edges.
      initialiseZooGraph();
         // Perform path finding for n attempts.
        for (int currentPath = 0; currentPath < attempts; currentPath++) {
          findPath();
        }      
      // Print number of unique solutions found.
      Console.WriteLine("Found " + resultsDict.Count + " unique solutions:");

      // Copy the results from resultsDict to resultslist.
      foreach (var entry in resultsDict) {
        resultsList.Add(new KeyValuePair<string, int>(entry.Key, entry.Value));
      }

      // Sort the list based on distance.
      var sortedList = resultsList.OrderBy(pair => pair.Value).ToList();

      // Print the best path (0th index of sortedList) and its distance.
      Console.WriteLine("Best path was " + sortedList[0].Key + " with distance " 
      + sortedList[0].Value + " meters.");

      // Iterate through sortedList
      foreach (var entry in sortedList) {
        // Don't print same result twice
        if (entry.Key.Equals(sortedList[0].Key))
          continue;
        // Obtain and print equivalent vertex order for best solution.
        if (entry.Value == sortedList[0].Value)
          Console.WriteLine("Equivalent solution is " + entry.Key + ".");
      }

      // Print all other unique solutions' found distances and vertex orders.
      foreach (var entry in sortedList) {
        Console.WriteLine("Path: " + entry.Key + " distance: " + entry.Value
        + " meters.");
      }
    }

    //findPath brute force pathfinding algorithm from 'D' to 'F'.
    void findPath() {
      // Def path as empty string to contain the sequence of vertices visited.
      string path = "";

      // Initialise vertices and visit counts to 0.
      Dictionary<char, int> visitCounts = initialiseCountsDictionary();

      // Start at the entry vertex and increment its visit count to 1.
      Vertex currentVertex = vertexDict[entryVertex];
      visitCounts[entryVertex]++;

      // Append entry vertex to the path sequence
      path += currentVertex.name;

      // Initialise distance as 0.
      int distance = 0;

      // Continuously search for a path from D to F while distance < bestDistance
      while (distance < bestDistance) {
  
        // Initialize viableEdges as empty list
        List<Edge> viableEdges = new List<Edge>();
        // For each edge in currentVertex's edges:
        foreach (Edge edge in currentVertex.edges) {
          // If the other vertex of the edge has been visited less than twice:
          if (visitCounts[edge.getOtherVertex(currentVertex).name] <
              maxVertexVisits) {
            // Add the edge to viableEdges to search
            viableEdges.Add(edge);
          }
        }

        // If there are no viable edges, break out of the loop.
        if (viableEdges.Count == 0)
          break;

        // Else, select a random edge from viableEdges.
        Edge randomEdge = viableEdges[randInt.Next(0, viableEdges.Count)];

        // Add length of the selected edge to the total distance.
        distance += randomEdge.length;

        // Move to the vertex on other side of the edge, and add to sequence.
        currentVertex = randomEdge.getOtherVertex(currentVertex); 
        path += currentVertex.name;

        // Increase visit count of the vertex
        visitCounts[currentVertex.name]++;

        // If not at the exit vertex: 
        if (!currentVertex.name.Equals(exitVertex))
          continue; //move to the next iteration of while loop.

       
        // Check if all vertices have been visited.
        bool incomplete = false;
        // Iterate over each vertex in visitCounts:
        foreach (var entry in visitCounts) {
          // If all vertices (animals) haven't been visited:
          if (entry.Value == 0) {
            // The pathfinding is incomplete.
            incomplete = true;
            break;
          }
        }
      
        // If all vertices have been visited:
        if (!incomplete) {
          // Add path and its associated distance to resultsDict.
          resultsDict[path] = distance;

          /* The final edge can cause a complete run to be larger than
          best distance, so stop if distance < bestDistance. */
          if (distance < bestDistance) {
            bestDistance = distance;
            break;
          }
        }
      }
    }
  }
}
