using System.Data;

namespace Project.Logic.SeatSelection;

public class GridNavigator
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
    //True if the move is permissible
    public Func<Position, bool>? MovePredicate { get; set; }
    public Func<GridNavigator, Func<bool>>? SelectAction { get; set; }
    public Action<GridNavigator>? RefreshAction { get; set; }
    public Func<GridNavigator, bool>? ConfirmationAction { get; set; }

    public void Start(ConsoleKey confirmKey = ConsoleKey.Enter, ConsoleKey cancelKey = ConsoleKey.Escape)
    {
        if (SelectAction == null || RefreshAction == null || MovePredicate == null)
            throw new DataException("DEVELOPER: SelectAction || RefreshAction || MovePredicate is not set which should've been done before calling start");

        for (var done = false; !done;)
        {
            RefreshAction.Invoke(this);

            var keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    Y -= MovePredicate.Invoke(new Position(X, Y - 1)) ? 1 : 0;
                    //Y = Math.Max(0, Y - 1);
                    break;
                case ConsoleKey.DownArrow:
                    Y += MovePredicate.Invoke(new Position(X, Y + 1)) ? 1 : 0;
                    //Y = Math.Min(yLimit, Y + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    X -= MovePredicate.Invoke(new Position(X - 1, Y)) ? 1 : 0;
                    //X = Math.Max(0, X - 1);
                    break;
                case ConsoleKey.RightArrow:
                    X += MovePredicate.Invoke(new Position(X + 1, Y)) ? 1 : 0;
                    //X = Math.Min(xLimit, X + 1);
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