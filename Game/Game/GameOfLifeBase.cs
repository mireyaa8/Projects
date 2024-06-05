using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLide
{
    public class GameOfLifeBase
    {
        internal StringBuilder stringBuilder;
        public int x { get; set; }
        public int y { get; set; }
        public int[,] CurrentCellGeneretion { get; set; }
        public int[,] NextCellGeneration { get; set; }
        public GameOfLifeBase(int x, int y)
        {
            CurrentCellGeneretion = new int[x, y];
            NextCellGeneration = new int[x, y];


        }
        public string Draw(int boardSize, int windowWidth)
        {
            string[,] sceneBuffer = new string[boardSize, windowWidth];
            for(int row =0; row<sceneBuffer.GetLength(0); row++)
            {
                for(int col = 0; col<sceneBuffer.GetLength(1); col++)
                {
                    if (CurrentCellGeneretion[row, col] == 1)
                    {
                        sceneBuffer[row, col] = "□";

                    }
                    else
                    {
                        sceneBuffer[row, col] = " ";
                    }
                }
                DrawMenuPanel(windowWidth);
            }
            return stringBuilder.ToString().TrimEnd();
        }
        public virtual void DrawMenuPanel(int windowWidth)
        {
            stringBuilder.AppendLine(new string('=', windowWidth));
        }
      public void SpawnNextGeneration()
        {
            for(int row =0; row < NextCellGeneration.GetLength(0);row++) 
            {
                for(int col = 0;col<NextCellGeneration.GetLength(1); col++)
                {
                    int liveNeighbours = CalculateLiveNeighbours;
                    if (CurrentCellGeneretion[row,col]==1&& liveNeighbours < 2)
                    {
                        NextCellGeneration[row, col]=0;
                    }
                }
            }
        }

    }
   
  

}
