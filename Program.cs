using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar
{
    /*
     This program calculate shortest path from start point to start point using A* Algorithm 
     This Algorithm most use for game development
     Creation Data : 11/05/2020
         */
    public class Node // This class represent a single node of grid 
    {
        public int gCost { get; set; } // this hold the G cost of single node
        public int hCost { get; set; } // this hold the h cost of single node
        public int fCost { get; set; } // this hold the f = g+h cost of a single node
        public int posI { get; set; } // this hold the node X position of a single node
        public int posJ { get; set; } // this hold the node Y position of a single node
        public Node Parent{get; set;} // this Node class type variable hold the previous node details
        public Node(int posI,int posJ) // this is a constructor of a Node class thats set the position of Node
        {
            this.posI = posI;
            this.posJ = posJ;
        }

        public void setFcost() // this Method Make the F cost of a single node
        {
            this.fCost = gCost + hCost;
        }
        
    }
    class Program // this is main class Of A* Algorithm 
    {
        static void Main(string[] args) // This is main Method
        {
            Console.WriteLine("Enter the grid size.");
            int gridSize = Convert.ToInt32(Console.ReadLine());
            int sideCost = 10;
            int diagonalCost = 14;
            Console.WriteLine("Enter the Start Point x : ");
            int StartPosI = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the Start Point Y : ");
            int StartPosJ = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the End Point x : ");
            int EndPosI = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the End Point Y : ");
            int EndPosJ = Convert.ToInt32(Console.ReadLine());

            Node[,] grid = new Node[gridSize,gridSize]; // Grid are created

            for(int i=0; i<grid.GetLength(0); i++) // this loop alive the Every Node with there position
            {
                for(int j=0; j<grid.GetLength(1); j++)
                {
                    grid[i, j] = new Node(i,j);
                }
            }

            List<Node> Open = new List<Node>(); //  this is Open list 
            List<Node> Close = new List<Node>(); // this is close list
            Node LastNode = new Node(-1, -1); // Here I create a just single node which just hold the last node. if this node positon of x is -1 and y is -1 then we don't reach the final distenation
            Stack<Node> FinalPath = new Stack<Node>(); // this stack hold the all right path so that we can get the final path 
            Node Parent; // this is just a single node for initializing parent node 

            Console.WriteLine("How mouch objtacle : ");
            int obj = Convert.ToInt32(Console.ReadLine());
            for(int i=0; i<obj; i++) // this loop create some objatacle into grid 
            {
                int x = i;
                ++x;
                Console.Write("Objtacle {0} posI : ",x);
                int I = Convert.ToInt32(Console.ReadLine());
                Console.Write("Objtacle {0} posJ : ", x);
                int J = Convert.ToInt32(Console.ReadLine());
                Close.Add(new Node(I, J)); // here we added our obstacle location as like objtacle node into our closelist 
            }

            //here we set our start node 
            grid[StartPosI, StartPosJ].gCost = 0;
            grid[StartPosI, StartPosJ].hCost = HCostcalculate(StartPosI, StartPosJ, EndPosI, EndPosJ);
            grid[StartPosI, StartPosJ].setFcost();

            Open.Add(grid[StartPosI, StartPosJ]); // And added into our open list

            while(Open.Count > 0) // this loop search the shortest path 
            {
                var minmum = Open.Min(F => F.fCost); // here we check which node have minimum number with LINQ quary 
                Parent = Open.First(F => F.fCost == minmum); // here we check the which node are first position in our Openlist with minimum F cost.

                //fill East Node
                if(Parent.posJ + 1 < gridSize)
                {
                    if(Parent.posI == EndPosI && Parent.posJ == EndPosJ)
                    {
                        grid[Parent.posI, Parent.posJ + 1].Parent = Parent;
                        LastNode = grid[Parent.posI, Parent.posJ + 1];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI, Parent.posJ + 1]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI, Parent.posJ + 1]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI, Parent.posJ + 1]);
                                int f1 = Parent.gCost + sideCost;
                                f1 += HCostcalculate(Parent.posI, Parent.posJ + 1, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI, Parent.posJ + 1].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI, Parent.posJ + 1].hCost = HCostcalculate(Parent.posI, Parent.posJ + 1, EndPosI, EndPosJ);
                                    grid[Parent.posI, Parent.posJ + 1].fCost = f1;
                                    grid[Parent.posI, Parent.posJ + 1].Parent = Parent;

                                    Open.Add(grid[Parent.posI, Parent.posJ + 1]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI, Parent.posJ + 1].gCost = Parent.gCost + sideCost;
                                grid[Parent.posI, Parent.posJ + 1].hCost = HCostcalculate(Parent.posI, Parent.posJ + 1, EndPosI, EndPosJ);
                                grid[Parent.posI, Parent.posJ + 1].setFcost();
                                grid[Parent.posI, Parent.posJ + 1].Parent = Parent;

                                Open.Add(grid[Parent.posI, Parent.posJ + 1]);
                            }
                        }
                    }

                }
                //fill North-East Node
                if(Parent.posI -1 >= 0 && Parent.posJ + 1 < gridSize)
                {
                    if (Parent.posI - 1 == EndPosI && Parent.posJ + 1 == EndPosJ)
                    {
                        grid[Parent.posI - 1, Parent.posJ + 1].Parent = Parent;
                        LastNode = grid[Parent.posI -1, Parent.posJ + 1];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI - 1, Parent.posJ + 1]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI - 1, Parent.posJ + 1]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI - 1, Parent.posJ + 1]);
                                int f1 = Parent.gCost + diagonalCost;
                                f1 += HCostcalculate(Parent.posI - 1, Parent.posJ + 1, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI - 1, Parent.posJ + 1].gCost = Parent.gCost + diagonalCost;
                                    grid[Parent.posI - 1, Parent.posJ + 1].hCost = HCostcalculate(Parent.posI - 1, Parent.posJ + 1, EndPosI, EndPosJ);
                                    grid[Parent.posI - 1, Parent.posJ + 1].fCost = f1;
                                    grid[Parent.posI - 1, Parent.posJ + 1].Parent = Parent;

                                    Open.Add(grid[Parent.posI - 1, Parent.posJ + 1]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI - 1, Parent.posJ + 1].gCost = Parent.gCost + diagonalCost;
                                grid[Parent.posI - 1, Parent.posJ + 1].hCost = HCostcalculate(Parent.posI - 1, Parent.posJ + 1, EndPosI, EndPosJ);
                                grid[Parent.posI - 1, Parent.posJ + 1].setFcost();
                                grid[Parent.posI - 1, Parent.posJ + 1].Parent = Parent;

                                Open.Add(grid[Parent.posI - 1, Parent.posJ + 1]);
                            }
                        }
                    }
                }
                //fill North Node
                if(Parent.posI-1 >= 0)
                {
                    if (Parent.posI - 1 == EndPosI && Parent.posJ == EndPosJ)
                    {
                        grid[Parent.posI - 1, Parent.posJ].Parent = Parent;
                        LastNode = grid[Parent.posI - 1, Parent.posJ];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI - 1, Parent.posJ]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI - 1, Parent.posJ]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI - 1, Parent.posJ]);
                                int f1 = Parent.gCost + sideCost;
                                f1 += HCostcalculate(Parent.posI - 1, Parent.posJ, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI - 1, Parent.posJ].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI - 1, Parent.posJ].hCost = HCostcalculate(Parent.posI - 1, Parent.posJ, EndPosI, EndPosJ);
                                    grid[Parent.posI - 1, Parent.posJ].fCost = f1;
                                    grid[Parent.posI - 1, Parent.posJ].Parent = Parent;

                                    Open.Add(grid[Parent.posI - 1, Parent.posJ]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI - 1, Parent.posJ].gCost = Parent.gCost + sideCost;
                                grid[Parent.posI - 1, Parent.posJ].hCost = HCostcalculate(Parent.posI - 1, Parent.posJ, EndPosI, EndPosJ);
                                grid[Parent.posI - 1, Parent.posJ].setFcost();
                                grid[Parent.posI - 1, Parent.posJ].Parent = Parent;

                                Open.Add(grid[Parent.posI - 1, Parent.posJ]);
                            }
                        }
                    }
                }
                //fill North-Wast Node
                if(Parent.posI-1 >= 0 && Parent.posJ - 1 >= 0)
                {
                    if (Parent.posI - 1 == EndPosI && Parent.posJ - 1 == EndPosJ)
                    {
                        grid[Parent.posI - 1, Parent.posJ - 1].Parent = Parent;
                        LastNode = grid[Parent.posI - 1, Parent.posJ - 1];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI - 1, Parent.posJ - 1]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI - 1, Parent.posJ - 1]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI - 1, Parent.posJ - 1]);
                                int f1 = Parent.gCost + diagonalCost;
                                f1 += HCostcalculate(Parent.posI - 1, Parent.posJ - 1, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI - 1, Parent.posJ - 1].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI - 1, Parent.posJ - 1].hCost = HCostcalculate(Parent.posI - 1, Parent.posJ - 1, EndPosI, EndPosJ);
                                    grid[Parent.posI - 1, Parent.posJ - 1].fCost = f1;
                                    grid[Parent.posI - 1, Parent.posJ - 1].Parent = Parent;

                                    Open.Add(grid[Parent.posI - 1, Parent.posJ - 1]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI - 1, Parent.posJ - 1].gCost = Parent.gCost + diagonalCost;
                                grid[Parent.posI - 1, Parent.posJ - 1].hCost = HCostcalculate(Parent.posI - 1, Parent.posJ - 1, EndPosI, EndPosJ);
                                grid[Parent.posI - 1, Parent.posJ - 1].setFcost();
                                grid[Parent.posI - 1, Parent.posJ - 1].Parent = Parent;

                                Open.Add(grid[Parent.posI - 1, Parent.posJ - 1]);
                            }
                        }
                    }
                }
                //fill Wast Node
                if (Parent.posJ - 1 >= 0)
                {
                    if (Parent.posI == EndPosI && Parent.posJ - 1 == EndPosJ)
                    {
                        grid[Parent.posI, Parent.posJ - 1].Parent = Parent;
                        LastNode = grid[Parent.posI, Parent.posJ - 1];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI, Parent.posJ - 1]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI, Parent.posJ - 1]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI, Parent.posJ - 1]);
                                int f1 = Parent.gCost + sideCost;
                                f1 += HCostcalculate(Parent.posI, Parent.posJ - 1, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI, Parent.posJ - 1].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI, Parent.posJ - 1].hCost = HCostcalculate(Parent.posI, Parent.posJ - 1, EndPosI, EndPosJ);
                                    grid[Parent.posI, Parent.posJ - 1].fCost = f1;
                                    grid[Parent.posI, Parent.posJ - 1].Parent = Parent;

                                    Open.Add(grid[Parent.posI, Parent.posJ - 1]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI, Parent.posJ - 1].gCost = Parent.gCost + sideCost;
                                grid[Parent.posI, Parent.posJ - 1].hCost = HCostcalculate(Parent.posI, Parent.posJ - 1, EndPosI, EndPosJ);
                                grid[Parent.posI, Parent.posJ - 1].setFcost();
                                grid[Parent.posI, Parent.posJ - 1].Parent = Parent;

                                Open.Add(grid[Parent.posI, Parent.posJ - 1]);
                            }
                        }
                    }
                }
                //fill South-Wast
                if (Parent.posI + 1 < gridSize && Parent.posJ-1 >= 0)
                {
                    if (Parent.posI + 1 == EndPosI && Parent.posJ - 1 == EndPosJ)
                    {
                        grid[Parent.posI + 1, Parent.posJ - 1].Parent = Parent;
                        LastNode = grid[Parent.posI + 1, Parent.posJ - 1];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI + 1, Parent.posJ - 1]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI + 1, Parent.posJ - 1]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI + 1, Parent.posJ - 1]);
                                int f1 = Parent.gCost + diagonalCost;
                                f1 += HCostcalculate(Parent.posI + 1, Parent.posJ - 1, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI + 1, Parent.posJ - 1].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI + 1, Parent.posJ - 1].hCost = HCostcalculate(Parent.posI + 1, Parent.posJ - 1, EndPosI, EndPosJ);
                                    grid[Parent.posI + 1, Parent.posJ - 1].fCost = f1;
                                    grid[Parent.posI + 1, Parent.posJ - 1].Parent = Parent;

                                    Open.Add(grid[Parent.posI + 1, Parent.posJ - 1]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI + 1, Parent.posJ - 1].gCost = Parent.gCost + diagonalCost;
                                grid[Parent.posI + 1, Parent.posJ - 1].hCost = HCostcalculate(Parent.posI + 1, Parent.posJ - 1, EndPosI, EndPosJ);
                                grid[Parent.posI + 1, Parent.posJ - 1].setFcost();
                                grid[Parent.posI + 1, Parent.posJ - 1].Parent = Parent;

                                Open.Add(grid[Parent.posI + 1, Parent.posJ - 1]);
                            }
                        }
                    }
                }
                //fill South Node
                if (Parent.posI + 1 < gridSize)
                {
                    if (Parent.posI + 1 == EndPosI && Parent.posJ == EndPosJ)
                    {
                        grid[Parent.posI + 1, Parent.posJ].Parent = Parent;
                        LastNode = grid[Parent.posI + 1, Parent.posJ];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI + 1, Parent.posJ]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI + 1, Parent.posJ]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI + 1, Parent.posJ]);
                                int f1 = Parent.gCost + sideCost;
                                f1 += HCostcalculate(Parent.posI + 1, Parent.posJ, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI + 1, Parent.posJ].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI + 1, Parent.posJ].hCost = HCostcalculate(Parent.posI + 1, Parent.posJ, EndPosI, EndPosJ);
                                    grid[Parent.posI + 1, Parent.posJ].fCost = f1;
                                    grid[Parent.posI + 1, Parent.posJ].Parent = Parent;

                                    Open.Add(grid[Parent.posI + 1, Parent.posJ]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI + 1, Parent.posJ].gCost = Parent.gCost + sideCost;
                                grid[Parent.posI + 1, Parent.posJ].hCost = HCostcalculate(Parent.posI + 1, Parent.posJ, EndPosI, EndPosJ);
                                grid[Parent.posI + 1, Parent.posJ].setFcost();
                                grid[Parent.posI + 1, Parent.posJ].Parent = Parent;

                                Open.Add(grid[Parent.posI + 1, Parent.posJ]);
                            }
                        }
                    }
                }
                //fill South-East Node
                if(Parent.posI+1<gridSize && Parent.posJ + 1 < gridSize)
                {
                    if (Parent.posI + 1 == EndPosI && Parent.posJ + 1 == EndPosJ)
                    {
                        grid[Parent.posI + 1, Parent.posJ + 1].Parent = Parent;
                        LastNode = grid[Parent.posI + 1, Parent.posJ + 1];
                        break;
                    }
                    else
                    {
                        if (!NodeCloseListOrNot(ref Close, grid[Parent.posI + 1, Parent.posJ + 1]))
                        {
                            if (NodeOpneListOrNot(ref Open, grid[Parent.posI + 1, Parent.posJ + 1]))
                            {
                                int f = GetTheFValue(ref Open, grid[Parent.posI + 1, Parent.posJ + 1]);
                                int f1 = Parent.gCost + diagonalCost;
                                f1 += HCostcalculate(Parent.posI + 1, Parent.posJ + 1, EndPosI, EndPosJ);
                                if (f1 < f)
                                {
                                    grid[Parent.posI + 1, Parent.posJ + 1].gCost = Parent.gCost + sideCost;
                                    grid[Parent.posI + 1, Parent.posJ + 1].hCost = HCostcalculate(Parent.posI + 1, Parent.posJ + 1, EndPosI, EndPosJ);
                                    grid[Parent.posI + 1, Parent.posJ + 1].fCost = f1;
                                    grid[Parent.posI + 1, Parent.posJ + 1].Parent = Parent;

                                    Open.Add(grid[Parent.posI + 1, Parent.posJ + 1]);
                                }
                            }
                            else
                            {
                                grid[Parent.posI + 1, Parent.posJ + 1].gCost = Parent.gCost + diagonalCost;
                                grid[Parent.posI + 1, Parent.posJ + 1].hCost = HCostcalculate(Parent.posI + 1, Parent.posJ + 1, EndPosI, EndPosJ);
                                grid[Parent.posI + 1, Parent.posJ + 1].setFcost();
                                grid[Parent.posI + 1, Parent.posJ + 1].Parent = Parent;

                                Open.Add(grid[Parent.posI + 1, Parent.posJ + 1]);
                            }
                        }
                    }
                }


                Close.Add(Parent); // here we added the parent node into closelist
                RemoveDoneNode(ref Open, Parent); // here we remove the parent node from openlist

            }


            while(true) // this loop find the the path from goal point to start point
            {
                if(LastNode.posI == StartPosI && LastNode.posJ == StartPosJ)
                {
                    break;
                }
                if (LastNode.posI == -1 && LastNode.posJ == -1)
                {
                    Console.WriteLine("Can't find path");
                    break;
                }

                //Console.WriteLine(LastNode.posI + "," + LastNode.posJ);
                FinalPath.Push(LastNode);
                LastNode = LastNode.Parent;
            }
            //Console.WriteLine(LastNode.posI + "," + LastNode.posJ);
            FinalPath.Push(LastNode); // This final shortest path from start point to goal point
            for(int i=0; i<FinalPath.Count; i++) // this loop show the final path
            {
                Console.WriteLine(FinalPath.ElementAt(i).posI + "," + FinalPath.ElementAt(i).posJ);
            }
            Console.ReadLine();
        }

        static void RemoveDoneNode(ref List<Node> Open, Node parent) // this method are using for remove the parent node from openlist
        {
            for(int i=0; i<Open.Count; i++)
            {
                if(Open.ElementAt(i).posI == parent.posI && Open.ElementAt(i).posJ == parent.posJ)
                {
                    Open.RemoveAt(i);
                    break;
                }
            }
        }
        static int HCostcalculate(int NodeX, int NodeY, int endX, int endY) // this method for calculating the H cost
        {
            return Math.Abs(endX - NodeX) + Math.Abs(endY - NodeY);
        }

        static bool NodeCloseListOrNot(ref List<Node> closeList,Node child) // this method check the node are in close list or not
        {
            bool b = false;
            foreach(var n in closeList)
            {
                if(n.posI == child.posI && n.posJ == child.posJ)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        static bool NodeOpneListOrNot(ref List<Node> openList, Node child) // this method check the node are in openlist or not
        {
            bool b = false;
            foreach (var n in openList)
            {
                if (n.posI == child.posI && n.posJ == child.posJ)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        static int GetTheFValue(ref List<Node> OpenList, Node child) // this method are using for get the F cost value from child
        {
            int value = 0;
            foreach (var n in OpenList)
            {
                if (n.posI == child.posI && n.posJ == child.posJ)
                {
                    value = n.fCost;
                    break;
                }
            }

            return value;
        }
    }
}
