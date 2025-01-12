using System;
using System.Collections.Generic;
using System.Drawing;

namespace TetrisGame.GameEngine
{
    public class TetrisGrid
    {
        private int[,] grid;
        private const int Rows = 20;
        private const int Columns = 10;
        private int score;  
        private int level;  
        private int linesCleared;  
        private Color[,] colorsGrid;  // Nouvelle matrice pour stocker les couleurs des cellules

        public TetrisGrid()
        {
            grid = new int[Rows, Columns];
            colorsGrid = new Color[Rows, Columns];  // Initialisation de la grille de couleurs
            score = 0;
            level = 1;
            linesCleared = 0;
        }

        public bool CanPlacePiece(TetrisPiece piece)
        {
            for (int i = 0; i < piece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < piece.Shape.GetLength(1); j++)
                {
                    if (piece.Shape[i, j] != 0)
                    {
                        int row = piece.Row + i;
                        int col = piece.Column + j;

                        if (row < 0 || row >= Rows || col < 0 || col >= Columns || grid[row, col] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void PlacePiece(TetrisPiece piece)
        {
            for (int i = 0; i < piece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < piece.Shape.GetLength(1); j++)
                {
                    if (piece.Shape[i, j] != 0)
                    {
                        int row = piece.Row + i;
                        int col = piece.Column + j;
                        grid[row, col] = 1;
                        colorsGrid[row, col] = piece.Color;  // Assigner la couleur de la pièce dans la grille
                    }
                }
            }
        }

        public List<int> CheckFullRows()
        {
            var fullRows = new List<int>();

            for (int row = 0; row < Rows; row++)
            {
                bool isFull = true;
                for (int col = 0; col < Columns; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    fullRows.Add(row);
                }
            }

            return fullRows;
        }

        public void ClearRows(List<int> fullRows)
        {
            foreach (var row in fullRows)
            {
                for (int r = row; r > 0; r--)
                {
                    for (int c = 0; c < Columns; c++)
                    {
                        grid[r, c] = grid[r - 1, c];
                        colorsGrid[r, c] = colorsGrid[r - 1, c];  // Décaler aussi les couleurs
                    }
                }

                for (int c = 0; c < Columns; c++)
                {
                    grid[0, c] = 0;
                    colorsGrid[0, c] = Color.Empty;  // Réinitialiser la couleur de la première ligne
                }
            }

            // Mise à jour du score et du niveau
            UpdateScore(fullRows.Count);
        }

        public bool IsCellOccupied(int row, int col)
        {
            return grid[row, col] != 0;
        }

        public Color GetCellColor(int row, int col)
        {
            return colorsGrid[row, col];  // Retourner la couleur de la cellule correspondante
        }

        public void DisplayScore(Graphics g)
        {
            string scoreText = $"Score: {score}  Lines Cleared: {linesCleared}";
            using (Font font = new Font("Arial", 10))
            {
                g.DrawString(scoreText, font, Brushes.Black, new Point(10, 500));
            }
        }

        private void UpdateScore(int fullRowCount)
        {
            score += fullRowCount * (100 + (level - 1) * 50);  // Bonus de score pour les niveaux supérieurs
            linesCleared += fullRowCount;

            if (linesCleared >= level * 10)  // Augmenter le niveau tous les 10 lignes effacées
            {
                level++;
            }
        }
    }
}
