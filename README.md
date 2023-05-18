# adelaideZooPuzzle
*NOTE - This can be run with https://dotnetfiddle.net if you don't have a C# compiler installed*

Direct link to run: https://dotnetfiddle.net/9V36vq

Pathfinding problem implementing grah theory

This is a program that uses elements of graph theory to solve a puzzle
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
was methodically increased until no shorter solutions were found.

A link to the diagram of the zoo is found here: https://i.ibb.co/F6c2VCp/Untitled.png
