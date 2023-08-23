using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;

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


        public static List<int[]> find_first_position(char[,] lab, int height, int width)
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
                        if (player_type[p] == null)
                        {

                            player_type[p] = new int[] { i, j };
                        }
                        else
                        {
                            int[] arr = new int[player_type[p].Length + 2];
                            for (int q = 0; q < player_type[p].Length; q++)
                            {
                                arr[q] = player_type[p][q];
                            };
                            arr[arr.Length - 2] = i;
                            arr[arr.Length - 1] = j;

                            player_type[p] = arr;


                        }

                    }

                }
            }

            List<int[]> positions = new List<int[]> { };
            foreach (int[] pos in player_type.Values)
            {
                if (pos != null)
                {
                    positions.Add(pos);
                }
            }

            return positions;
        }

        public static char[,] forward(char[,] lab, Player player)
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
                lab[player.coords[0], player.coords[1]] = '.';
                player.coords[0] += movements[player.type][0];
                player.coords[1] += movements[player.type][1];
            }

            lab[player.coords[0], player.coords[1]] = player.type;

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

        public static bool check_wall_front(char[,] lab, Player player, Player player2)
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
                if (lab[player.coords[0] + movements[player.type][0], player.coords[1] + movements[player.type][1]] == 'X'
                    || (lab[player.coords[0] + movements[player.type][0], player.coords[1] + movements[player.type][1]] == player2.type))
                {

                    return true;
                }
            }


            return false;
        }


        public static bool check_wall_right(char[,] lab, Player player, Player player2)
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
                if (lab[player.coords[0] + movements[player.type][0], player.coords[1] + movements[player.type][1]] == 'X'
                    || (lab[player.coords[0] + movements[player.type][0], player.coords[1] + movements[player.type][1]] == player2.type))
                {
                    return true;
                }

            }


            return false;
        }

        public static void move(char[,] labyrinth,int height,int width,Player hrac, Player hrac2)
        {
            if ((check_wall_front(labyrinth, hrac, hrac2) == false && check_wall_right(labyrinth, hrac,hrac2) == true) || hrac.turned == true)
            {

                labyrinth = forward(labyrinth, hrac);
                hrac.turned = false;
                return;
            }
            else
            {
                if (check_wall_front(labyrinth, hrac, hrac2) == true && check_wall_right(labyrinth, hrac, hrac2) == true)
                {
                    labyrinth = turn_left(labyrinth, hrac);
                    return;
                }
                else
                {
                    if (check_wall_right(labyrinth, hrac, hrac2) == false)
                    {
                        labyrinth = turn_right(labyrinth, hrac);
                        hrac.turned = true;
                        return;
                    }

                }
            }

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
            

            List<int[]> players_pos = find_first_position(labyrinth, height, width);
            try
            {
                int[] player_1_pos = new int[] { players_pos[0][0], players_pos[0][1] };
                int[] player_2_pos = new int[] { players_pos[1][0], players_pos[1][1] };
                char player_1 = labyrinth[players_pos[0][0], players_pos[0][1]];
                char player_2 = labyrinth[players_pos[1][0], players_pos[1][1]];
                Player hrac = new Player(player_1_pos, player_1);
                Player hrac2 = new Player(player_2_pos, player_2);

                int counter = 0;
                while (counter != 20)

                {

                    move(labyrinth, height, width, hrac, hrac2);
                    move(labyrinth, height, width, hrac2, hrac);
                    printer(labyrinth, height, width);
                    counter++;
                }

            }
            catch (System.ArgumentOutOfRangeException)
            {
                int[] player_1_pos = new int[] { players_pos[0][0], players_pos[0][1] };
                int[] player_2_pos = new int[] { players_pos[0][2], players_pos[0][3] };
                char player_1 = labyrinth[players_pos[0][0], players_pos[0][1]];
                char player_2 = labyrinth[players_pos[0][2], players_pos[0][3]];

                Player hrac = new Player(player_1_pos, player_1);
                Player hrac2 = new Player(player_2_pos, player_2);

                int counter = 0;
                while (counter != 20)

                {

                    move(labyrinth, height, width, hrac, hrac2);
                    move(labyrinth, height, width, hrac2, hrac);
                    printer(labyrinth, height, width);
                    counter++;
                }
            }


                
           

           

        }
    }
}