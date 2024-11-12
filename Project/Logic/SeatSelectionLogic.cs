using System.Diagnostics.Contracts;
using System.Text;

namespace Project.Logic;

public class SeatSelectionLogic
{
    public static string GenerateSeatingLayout(int roomId, int showingId)
    {
        var data = RoomAccess.LoadAll();
        var room = data.Find(rm => rm.Id == roomId);

        //Exception that should only be called during development when the data isn't correctly linked
        if (room == null)
            throw new ArgumentException("Logic error: The roomId was not found");

        var rows = room.Rows;
        var seatDepth = room.SeatDepth;

        var takenSeats = GetTakenSeats(showingId);

        var sb = new StringBuilder();
        for (int i = 1; i <= rows; i++)
        {
            sb.Append(i + ": ");
            if (i < 10) sb.Append(' ');

            for (int j = 1; j <= seatDepth; j++)
            {
                sb.Append(!takenSeats.Contains(j) ? $"[{j}]" : "[x]");
            }

            if (i != rows)
                sb.Append('\n');
        }

        return sb.ToString();
    }

    private static List<int> GetTakenSeats(int showingId)
    {
        var relevantReservations = ReservationsAccess.LoadAll().FindAll(rm => rm.ShowingId == showingId);
        return relevantReservations.Select(rm => rm.Id).ToList();
    }
}