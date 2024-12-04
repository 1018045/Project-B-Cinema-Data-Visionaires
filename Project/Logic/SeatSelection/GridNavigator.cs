using System.Data;

namespace Project.Logic.SeatSelection;

//WARNING: the limits start from 0
public class GridNavigator(int xLimit, int yLimit)
{
    public Position Cursor { get; } = new(0, 0);
    public int X
    {
        get => Cursor.X;
        set => Cursor.X = value;
    }
    public int Y
    {
        get => Cursor.Y;
        set => Cursor.Y = value;
    }
    public Func<GridNavigator, Func<bool>>? SelectAction { get; set; }
    public Action<GridNavigator>? RefreshAction { get; set; }
    public Func<GridNavigator, bool>? ConfirmationAction { get; set; }

    public void Start(ConsoleKey confirmKey = ConsoleKey.Enter, ConsoleKey cancelKey = ConsoleKey.Escape)
    {
        if (SelectAction == null || RefreshAction == null)
            throw new DataException("DEVELOPER: SelectAction || RefreshAction is not set which should've been done before calling start");

        for (var done = false; !done;)
        {
            RefreshAction.Invoke(this);

            var keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    Y = Math.Max(0, Y - 1);
                    break;
                case ConsoleKey.DownArrow:
                    Y = Math.Min(yLimit, Y + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    X = Math.Max(0, X - 1);
                    break;
                case ConsoleKey.RightArrow:
                    X = Math.Min(xLimit, X + 1);
                    break;
                case var key when key == confirmKey:
                    if (ConfirmationAction?.Invoke(this) ?? true)
                        done = SelectAction
                            .Invoke(this)
                            .Invoke();
                    break;
                case var key when key == cancelKey:
                    done = true;
                    break;
                default:
                    continue;
            }
        }
    }

    public record Position(int X, int Y)
    {
        public int X { get; set; } = X;
        public int Y { get; set;  } = Y;
    }
}