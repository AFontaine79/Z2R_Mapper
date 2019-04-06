# Palace Routing Algorithm
Palace routing works based on a recursive algorithm.  It starts at the index of the starting room and recursively follows valid exits until it arrives at the index of the ending room.  A search path terminates when the same connection is reused.  That means one of two things:
1.	We have come back around to the same room and are about to follow an exit we took previously.
2.	We are about to exit through a room connection already used to enter a room regardless of whether that connection was the one just used to enter the room or its use occurred previously in the path.

The pathfind algorithm always searches the entire tree.  It does not stop at the first solution.  When a solution is found, it calls AddSolution() to snapshot the current path and convert it a RoutingSolution object, which is then added to the array of solutions.  After snapshotting a solution, the pathfind algorithm continues on from that point.
