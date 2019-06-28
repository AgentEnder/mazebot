using System;
using System.Collections.Generic;
using System.Text;
using ImageMagick;
namespace MazeSolver
{
    static class ImageSaver
    {
        static public bool SaveMazeImage(int[,] g, List<coordinate> path, int wallThickness, int pathThickness, string fileName)
        {
            int w = g.GetLength(0);
            int h = g.GetLength(1);
            using (MagickImage image = new MagickImage(new MagickColor("#ff00ff"), wallThickness*w, wallThickness*h))
            {
                Drawables draw = new Drawables();
                draw.StrokeWidth(1);
                draw.StrokeColor(MagickColors.White);
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        
                        switch (g[x,y])
                        {
                            case 0:
                                {
                                    draw.FillColor(MagickColors.White).
                                        Rectangle(x * wallThickness, y * wallThickness, (x + 1) * wallThickness, (y + 1) * wallThickness); ;
                                }break;
                            case 1:
                                {
                                    draw.FillColor(MagickColors.Black).
                                        Rectangle(x * wallThickness, y * wallThickness, (x + 1) * wallThickness, (y + 1) * wallThickness); ;
                                }
                                break;
                            case 2:
                                {
                                    draw.FillColor(MagickColors.Red).
                                        Rectangle(x * wallThickness, y * wallThickness, (x + 1) * wallThickness, (y + 1) * wallThickness); ;
                                }
                                break;
                            case 3:
                                {
                                    draw.FillColor(MagickColors.Blue).
                                        Rectangle(x * wallThickness, y * wallThickness, (x + 1) * wallThickness, (y + 1) * wallThickness); ;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                draw.StrokeColor(MagickColors.MediumVioletRed).StrokeWidth(pathThickness);
                for (int i = 0; i < path.Count-1; i++)
                {
                    draw.Line(path[i].x*wallThickness+wallThickness/2, path[i].y * wallThickness + wallThickness / 2, path[i + 1].x * wallThickness + wallThickness / 2, path[i + 1].y * wallThickness + wallThickness / 2);
                }

                
                draw.FillOpacity(new Percentage(0));
                draw.StrokeColor(MagickColors.Black).StrokeWidth(4);
                draw.Rectangle(0, 0, wallThickness * w, wallThickness * h);
                image.Draw(draw);
                image.Format = MagickFormat.Png;
                image.Write($"{fileName}.png");
            }
            return true;
        }
    }
}
