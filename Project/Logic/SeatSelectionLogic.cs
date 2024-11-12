using System.Text;
using Project.Presentation;

namespace Project.Logic;

public static class SeatSelectionLogic
{
    private const string SelectedSymbol = "()";

    public static List<string> StartSeatSelection(int showingId, int seatCount)
    {
        var room = GetRoomByShowing(showingId);
        var maxYPos = room.Rows - 1;
        var maxXPos = room.SeatDepth - 1;
        var takenSeats = GetTakenSeats(showingId);

        var curPos = (0, 0); //x,y
        List<string> selectedSeats = [];
        List<(int, int)> selectedSeatsPos = [];
        for (;;)
        {
            SeatingPresentation.UpdateSeatingPresentation(GenerateSeatingLayout(room.Id, showingId, curPos, selectedSeatsPos));

            var keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    //Move up if not already at the top boundary
                    curPos.Item2 = Math.Max(0, curPos.Item2 - 1);
                    break;
                case ConsoleKey.DownArrow:
                    //Move down if not already at the bottom boundary
                    curPos.Item2 = Math.Min(maxYPos, curPos.Item2 + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    //Move left if not already at the left boundary
                    curPos.Item1 = Math.Max(0, curPos.Item1 - 1);
                    break;
                case ConsoleKey.RightArrow:
                    //Move right if not already at the right boundary
                    curPos.Item1 = Math.Min(maxXPos, curPos.Item1 + 1);
                    break;
                case ConsoleKey.Enter:
                    var seat = $"({curPos.Item2 + 1};{curPos.Item1 + 1})"; //(row;seat_number)
                    if (selectedSeats.Contains(seat) || takenSeats.Contains(curPos))
                        continue;

                    selectedSeats.Add(seat);
                    selectedSeatsPos.Add(curPos);
                    curPos = (0, 0);

                    if (selectedSeats.Count < seatCount)
                        break;

                    Console.Clear();
                    return selectedSeats;
                default:
                    continue;
            }
        }
    }

    public static string GenerateSeatingLayout(int roomId, int showingId, (int, int) cursorPos, List<(int, int)> selectedSeats)
    {
        var room = GetRoom(roomId);

        var rows = room.Rows;
        var seatDepth = room.SeatDepth;
        var takenSeats = GetTakenSeats(showingId);
        var sb = new StringBuilder();

        BuildSeatingString(sb, rows, seatDepth, takenSeats, selectedSeats, cursorPos);

        return sb.ToString();
    }

    public static List<(int, int)> StringSeatsToPositions(string input)
    {
        var result = new List<(int, int)>();
        var seats = input.Split(',');

        foreach (var seat in seats)
        {
            var parts = seat.Trim('(', ')').Split(';');

            int first = int.Parse(parts[0]);
            int second = int.Parse(parts[1]);

            result.Add((first, second));
        }

        return result;
    }

    private static void BuildSeatingString(StringBuilder sb, int rows, int seatDepth, List<(int, int)> takenSeats, List<(int, int)> selectedSeats, (int, int) cursorPos)
    {
        for (var y = 1; y <= rows; y++)
        {
            sb.Append(y + ": ");
            if (y < 10) sb.Append(' ');

            for (var x = 1; x <= seatDepth; x++)
            {
                var characters = !takenSeats.Contains((y,x)) ? $"[{x}]" : "[x]";
                var currentGenPos = (x - 1, y - 1);

                if (selectedSeats.Contains(currentGenPos))
                    characters = $"{SelectedSymbol}"; //already selected seat
                if (cursorPos.Equals(currentGenPos))
                    characters = $" {SelectedSymbol} "; //cursor position

                sb.Append(characters);
            }

            if (y != rows)
                sb.Append('\n');
        }
    }

    private static RoomModel GetRoom(int roomId)
    {
        var data = RoomAccess.LoadAll();
        var room = data.Find(rm => rm.Id == roomId);

        //Exception that should only be called during development when the data isn't correctly linked
        if (room == null)
            throw new ArgumentException("Logic error: The roomId was not found");

        return room;
    }

    private static RoomModel GetRoomByShowing(int showingId)
    {
        var showing = ShowingsAccess.LoadAll().Find(s => s.Id == showingId);

        if (showing == null)
            throw new ArgumentException($"Logic error: Showing with id {showingId} not found");

        return GetRoom(showing.Room);
    }

    private static List<(int, int)> GetTakenSeats(int showingId)
    {
        var relevantReservations = ReservationsAccess.LoadAll().FindAll(rm => rm.ShowingId == showingId);
        List<(int, int)> taken = [];
        relevantReservations
            .ForEach(r => StringSeatsToPositions(r.Seats)
                .ForEach(p => taken.Add(p))
        );
        return taken;
    }
}