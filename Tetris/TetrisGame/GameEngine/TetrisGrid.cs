using System.Collections.Generic;

namespace TetrisGame.GameEngine
{
    public class TetrisGrid
    {
        private int[,] grid;
        private const int Rows = 20;
        private const int Columns = 10;

        public TetrisGrid()
        {
            grid = new int[Rows, Columns];
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
                    }
                }

                for (int c = 0; c < Columns; c++)
                {
                    grid[0, c] = 0;
                }
            }
        }

        public bool IsCellOccupied(int row, int col)
        {
            return grid[row, col] != 0;
        }
    }
}
