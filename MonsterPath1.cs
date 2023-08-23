using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Monster_s_path
{

    class Player
    {
        public int[] coords;
        public char type;
        public bool turned;

        public Player(int[] coords, char type)
        {
            this.coords = coords;
            this.type = type;
        }
    }

    internal class Program
    {


        public static void printer(char[,] labyrinth, int height, int width)
        {

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(labyrinth[i, j]);
                }
                Console.Write("\n");

            }
            Console.WriteLine();
        }


        public static int[] find_first_position(char[,] lab, int height, int width)
        {
            Dictionary<char, int[]> player_type = new Dictionary<char, int[]>()
            {
                {'v',null},
                {'^',null},
                {'>',null},
                {'<',null},
            };

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    char p = lab[i, j];
                    if (player_type.ContainsKey(p))
                    {
                        player_type[p] = new int[] { i, j };
                    }

                }
            }

            foreach (int[] pos in player_type.Values)
            {
                if (pos != null)
                {
                    return pos;
                }
            }

            return new int[0];
        }

        public static char[,] forward(char[,] lab, char player, int[] player_pos)
        {
            Dictionary<char, int[]> movements = new Dictionary<char, int[]>()
            {
                {'>',new int [] { 0,1} },
                {'^',new int[] {-1,0} },
                {'<',new int[] {0,-1}},
                {'v',new int [] {1,0} }
            };

            if (movements.ContainsKey(player))
            {
                lab[player_pos[0], player_pos[1]] = '.';
                player_pos[0] += movements[player][0];
                player_pos[1] += movements[player][1];
            }

            lab[player_pos[0], player_pos[1]] = player;

            return lab;
        }

        public static char[,] turn_left(char[,] lab, Player player)
        {
            Dictionary<char, char> player_type = new Dictionary<char, char>()
            {
                {'>','^'},
                {'^','<'},
                {'<','v'},
                {'v','>'}
            };

            if (player_type.ContainsKey(player.type))
            {
                player.type = player_type[player.type];
            }
            lab[player.coords[0], player.coords[1]] = player.type;
            return lab;

        }

        public static char[,] turn_right(char[,] lab, Player player)
        {
            Dictionary<char, char> player_type = new Dictionary<char, char>()
            {
                {'>','v'},
                {'^','>'},
                {'<','^'},
                {'v','<'}
            };

            if (player_type.ContainsKey(player.type))
            {
                player.type = player_type[player.type];
            }
            lab[player.coords[0], player.coords[1]] = player.type;

            return lab;
        }

        public static bool check_wall_front(char[,] lab, Player player)
        {
            Dictionary<char, int[]> movements = new Dictionary<char, int[]>()
            {
                {'>',new int [] { 0,1} },
                {'^',new int[] {-1,0} },
                {'<',new int[] {0,-1}},
                {'v',new int [] {1,0} }
            };

            if (movements.ContainsKey(player.type))
            {
                if (lab[player.coords[0] + movements[player.type][0], player.coords[1] + movements[player.type][1]] == 'X')
                {

                    return true;
                }
            }


            return false;
        }


        public static bool check_wall_right(char[,] lab, Player player)
        {
            Dictionary<char, int[]> movements = new Dictionary<char, int[]>()
            {
                {'>',new int [] { 1,0} },
                {'^',new int[] {0,1} },
                {'<',new int[] {-1,0}},
                {'v',new int [] {0,-1} }
            };

            if (movements.ContainsKey(player.type))
            {
                if (lab[player.coords[0] + movements[player.type][0], player.coords[1] + movements[player.type][1]] == 'X')
                {
                    return true;
                }

            }


            return false;
        }
        

        static void Main(string[] args)
        {
            

            int width = int.Parse(Console.ReadLine());
            int height = int.Parse(Console.ReadLine());

            char[,] labyrinth;
            labyrinth = new char[height, width];


            for (int i = 0; i < height; i++)
            {
                string line = Console.ReadLine();
                for (int j = 0; j < width; j++)
                {
                    labyrinth[i, j] = line[j];
                }
            }
            

            int[] player_pos = find_first_position(labyrinth, height, width);
            char player = labyrinth[player_pos[0], player_pos[1]];

            Player hrac = new Player(player_pos,player);

            int counter = 0;
            while (counter != 20)

            {

                player_pos = find_first_position(labyrinth, height, width);
                player = labyrinth[player_pos[0], player_pos[1]];
                hrac.coords= player_pos;
                hrac.type = player;

                if ((check_wall_front(labyrinth,hrac) == false && check_wall_right(labyrinth, hrac) == true) || hrac.turned == true)
                {

                    labyrinth = forward(labyrinth, hrac.type, hrac.coords);
                    printer(labyrinth, height, width);
                    counter++;
                    hrac.turned = false;
                }
                else
                {
                    if (check_wall_front(labyrinth, hrac) == true && check_wall_right(labyrinth, hrac) == true)
                    {
                        labyrinth = turn_left(labyrinth, hrac);
                        printer(labyrinth, height, width);
                        counter++;
                    }
                    else
                    {
                        if (check_wall_right(labyrinth, hrac) == false)
                        {
                            labyrinth = turn_right(labyrinth, hrac);
                            printer(labyrinth, height, width);
                            counter++;
                            hrac.turned = true;
                        }

                    }
                }

            }

        }
    }
}