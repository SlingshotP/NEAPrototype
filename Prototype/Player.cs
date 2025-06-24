namespace PROTOTYPE;

public class Player(int x, int y, string skin, ConsoleColor colour)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public string Skin { get; } = skin;

    public bool HasKey { get; set; } = false;

    public ConsoleColor Colour { get; } = colour;
}