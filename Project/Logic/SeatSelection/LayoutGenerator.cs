using System.Text;
using static Project.Helpers.SeatSelectionHelpers;
using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Logic.SeatSelection;

public class LayoutGenerator
{
    private static readonly List<char> Alphabet = [.."ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()];
    public List<Position> TakenSeats { get; }
    public List<Position> SelectedSeats { get; }

    private const string SelectedSymbol = "()";

    private readonly RoomModel _room;
    private readonly GridNavigator _navigator;

    public LayoutGenerator(int roomId, int showingId, GridNavigator navigator, ref List<Position> selectedSeats)
    {
        _room = GetRoom(roomId);
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
        var textWidth = 3 + (seatDepth * 3) + (seatDepth - 9);

        var separator = new string('-', textWidth);
        var padding = new string(' ', (textWidth / 2) - 4);

        sb.AppendLine(separator);
        sb.Append('|').Append(padding).Append("SCREEN").Append(padding).AppendLine("|");
        sb.AppendLine(separator);

        for (var y = 1; y <= rows; y++)
        {
            sb.Append(Alphabet[y-1] + ": ");
            // if (y < 10) sb.Append(' ');

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