using System.Text;
using static Project.Helpers.SeatSelectionHelpers;
using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Logic.SeatSelection;

public class LayoutGenerator
{
    public List<Position> TakenSeats { get; } = [];
    public List<Position> SelectedSeats { get; }

    private const string SelectedSymbol = "()";

    private readonly RoomModel _room;
    private readonly GridNavigator _navigator;

    public LayoutGenerator(int roomId, int showingId, GridNavigator navigator, ref List<Position> selectedSeats)
    {
        _room = GetRoomByShowing(roomId);
        _navigator = navigator;
        TakenSeats = GetTakenSeats(showingId);
        SelectedSeats = selectedSeats;
    }

    public string GenerateSeatingLayout()
    {
        BuildSeatingString(out var sb);
        return sb.ToString();
    }

    private void BuildSeatingString(out StringBuilder sb)
    {
        sb = new StringBuilder();

        var rows = _room.Rows;
        var seatDepth = _room.SeatDepth;

        for (var y = 1; y <= rows; y++)
        {
            sb.Append(y + ": ");
            if (y < 10) sb.Append(' ');

            for (var x = 1; x <= seatDepth; x++)
            {
                var currentPosition = new Position(x - 1, y - 1);
                var characters = !TakenSeats.Contains(currentPosition) ? $"[{x}]" : "[x]";

                if (SelectedSeats.Contains(currentPosition))
                    characters = $"{SelectedSymbol}"; //already selected seat
                if (_navigator.Cursor.Equals(currentPosition))
                    characters = $" {SelectedSymbol} "; //cursor position

                sb.Append(characters);
            }

            if (y != rows)
                sb.Append('\n');
        }
    }
}