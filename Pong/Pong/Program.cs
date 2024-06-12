namespace Pong
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            Field field = new Field(51, 15);
            Racket left = new Racket(5, 1);
            Racket right = new Racket(5, field.GetCols - 2);
            Ball ball = new Ball(field.GetRows / 2, field.GetCols / 2);

            left.Draw(field);
            right.Draw(field);
            ball.Draw(field);

            Console.SetWindowSize(field.GetCols + 1, field.GetRows + 5);
            Console.CursorVisible = false;

            int leftScore = 0;
            int rightScore = 0;

            DrawScore(field, leftScore, rightScore);

            int skipBall = 2;
            while (true)
            {
                for (int i = 0; i < field.GetRows; i++)
                {
                    for (int j = 0; j < field.GetCols; j++)
                    {
                        DrawAt(i, j, field.Get(i, j).ToString());
                    }
                }
                ReadInput(field, left, right);
                skipBall--;
                if (skipBall == 0)
                {
                    int scored = CheckForGoal(field, ball);
                    if (scored != -1)
                    {
                        if (scored == 0)
                        {
                            leftScore++;
                        }
                        else
                        {
                            rightScore++;
                        }

                        DrawScore(field, leftScore, rightScore);
                        ball.Reset(field);
                        left.Reset(field);
                        right.Reset(field);
                    }
                    else
                    {
                        ball.CalculateTrajectory(field, left.Tile, right.Tile);
                    }
                    skipBall = 2;
                }

                Thread.Sleep(10);
            }
        }

        static void DrawScore(Field field, int leftScore, int rightScore)
        {
            DrawAt(field.GetRows + 1, 1, $"Player Left Score: {leftScore}");
            DrawAt(field.GetRows + 2, 1, $"Player Right Score: {rightScore}");
        }

        static void DrawAt(int x, int y, string s)
        {
            Console.SetCursorPosition(y, x);
            Console.WriteLine(s);
        }

        static void ReadInput(Field field, Racket left, Racket right)
        {
            if (!Console.KeyAvailable)
            {
                return;
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    left.MoveUp(field);
                    break;
                case ConsoleKey.S:
                    left.MoveDown(field);
                    break;
                case ConsoleKey.UpArrow:
                    right.MoveUp(field);
                    break;
                case ConsoleKey.DownArrow:
                    right.MoveDown(field);
                    break;
            }
        }

        static int CheckForGoal(Field field, Ball ball)
        {
            if (ball.Y <= 0)
            {
                return 1;
            }
            if (ball.Y >= field.GetCols - 1)
            {
                return 0;
            }
            return -1;
        }
    }

    public class Field
    {
        private readonly char[,] _field;

        public int GetRows => this._field.GetLength(0);
        public int GetCols => this._field.GetLength(1);

        public char Tile { get; }

        public char Get(int row, int col)
        {
            if (row < 0 || row >= GetRows || col < 0 || col >= GetCols)
                throw new IndexOutOfRangeException("Attempting to access an out of bounds index in the field.");
            return this._field[row, col];
        }

        public void Set(int row, int col, char el)
        {
            if (row < 0 || row >= GetRows || col < 0 || col >= GetCols)
                throw new IndexOutOfRangeException("Attempting to set an out of bounds index in the field.");
            this._field[row, col] = el;
        }

        public Field(int width, int height, char tile = '#')
        {
            this._field = new char[height, width];
            this.Tile = tile;


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    this._field[i, j] = ' ';
                }
            }


            for (int i = 0; i < this.GetCols; i++)
            {
                this.Set(0, i, tile);
                this.Set(this.GetRows - 1, i, tile);
            }
        }
    }


    public class Racket
    {
        private readonly int _y;
        private readonly int _length;
        private readonly int _initialX;

        private int _x;

        public Racket(int x, int y, int length = 5, char tile = '|')
        {
            this._x = this._initialX = x;
            this._y = y;
            this._length = length;
            this.Tile = tile;
        }

        public char Tile { get; set; }

        public void Draw(Field field)
        {
            for (int i = 0; i < this._length; i++)
            {
                if (i + this._x >= 0 && i + this._x < field.GetRows)
                    field.Set(i + this._x, this._y, this.Tile);
            }
        }

        public void MoveUp(Field field)
        {
            if (this._x <= 1) return;

            field.Set(this._x + (this._length - 1), this._y, ' ');
            this._x--;
            this.Draw(field);
        }

        public void MoveDown(Field field)
        {
            if (this._x + this._length >= field.GetRows - 1) return;

            field.Set(this._x, this._y, ' ');
            this._x++;
            this.Draw(field);
        }

        public void Reset(Field field)
        {
            for (int i = 0; i < this._length; i++)
            {
                if (i + this._x >= 0 && i + this._x < field.GetRows)
                    field.Set(i + this._x, this._y, ' ');
            }

            this._x = this._initialX;
            this.Draw(field);
        }
    }

    public class Ball
    {
        private readonly char _tile;
        private readonly int _initialX;
        private readonly int _initialY;

        private int _x;
        private int _y;
        private bool _isGoingDown;
        private bool _isGoingRight;

        public Ball(int x, int y, char tile = 'o')
        {
            this._x = this._initialX = x;
            this._y = this._initialY = y;
            this._tile = tile;
            this._isGoingDown = true;
            this._isGoingRight = true;
        }

        public int Y => this._y;

        public void Draw(Field field)
        {
            if (_x >= 0 && _x < field.GetRows && _y >= 0 && _y < field.GetCols)
                field.Set(this._x, this._y, this._tile);
        }

        public void CalculateTrajectory(Field field, char leftRacketTile, char rightRacketTile)
        {

            
            if (_x >= 0 && _x < field.GetRows && _y >= 0 && _y < field.GetCols && field.Get(_x, _y) != '#')
                field.Set(this._x, this._y, ' ');

            if (_x - 1 < 0 || field.Get(_x - 1, _y) == '#')
            {
                this._isGoingDown = true;
            } 
            else if (_x + 1 >= field.GetRows || field.Get(_x + 1, _y) == '#')
            {
                this._isGoingDown = false;
            }     
            if (_y + 1 < field.GetCols && field.Get(_x, _y + 1) == rightRacketTile)
            {
                this._isGoingRight = false;
            }
            else if (_y - 1 >= 0 && field.Get(_x, _y - 1) == leftRacketTile)
            {
                this._isGoingRight = true;
            }

            this._x = this._isGoingDown ? this._x + 1 : this._x - 1;
            this._y = this._isGoingRight ? this._y + 1 : this._y - 1;
            Draw(field);
        }

        public void Reset(Field field)
        {     
            if (_x >= 0 && _x < field.GetRows && _y >= 0 && _y < field.GetCols && field.Get(_x, _y) != '#')
                field.Set(this._x, this._y, ' ');

            this._x = this._initialX;
            this._y = this._initialY;
            this.Draw(field);
        }
    }
}
        
    
