//Write a program finding a shortest path with a chess king on a chessboard 8x8 where several squares cannot be accessed (by the king).

//Input is given in this ordering:

//Number of obstacles
//Coordinates of the obstacles (pairs of numbers 1.. 8)
//Coordinates of the starting square
//Coordinates of the end square.
//Number of the obstacles is on a separate line, obstacles are described each on a separate line (i.e., one pair of numbers on a line). On a line the numbers are separated by the space-character.

//Output is either -1 (if the king cannot reach the end-square) or number of steps that the king has to perform.

using System;
using System.Runtime.ExceptionServices;
using System.Collections.Generic;

namespace Kings_s_path
{
    class Reader
    {
        public static int ReadSymbol()
        {
            int previous = '?';
            int symbol = Console.Read();
            while ((symbol < '0') || (symbol > '9'))
            {
                previous = symbol;
                symbol = Console.Read();
            }
            int x = 0;

            while ((symbol >= '0') && (symbol <= '9'))
            {
                x = 10 * x + (symbol - '0');
                symbol = Console.Read();
            }
            if (previous == '-')
                x = -x;
            return x;
        }
    }
    
    class Chessboards
    {
        public static int[,] pure_chessboard()
        {
           
            int[,] chessboard = new int[8,8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chessboard[i, j] = 0;
                }

            }
            return chessboard;
        }

    }

    internal class Program
    {
        static public int BFS(int[] start,int[,] chessboard)
        {
            List<int[]> visited = new List<int[]> ();
            Queue<int[]> my_queue = new Queue<int[]>();

            visited.Add(start);
            my_queue.Enqueue(start);
            while (my_queue.Count > 0)
            {
                int[] x = my_queue.Dequeue();
                List<int[]> positions = new List<int[]>();

                positions.Add(new int[] { x[0], x[1] + 1, x[2] + 1 });
                positions.Add(new int[] { x[0], x[1] - 1, x[2] + 1 });
                positions.Add(new int[] { x[0] + 1, x[1], x[2] + 1 });
                positions.Add(new int[] { x[0] - 1, x[1], x[2] + 1 });
                positions.Add(new int[] { x[0] + 1, x[1] + 1, x[2] + 1 });
                positions.Add(new int[] { x[0] - 1, x[1] - 1, x[2] + 1 });
                positions.Add(new int[] { x[0] + 1, x[1] - 1, x[2] + 1 });
                positions.Add(new int[] { x[0] - 1, x[1] + 1, x[2] + 1 });
            

                foreach (int[] pos in positions)
                {
                    if (pos[0] > -1 && pos[0] < 8 && pos[1] > -1 && pos[1] < 8)
                    {
                        if (chessboard[pos[0], pos[1]] == 2)
                        {
                            return pos[2];
                        }
                        if (chessboard[pos[0], pos[1]] != -1)
                        {

                            if (!visited.Exists(a => a[0] == pos[0] && a[1] == pos[1]))
                            {
                                my_queue.Enqueue(pos);
                                visited.Add(new int[] { pos[0], pos[1] });
                            }
                        }
                    }
                }
            }
            return -1;
        }
        static void Main(string[] args)
        {
            int obstacles_count = Reader.ReadSymbol();
            int[,] chessboard = Chessboards.pure_chessboard();

            //adding obstacles
            List<int[]> obstacles = new List<int[]>();
            for(int i = 0; i < obstacles_count; i++)
            {
                int os_x = Reader.ReadSymbol() - 1;
                int os_y = Reader.ReadSymbol() - 1;
                obstacles.Add(new int[] { os_x,os_y});
            }

            //adding start and end position
            int[] start = new int[] { };
            int start_1 = Reader.ReadSymbol() - 1;
            int start_2 = Reader.ReadSymbol() - 1;
            chessboard[start_1, start_2] = 1;
            start = new int[] { start_1, start_2, 0 };

            int end_1 = Reader.ReadSymbol() - 1;
            int end_2 = Reader.ReadSymbol() - 1;
            chessboard[end_1, end_2] = 2;

            foreach (int[] obstacle in obstacles)
            {
                chessboard[obstacle[0],obstacle[1]] = -1;
            }



            int answer = BFS(start, chessboard);
            Console.WriteLine(answer);
           
        }
    }
}
