using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] world = MakeWorld(GetAliveCells());

            while (ArePeopleAlive(world))
            {
                Console.CursorVisible = false;

                PrintWorld(world);

                Revolution(world);

                Thread.Sleep(10);  
            }

            Console.Clear();

            Console.WriteLine("All people are dead!");

            Console.Read();
        }

        static bool ArePeopleAlive(bool[,] world)
        {
            foreach (bool item in world)
            {
                if (item)
                {
                    return true;
                }
            }

            return false;
        }

        static bool[,] MakeWorld(int cells)
        {

            const int Y = 20;
            const int X = 79;

            bool[,] World = new bool[Y, X];

            for (int i = 0; i < cells; i++)
            {
                Random random = new Random();

                bool isNotChanged = true;

                while (isNotChanged)
                {

                    int y = random.Next(0, Y);
                    int x = random.Next(0, X);

                    if (World[y, x] == false)
                    {

                        World[y, x] = true;
                        isNotChanged = false;

                    }
                }
            }

            return World;
        }

        static int GetAliveCells()
        {
            const int MAXSTARTINGCELLS = 1580;

            while (true)
            {
                Console.WriteLine("Wieviele Zellen sollen belegt werden <Max:{0}>", MAXSTARTINGCELLS);
                int World = 0;


                bool isValid = int.TryParse(Console.ReadLine(), out World) && World > 0 && World <= MAXSTARTINGCELLS;

                if (isValid)
                {
                    return World;
                }
            }
        }

        static void PrintWorld(bool[,] World)
        {


            for (int y = 0; y < World.GetLength(0); y++)
            {
                for (int x = 0; x < World.GetLength(1); x++)
                {
                    Console.SetCursorPosition(x, y);

                    if (World[y, x])
                    {
                        Console.WriteLine("X");
                    }
                    else
                    {
                        Console.WriteLine(" ");
                    }
                }
            }
        }

        static void Revolution(bool[,] World)
        {
            bool[,] previousWorld = World;


            for (int y = 0; y < World.GetLength(0); y++)
            {
                for (int x = 0; x < World.GetLength(1); x++)
                {
                    World[y, x] = UpdateCell(previousWorld, y, x);
                }
            }
        }
        static bool UpdateCell(bool[,] World, int startY, int startX)
        {
            int coutNeighbours = 0;

            for (int y = startY - 1; y < World.GetLength(0) && y < startY + 2; y++)
            {

                if (y >= 0)
                {

                    for (int x = startX - 1; x < World.GetLength(1) && x < startX + 2; x++)
                    {

                        if (x >= 0 && (x != startX || y != startY))
                        {

                            if (World[y, x])
                            {
                                coutNeighbours++;
                            }
                        }
                    }
                }
            }
            if (coutNeighbours < 2 || coutNeighbours >= 4)
            {

                return false;

            }
            else if (coutNeighbours == 2)
            {

                return World[startY, startX];

            }

            return true;
        }
    }
}
