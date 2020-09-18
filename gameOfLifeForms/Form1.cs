using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameOfLifeForms
{
    public partial class Form1 : Form
    {
        bool[,] world = new bool[400, 400];

        BackgroundWorker worker = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(400, 400);

            worker.DoWork += Worker_DoWork;

            MakeWorld(7000);

            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                Revolution(world);

                var b = new Bitmap(400, 400);

                for (int y = 0; y < world.GetLength(1); y++)
                {
                    for (int x = 0; x < world.GetLength(0); x++)
                    {
                        if (world[x,y])
                        {
                            b.SetPixel(x, y, Color.Black);
                        }
                        else
                        {
                            b.SetPixel(x, y, Color.White);
                        }
                    }
                }

                pictureBox1.Image = b;

                pictureBox1.Refresh();

                Thread.Sleep(10);
            } while (true);
        }

        void MakeWorld(int cells)
        {

            const int Y = 400;
            const int X = 400;


            for (int i = 0; i < cells; i++)
            {
                Random random = new Random();

                bool isNotChanged = true;

                while (isNotChanged)
                {

                    int y = random.Next(0, Y);
                    int x = random.Next(0, X);

                    if (world[y, x] == false)
                    {

                        world[y, x] = true;
                        isNotChanged = false;

                    }
                }
            }
        }

        int GetAliveCells()
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

        void PrintWorld(bool[,] World)
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

        void Revolution(bool[,] World)
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
        bool UpdateCell(bool[,] World, int startY, int startX)
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
