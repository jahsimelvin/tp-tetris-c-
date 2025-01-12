using System.Drawing;


namespace TetrisGame.GameEngine
{
    public class TetrisPiece
    {
        public int[,] Shape { get; private set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Color Color { get; private set; } // Ajout de la propriété pour la couleur

        public TetrisPiece(int[,] shape, Color color)
        {
            Shape = shape;
            Color = color; // Stocke la couleur de la pièce
            Row = 0;
            Column = 4; // Centre horizontalement
        }

        public void Rotate()
        {
            int rows = Shape.GetLength(0);
            int cols = Shape.GetLength(1);
            int[,] rotated = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[j, rows - i - 1] = Shape[i, j];
                }
            }

            Shape = rotated;
        }

        public bool MovePiece(TetrisGrid grid, int deltaRow, int deltaCol)
        {
            Row += deltaRow;
            Column += deltaCol;

            if (!grid.CanPlacePiece(this))
            {
                Row -= deltaRow;
                Column -= deltaCol;
                return false;
            }
            return true;
        }

        public bool TryRotate(TetrisGrid grid)
        {
            int[,] originalShape = Shape;
            Rotate();

            if (!grid.CanPlacePiece(this))
            {
                // Annuler la rotation si invalide
                Shape = originalShape;
                return false;
            }

            return true;
        }
    }
}
