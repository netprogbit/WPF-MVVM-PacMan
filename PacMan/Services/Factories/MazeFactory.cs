using Core.Enums;
using System;
using System.Collections.Generic;

namespace Services.Factories
{
    public class MazeFactory
    {
        /// <summary>
        /// Creating new maze
        /// </summary>
        /// <param name="width">Matrix width in places</param>
        /// <param name="height">Matrix height in places</param>
        /// <returns>Maze matrix</returns>
        public Occupation[,] Create(int width, int height)
        {
            if (width < 10 || height < 10)
                return null;

            width = Round(width);
            height = Round(height);

            int x;
            int y;

            bool finish = false;

            List<string> dir = new List<string>();

            Node[,] field = new Node[width, height];

            var random = new Random();

            int j = Round(random.Next(0, height - 1));

            field[1, j].Path = true; // fork
            field[1, j].Check = true; // passable place

            while (true)
            {
                finish = true;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (field[x, y].Path) // search fork flag
                        {
                            finish = false;

                            // If the path is clear, add direction
                            if (x - 2 >= 0)
                                if (!field[x - 2, y].Check) dir.Add("Left"); 

                            if (y - 2 >= 0)
                                if (!field[x, y - 2].Check) dir.Add("Down");

                            if (x + 2 < width)
                                if (!field[x + 2, y].Check) dir.Add("Right");

                            if (y + 2 < height)
                                if (!field[x, y + 2].Check) dir.Add("Up");

                            if (dir.Count > 0)
                            {
                                switch (dir[random.Next(0, dir.Count)]) // Choice a random direction
                                {
                                    case "Left":
                                        field[x - 1, y].Check = true;
                                        field[x - 2, y].Check = true;
                                        field[x - 2, y].Path = true;
                                        break;

                                    case "Down":
                                        field[x, y - 1].Check = true;
                                        field[x, y - 2].Check = true;
                                        field[x, y - 2].Path = true;
                                        break;

                                    case "Right":
                                        field[x + 1, y].Check = true;
                                        field[x + 2, y].Check = true;
                                        field[x + 2, y].Path = true;
                                        break;

                                    case "Up":
                                        field[x, y + 1].Check = true;
                                        field[x, y + 2].Check = true;
                                        field[x, y + 2].Path = true;
                                        break;
                                }
                            }
                            else
                            {
                                field[x, y].Path = false; // direction can not be added, remove the checkbox
                            }

                            dir.Clear(); // Clearing list
                        }
                    }
                }

                if (finish)
                    break; // There was not a single fork
            }

            Occupation[,] matrix = new Occupation[width, height];

            // Filling a matrix (1 is passable place, and -1 is wall)
            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    if (field[x, y].Check)
                    {
                        matrix[x, y] = Occupation.Wall;
                    }
                    else
                    {
                        matrix[x, y] = Occupation.Empty;
                    }
                }
            }

            MakePassagesWall(matrix);

            return matrix;
        }

        // Making passages in wall
        private static void MakePassagesWall(Occupation[,] matrix)
        {
            int matrixSize = matrix.Length;

            var random = new Random();

            while (true)
            {
                int x = random.Next(0, matrix.GetLength(0));
                int y = random.Next(0, matrix.GetLength(1));

                if (matrix[x, y] != Occupation.Wall)
                    continue;

                // Ability check of pass in wall
                if (!(
                    ((matrix[x, y - 1] == Occupation.Wall && matrix[x, y + 1] == Occupation.Wall) && (matrix[x - 1, y] != Occupation.Wall && matrix[x + 1, y] != Occupation.Wall)) ||
                    ((matrix[x - 1, y] == Occupation.Wall && matrix[x + 1, y] == Occupation.Wall) && (matrix[x, y - 1] != Occupation.Wall && matrix[x, y + 1] != Occupation.Wall))
                ))
                    continue;

                matrix[x, y] = Occupation.Empty;

                int amountWall = 0;

                for (var i = 0; i < matrix.GetLength(0); i++)
                {
                    for (var j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == Occupation.Wall)
                            amountWall++;
                    }
                }

                int fillPercent = amountWall * 100 / matrixSize;

                if (fillPercent <= 40)
                    break; // Walls occupy more than 40 percent
            }
        }

        // If the value is even, return odd
        private static int Round(int value)
        {            
            if (value % 2 == 0)
                return value + 1;

            return value;
        }
    }

    internal struct Node
    {
        public bool Path;
        public bool Check;
    }
}
