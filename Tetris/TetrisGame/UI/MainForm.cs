using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TetrisGame.GameEngine;

namespace TetrisGame.UI
{
    public class MainForm : Form
    {
        private Timer timer;
        private TetrisGrid tetrisGrid;
        private TetrisPiece currentPiece;
        private Random random;

        public MainForm()
        {
            InitializeComponent();
            tetrisGrid = new TetrisGrid();
            random = new Random();
            currentPiece = GenerateRandomPiece();

            timer = new Timer();
            timer.Interval = 500; // Vitesse du jeu (ms)
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            GameLoop();
        }

        private void GameLoop()
        {
            if (!currentPiece.MovePiece(tetrisGrid, 1, 0))
            {
                tetrisGrid.PlacePiece(currentPiece);

                var fullRows = tetrisGrid.CheckFullRows();
                if (fullRows.Count > 0)
                {
                    tetrisGrid.ClearRows(fullRows);
                }

                currentPiece = GenerateRandomPiece();

                if (!tetrisGrid.CanPlacePiece(currentPiece))
                {
                    timer.Stop();
                    MessageBox.Show("Game Over!");
                }
            }

            Invalidate();
        }

        private TetrisPiece GenerateRandomPiece()
        {
            var pieces = new List<int[,]>
            {
                new int[,] // Carré (2x2)
                {
                    { 1, 1 },
                    { 1, 1 }
                },
                new int[,] // Ligne (4x1)
                {
                    { 1, 1, 1, 1 }
                },
                new int[,] // Forme L
                {
                    { 1, 0 },
                    { 1, 0 },
                    { 1, 1 }
                },
                new int[,] // Forme T
                {
                    { 0, 1, 0 },
                    { 1, 1, 1 }
                },
                new int[,] // Forme Z
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 }
                }
            };

            var randomIndex = random.Next(pieces.Count);
            return new TetrisPiece(pieces[randomIndex]);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (tetrisGrid.IsCellOccupied(row, col))
                    {
                        g.FillRectangle(Brushes.Blue, col * 20, row * 20, 20, 20);
                    }
                }
            }

            for (int i = 0; i < currentPiece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < currentPiece.Shape.GetLength(1); j++)
                {
                    if (currentPiece.Shape[i, j] != 0)
                    {
                        int x = (currentPiece.Column + j) * 20;
                        int y = (currentPiece.Row + i) * 20;
                        g.FillRectangle(Brushes.Red, x, y, 20, 20);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.ClientSize = new Size(200, 400);
            this.Text = "Tetris";
            this.DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    currentPiece.MovePiece(tetrisGrid, 0, -1);
                    break;
                case Keys.Right:
                    currentPiece.MovePiece(tetrisGrid, 0, 1);
                    break;
                case Keys.Down:
                    currentPiece.MovePiece(tetrisGrid, 1, 0);
                    break;
                case Keys.Space: // Rotation
                    currentPiece.TryRotate(tetrisGrid);
                    break;
            }

            Invalidate();
        }
    }
}
