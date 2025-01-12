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
        private Label scoreLabel;

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
                    Console.WriteLine("Game Over! The game will now shut down.");
                    System.Threading.Thread.Sleep(2000); // Pause de 2 secondes
                    Environment.Exit(0); // Ferme l'application après la pause
                }
            }

            Invalidate();
        }

        private TetrisPiece GenerateRandomPiece()
        {
            var pieces = new List<(int[,], Color)>
            {
                (new int[,] // Carré (2x2)
                {
                    { 1, 1 },
                    { 1, 1 }
                }, Color.GreenYellow),

                (new int[,] // Ligne (4x1)
                {
                    { 1, 1, 1, 1 }
                }, Color.Cyan),

                (new int[,] // Forme L
                {
                    { 1, 0 },
                    { 1, 0 },
                    { 1, 1 }
                }, Color.Orange),

                (new int[,] // Forme T
                {
                    { 0, 1, 0 },
                    { 1, 1, 1 }
                }, Color.Purple),

                (new int[,] // Forme Z
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 }
                }, Color.Red)
            };

            var randomIndex = random.Next(pieces.Count);
            var (shape, color) = pieces[randomIndex];
            return new TetrisPiece(shape, color);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Dessiner la grille occupée
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (tetrisGrid.IsCellOccupied(row, col))
                    {
                        var color = tetrisGrid.GetCellColor(row, col); // Méthode à implémenter
                        using (Brush brush = new SolidBrush(color))
                        {
                            g.FillRectangle(brush, col * 20, row * 20, 20, 20);
                        }
                    }
                }
            }

            // Dessiner la pièce courante
            for (int i = 0; i < currentPiece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < currentPiece.Shape.GetLength(1); j++)
                {
                    if (currentPiece.Shape[i, j] != 0)
                    {
                        int x = (currentPiece.Column + j) * 20;
                        int y = (currentPiece.Row + i) * 20;
                        using (Brush brush = new SolidBrush(currentPiece.Color))
                        {
                            g.FillRectangle(brush, x, y, 20, 20);
                        }
                    }
                }
            }

            // Afficher le score et le niveau
            tetrisGrid.DisplayScore(g);  // Affichage du score dans la fenêtre
        }

        private void InitializeComponent()
        {
            this.ClientSize = new Size(400, 700);
            this.Text = "Tetris";
            this.DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);

            // Initialisation du label pour afficher le score
            scoreLabel = new Label();
            scoreLabel.Location = new Point(-500, -500);
            scoreLabel.Size = new Size(180, 500);
            this.Controls.Add(scoreLabel);
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
