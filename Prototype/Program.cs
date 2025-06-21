namespace PROTOTYPE;
internal class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Please fullscreen your window.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        DungeonCrawlers();
    }

    static void BooleanAlgebraSimplifier()
    {
        string input = Console.ReadLine()!;

    }

    static void DungeonCrawlers()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.WriteLine("4. Tricky");
            Console.WriteLine("Press escape to go back");

            ConsoleKey difficulty = Console.ReadKey(true).Key;
            if (difficulty == ConsoleKey.Escape) break;

            Dungeon dungeon = null!;

            if (difficulty == ConsoleKey.D1)
                dungeon = new(20, 20);
            else if (difficulty == ConsoleKey.D2)
                dungeon = new(40, 20);
            else if (difficulty == ConsoleKey.D3)
            {
                dungeon = new(40, 30);
            }
            else if (difficulty == ConsoleKey.D4)
            {
                dungeon = new(60, 50);
            }

            dungeon?.Play("[]");
        }
    }
}

public class Dungeon
{
    private class Cell(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public bool IsWall { get; set; } = true;

        public bool IsLocked { get; set; } = false;

        public bool HasKey { get; set; } = false;

        public bool Visited { get; set; } = false;

        public override string ToString()
        {
            return $"{(IsWall ? "WALL" : "SPACE")} ({X},{Y})";
        }
    }

    public int Width { get; }
    public int Height { get; }

    private Cell StartPos { get; set; }
    private Cell EndPos { get; set; }
    private Cell KeyPos { get; set; }
    private Cell DoorPos { get; set; }

    private readonly Cell[,] Board;

    public bool Won { get; private set; } = false;

    public Dungeon(int width, int height)
    {
        Width = width;
        Height = height;
        Board = new Cell[width, height];
        PopulateBoard(Board);
        Generate();
    }

