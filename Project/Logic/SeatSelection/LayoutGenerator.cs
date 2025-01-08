using System.Diagnostics;
using System.Text;
using Project.Presentation;
using static Project.Helpers.SeatSelectionHelpers;
using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Logic.SeatSelection;

public class LayoutGenerator
{
    private static readonly List<char> Alphabet = [.."ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()];
    public List<Position> TakenSeats { get; }
    public List<Position> SelectedSeats { get; }

    private const string SeatChar = " \u2588\u2588 ";
    private const ConsoleColor TakenColor = ConsoleColor.Magenta;
    private const ConsoleColor SelectedColor = ConsoleColor.DarkGreen;

    private readonly RoomModel _room;
    private readonly GridNavigator _navigator;

    public LayoutGenerator(int roomId, int showingId, GridNavigator navigator, ref List<Position> selectedSeats)
    {
        _room = GetRoom(roomId);
        _navigator = navigator;
        TakenSeats = GetTakenSeats(showingId);
        SelectedSeats = selectedSeats;
    }

    /*private void BuildSeatingString(out StringBuilder sb)
    {
        sb = new StringBuilder();

        var rows = _room.Rows;
        var seatDepth = _room.SeatDepth;
        var textWidth = 3 + (seatDepth * 3) + (seatDepth - 9);

        for (var y = 1; y <= rows; y++)
        {
            sb.Append(Alphabet[y-1] + ": ");

            for (var x = 1; x <= seatDepth; x++)
            {
                var currentPosition = new Position(x - 1, y - 1);
                var characters = !TakenSeats.Contains(currentPosition) ? $"[{x}]" : "[x]";

                if (SelectedSeats.Contains(currentPosition))
                    characters = $""; //already selected seat
                if (_navigator.Cursor.Equals(currentPosition))
                    characters = $""; //cursor position

                sb.Append(characters);
            }

            if (y != rows)
                sb.Append('\n');
        }

        var separator = new string('-', textWidth);
        var padding = new string(' ', (textWidth / 2) - 4);

        sb.AppendLine(separator);
        sb.Append('|').Append(padding).Append("SCREEN").Append(padding).AppendLine("|");
        sb.AppendLine(separator);
    }*/

    /*public void BuildSeatingLayoutV2()
    {
        var high = _room.SeatCategories.High;
        var medium = _room.SeatCategories.Medium;
        var low = _room.SeatCategories.Low;

        Dictionary<Position, ConsoleColor> map = [];
        AddSeats(high, ref map);
        AddSeats(medium, ref map);
        AddSeats(low, ref map);

        for (var row = 0; row < _room.Height + 1; row++)
        {
            StringBuilder sb = new();
            List<ConsoleColor> colors = [];

            sb.Append($"{Alphabet[row]}: ");
            colors.AddRange([ConsoleColor.White, ConsoleColor.White, ConsoleColor.White]);

            for (var seat = 0; seat < _room.Width + 1; seat++)
            {
                var pos = new Position(seat, row);

                sb.Append($" {seat}:");
                colors.AddRange(Enumerable.Repeat(ConsoleColor.White, 2 + seat.ToString().Length));

                ConsoleColor? color = null;
                if (_navigator.Cursor.Equals(pos))
                    color = ConsoleColor.Cyan;


                if (TakenSeats.Contains(pos))
                {
                    sb.Append(SeatChar);
                    colors.AddRange([color ?? TakenColor, color ?? TakenColor, color ?? TakenColor, color ?? TakenColor]);
                    continue;
                }

                if (SelectedSeats.Contains(pos))
                {
                    sb.Append(SeatChar);
                    colors.AddRange([SelectedColor, SelectedColor, SelectedColor, SelectedColor]);
                    continue;
                }

                if (!map.ContainsKey(pos))
                {
                    sb.Append('x');
                    colors.Add(color ?? ConsoleColor.Black);
                }
                else
                {
                    sb.Append(SeatChar);
                    colors.AddRange([color ?? map[pos], color ?? map[pos], color ?? map[pos], color ?? map[pos]]);
                }
            }
            colors.Add(ConsoleColor.Black); //add redundant for newline entry
            SeatingPresentation.PrintInColor(sb + "\n", colors);
        }
    }*/

    //todo add the layout to the param so that the isreserver and selected can be changed properly and pass as a ref
    public void BuildSeatingLayoutV3(List<List<Seat>> layout)
    {
        foreach (var row in layout)
        {
            List<ConsoleColor> colors = [];
            var sb = new StringBuilder();

            //add row num
            /*sb.Append("");
            colors.AddRange([ConsoleColor.White, ConsoleColor.White]);*/

            foreach (var seat in row)
            {
                if (seat.Position.Equals(_navigator.Cursor))
                    seat.IsSelected = true;

                var color = seat switch
                {
                    { IsSelected: true } => ConsoleColor.Cyan,
                    { IsDecoy: true } => ConsoleColor.Black,
                    { IsTaken: true} => ConsoleColor.DarkGray,
                    { IsReserved: true } => ConsoleColor.Magenta,
                    _ => seat.Color
                };

                sb.Append(" \u25a0 "); //3
                colors.AddRange([ConsoleColor.White, color, ConsoleColor.White]); //3

                seat.IsSelected = false;
            }
            sb.Append('\n'); //1
            colors.Add(ConsoleColor.White); //1

            SeatingPresentation.PrintInColor(sb.ToString(), colors);
        }
    }
}