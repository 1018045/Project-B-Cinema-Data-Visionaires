namespace Project.Logic.SeatSelection;

public class Seat(GridNavigator.Position position, ConsoleColor color, bool isReserved = false, bool isDecoy = false)
{
    public GridNavigator.Position Position { get; } = position;
    public ConsoleColor Color { get; } = color;
    public bool IsSelected { get; set; } = false;
    public bool IsReserved { get; set; } = isReserved;
    public bool IsDecoy { get; } = isDecoy;
    public bool IsTaken { get; set; }
}