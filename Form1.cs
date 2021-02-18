using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeReplica
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();// array list 4 snake
        private Circle food = new Circle();// creating a single Circle class called food
        public Form1()
        {
            InitializeComponent();
            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += updateScreen;
            gameTimer.Start();

            startGame();
        }

        private void movePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0) // if snake head active
                {
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXpos || Snake[i].Y > maxYpos)
                    {
                        die(); //end game if snake reaches edge of canvas
                    }

                    //for will check if snake had collision with another parts of his body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            //if yes, die
                            die();
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                    }
                }

                else
                {
                    //if no collisions happen
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void updateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                //each tick runs here
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if(Input.KeyPress(Keys.Right)&&Settings.direction!=Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if(Input.KeyPress(Keys.Left)&&Settings.direction!=Directions.Right)
                    {
                    Settings.direction = Directions.Left;
                    }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down) 
                    {
                    Settings.direction = Directions.Up;
                    }
                else if(Input.KeyPress(Keys.Down)&&Settings.direction!=Directions.Up)
                    {
                    Settings.direction = Directions.Down;
                    }

                movePlayer();
            }

            pbCanvas.Invalidate(); //refresh picture & update graphics
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //will trigger change state
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            //snake and moving parts
            Graphics canvas = e.Graphics; //new graphics class named canvas
            
            if(Settings.GameOver==false)
            {
                //if game is not over, execute below
                Brush snakeColour;

                //run loop to check snake parts
                for(int i=0;i< Snake.Count;i++)
                {
                    if(i==0)
                    {
                        snakeColour = Brushes.Black; // snake head
                    }
                    else
                    {
                        snakeColour = Brushes.Green; //snake body
                    }

                    canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height,Settings.Width,Settings.Height));

                    //draw food
                    canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width,Settings.Height));
                }
            }
            else
            {
                //will run when game is over
                string gameOver = "Game Over\n" + "Final score is " + Settings.Score + "\n Press enter to continue\n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }

        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 }; //new head
            Snake.Add(head); //add head to snake array

            label2.Text = Settings.Score.ToString();

            generateFood();

        }

        private void generateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width; //create max X pos within half of the play size
            int maxYpos = pbCanvas.Size.Height / Settings.Height; //create max Y
            Random rnd = new Random();
            food = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
            //new food with random coords (X,Y)

        }
        private void eat()
        {
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y                
            };

            Snake.Add(body);
            Settings.Score += Settings.Points; // increase game score
            label2.Text = Settings.Score.ToString(); // show score on label 2
            generateFood();

        }
        private void die()
        {
            //change to bool true
            Settings.GameOver = true;
        }
    }
}
