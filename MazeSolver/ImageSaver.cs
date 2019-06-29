using ImageMagick;
using System.Collections.Generic;

namespace MazeSolver
{
    static class ImageSaver
    {
        static MagickColor[] colors = new MagickColor[] {
            MagickColors.White,     //0, background
            MagickColors.Black,     //1, walls
            MagickColors.Red,       //2, start
            MagickColors.Blue,       //3, stop
            MagickColors.Firebrick //End elment, used for path
        };
        static public bool SaveMazeImage(int[,] grid, List<Coordinate> path, int wallThickness, int pathThickness, string fileName)
        {
            int width = grid.GetLength(0); //dimension 0 of grid is width
            int height = grid.GetLength(1); //dimension 1 of grid is height
            using (MagickImage image = new MagickImage(new MagickColor("#ff00ff"), wallThickness * width, wallThickness * height))
            {
                Drawables draw = new Drawables(); //Hold the changes to be added to the image.
                draw.StrokeWidth(0);
                draw.StrokeColor(colors[0]);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++) { 
                        draw.FillColor(colors[grid[x,y]]).
                            Rectangle(x * wallThickness, y * wallThickness, (x + 1) * wallThickness, (y + 1) * wallThickness);
                    }
                }

                draw.StrokeColor(colors[colors.Length-1]).StrokeWidth(pathThickness);
                for (int i = 0; i < path.Count - 1; i++)
                {
                    //Use center points in grid for drawing path
                    Coordinate startPoint = new Coordinate (
                        path[i].x * wallThickness + wallThickness / 2,
                        path[i].y * wallThickness + wallThickness / 2
                    );
                    Coordinate endPoint = new Coordinate(
                        path[i+1].x * wallThickness + wallThickness / 2,
                        path[i+1].y * wallThickness + wallThickness / 2
                    );
                    draw.Line(startPoint.x, startPoint.y, endPoint.x, endPoint.y);
                }

                // Draw a border around the image
                draw.FillOpacity(new Percentage(0));
                draw.StrokeColor(colors[1]).StrokeWidth(4);
                draw.Rectangle(0, 0, wallThickness * width, wallThickness * height);
                //Commit draw calls to the image itself
                image.Draw(draw);
                //Save the Image
                image.Format = MagickFormat.Png;
                image.Write($"{fileName}.png");
            }
            return true;
        }
    }
}
