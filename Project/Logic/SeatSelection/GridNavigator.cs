using System.Data;

namespace Project.Logic.SeatSelection;

//WARNING: the limits start from 0
public class GridNavigator(int xLimit, int yLimit)
{
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public Action<GridNavigator>? Action { get; init; }
    public Action<GridNavigator>? Confirmation { get; init; }

    public void Start()
    {
        if (Action == null)
            throw new DataException("DEVELOPER: Action is not set which should've been done before calling start");
        for (;;)
        {
            var keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    //Move up if not already at the top boundary
                    Y = Math.Max(0, Y - 1);
                    break;
                case ConsoleKey.DownArrow:
                    //Move down if not already at the bottom boundary
                    Y = Math.Min(yLimit, Y + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    //Move left if not already at the left boundary
                    X = Math.Max(0, X - 1);
                    break;
                case ConsoleKey.RightArrow:
                    //Move right if not already at the right boundary
                    X = Math.Min(xLimit, X + 1);
                    break;
                default:
                    continue;
            }
        }
    }
}