    private void PopulateBoard(Cell[,] board)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                board[x, y] = new Cell(x, y);
                if (x % 2 == 0 && y % 2 == 0)
                {
                    board[x, y].IsWall = false; // Alternating between wall and no wall to start off with
                }
            }
        }
    }

    private void DisplayBoard()
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.White;
        for (int i = 0; i <= Width; i++)
        {
            Console.Write("  ");
        }
        for (int y = 0; y < Height; y++)
        {
            Console.WriteLine();
            Console.Write("  ");
            for (int x = 0; x < Width; x++)
            {
                if (Board[x, y].IsWall)
                {
                    Console.Write("  ");
                }
                else if (Board[x, y].IsLocked)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("  ");
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else if (Board[x, y].HasKey)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("+-");
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                    Console.BackgroundColor = ConsoleColor.White;
                }
            }
        }
        Console.BackgroundColor = ConsoleColor.Black;
    }

    private void Generate(int x = 0, int y = 0) // Wrapper funcion :)
    {
        GenerateRecursively(x, y);

        ResetBoard();

        StartPos = StartPosGenerator();
        EndPos = EndPosGenerator();

        Board[EndPos.X, EndPos.Y].IsWall = false;

        DisplayBoard();

        // Locked (only works for 1 door and 1 key for now):
        HashSet<Cell> possibleDoorPositions = CellsToEnd(StartPos.X, StartPos.Y);
        int randomNum = Random.Shared.Next(0, possibleDoorPositions.Count);
        DoorPos = possibleDoorPositions.ElementAt(randomNum);
        Board[DoorPos.X, DoorPos.Y].IsLocked = true;

        ResetBoard();

        // New:
        HashSet<Cell> possibleKeyPositions = PossibleKeyPositions();
        randomNum = Random.Shared.Next(0, possibleKeyPositions.Count);
        KeyPos = possibleKeyPositions.ElementAt(randomNum);
        Board[KeyPos.X, KeyPos.Y].HasKey = true;
    }
    
    private void ResetBoard() // Resets all cells in Board to Visited == false
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Board[i, j].Visited = false;
            }
        }
    }

    private HashSet<Cell> CellsToEnd(int x, int y)
    {
        if ((x, y) == (EndPos.X - 1, EndPos.Y))
        {
            return [Board[x, y]];
        }

        Board[x, y].Visited = true;
        List<(int x, int y)> neighbours = [(x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1)];

        while (neighbours.Count > 0)
        {
            (int x, int y) neighbour = neighbours[Random.Shared.Next(0, neighbours.Count)];

            if (!(neighbour.x < Width && neighbour.y < Height && neighbour.x >= 0 && neighbour.y >= 0 &&
                !Board[neighbour.x, neighbour.y].Visited && !Board[neighbour.x, neighbour.y].IsWall))
            {
                neighbours.Remove(neighbour);
                continue;
            }

            neighbours.Remove(neighbour);
            var cells = CellsToEnd(neighbour.x, neighbour.y);
            if (cells.Count > 0) // If it's reached EndPos, it won't be empty as it won't have reached a dead end, meaning it is backtracking along the correct path
            {
                return [.. cells, Board[x, y]];
            }
        }
        return []; // Empty if it's reached a dead end
    }

    private HashSet<Cell> PossibleKeyPositions()
    {
        HashSet<Cell> result = [];

        Queue<Cell> queue = new([StartPos]);
        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            cell.Visited = true;
            if (cell != StartPos) result.Add(cell);
            var neighbours = new List<(int X, int Y)>()
            {
                (cell.X - 1, cell.Y),
                (cell.X, cell.Y - 1),
                (cell.X + 1, cell.Y),
                (cell.X, cell.Y + 1)
            };

            foreach (var neighbour in neighbours)
            {
                if (neighbour.X >= 0 && neighbour.Y >= 0 && neighbour.X < Width && neighbour.Y < Height &&
                    !Board[neighbour.X, neighbour.Y].IsLocked && !Board[neighbour.X, neighbour.Y].IsWall && !Board[neighbour.X, neighbour.Y].Visited)
                {
                    queue.Enqueue(Board[neighbour.X, neighbour.Y]);
                }
            }
        }
        return result;
    }

    private void GenerateRecursively(int x, int y)
    {
        Board[x, y].Visited = true;
        List<(int x, int y)> neighbours = [(x + 2, y), (x - 2, y), (x, y + 2), (x, y - 2)]; // Only care about neighbouring cells beyond the walls
        Random rnd = new();

        while (neighbours.Count > 0)
        {
            (int x, int y) neighbour = neighbours[rnd.Next(0, neighbours.Count)];

            if (!(neighbour.x < Width && neighbour.y < Height && neighbour.x >= 0 && neighbour.y >= 0 &&
                !Board[neighbour.x, neighbour.y].Visited))
            {
                neighbours.Remove(neighbour);
                continue;
            }

            int wallX = (x + neighbour.x) / 2;
            int wallY = (y + neighbour.y) / 2;
            Board[wallX, wallY].IsWall = false;

            neighbours.Remove(neighbour);
            GenerateRecursively(neighbour.x, neighbour.y);
        }
    }

    private Cell StartPosGenerator()
    {
        int x = 0;
        int y = Random.Shared.Next(1, Height - 1);
        while (Board[x, y].IsWall)
        {
            y = Random.Shared.Next(1, Height - 1);
        }
        return Board[x, y];
    }

    private Cell EndPosGenerator()
    {
        int x = Width - 1;
        int y = Random.Shared.Next(1, Height - 1);
        while (Board[x - 1, y].IsWall)
        {
            y = Random.Shared.Next(1, Height - 1);
        }
        return Board[x, y];
    }

    public void Play(string skin)
    {
        Console.CursorVisible = false;

        Player player = new(StartPos.X, StartPos.Y, skin, ConsoleColor.Gray);
        DisplayBoard();

        Place(player);

        ConsoleKey keyPressed = ConsoleKey.D0;
        while (keyPressed != ConsoleKey.D9)
        {
            if (player.X == EndPos.X && player.Y == EndPos.Y)
            {
                Won = true;
                break;
            }
            keyPressed = Console.ReadKey(true).Key;

            // New:
            int xNew = keyPressed == ConsoleKey.A ? player.X - 1 : keyPressed == ConsoleKey.D ? player.X + 1 : 
                keyPressed == ConsoleKey.LeftArrow ? FindNewPos('<', player) : keyPressed == ConsoleKey.RightArrow ? FindNewPos('>', player) : player.X;
            int yNew = keyPressed == ConsoleKey.W ? player.Y - 1 : keyPressed == ConsoleKey.S ? player.Y + 1 :
                keyPressed == ConsoleKey.UpArrow ? FindNewPos('^', player) : keyPressed == ConsoleKey.DownArrow ? FindNewPos('v', player) : player.Y;

            (int X, int Y) oldPos = (player.X, player.Y);
            (int X, int Y) newPos = (xNew, yNew);
            if (newPos.Y >= 0 && newPos.X >= 0 && newPos.Y < Height && newPos.X < Width &&
                !Board[newPos.X, newPos.Y].IsWall)
            {
                if (!Board[newPos.X, newPos.Y].IsLocked || player.HasKey)
                {
                    player.X = xNew;
                    player.Y = yNew;
                    Erase(oldPos);
                    Place(player);
                    if (Board[newPos.X, newPos.Y].HasKey)
                    {
                        player.HasKey = true;
                        Board[newPos.X, newPos.Y].HasKey = false;
                        KeyMessage();
                    }
                }
            }
        }
    }

    private void KeyMessage()
    {
        Console.SetCursorPosition(Width * 2 + 3, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("You have found the blue key! unlock the blue door :)");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    private int FindNewPos(char direction, Player player)
    {
        int xChange = 0, yChange = 0;

        int x = player.X, y = player.Y;

        switch (direction)
        {
            case '<':
                xChange = -1;
                yChange = 0;
                break;
            case '>':
                xChange = +1;
                yChange = 0;
                break;
            case '^':
                xChange = 0;
                yChange = -1;
                break;
            case 'v':
                xChange = 0;
                yChange = +1;
                break;
        }

        int xNew = x + xChange, yNew = y + yChange;
        while (xNew >= 0 && yNew >= 0  && xNew < Width && yNew < Height
            && !Board[xNew, yNew].IsWall && (!Board[xNew, yNew].IsLocked || player.HasKey))
        {
            x += xChange; y += yChange;
            
            if (Board[x, y].HasKey)
            {
                Erase((KeyPos.X, KeyPos.Y));
                player.HasKey = true;
                Board[x, y].HasKey = false;
                KeyMessage();
            }
            if (Board[x, y].IsLocked)
            {
                Erase((DoorPos.X, DoorPos.Y));
                Board[x, y].IsLocked = false;
            }
            xNew = x + xChange;
            yNew = y + yChange;
        }
        return xChange == 0 ? y : x;
    }

    private void Erase((int X, int Y) cell)
    {
        Console.SetCursorPosition(2 * cell.X + 2, cell.Y + 1);
        Console.Write("  ");
    }

    private void Place(Player player)
    {
        Console.SetCursorPosition(2 * player.X + 2, player.Y + 1);
        Console.Write(player.Skin);
    }
}

public class Player(int x, int y, string skin, ConsoleColor colour)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public string Skin { get; } = skin;

    public bool HasKey { get; set; } = false;

    public ConsoleColor Colour { get; } = colour;
}