using System.Runtime.InteropServices;
using Project.Logic.SeatSelection;
using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Helpers;

public static class SeatSelectionHelpers
{
    private static readonly List<char> Alphabet = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
    public static RoomModel GetRoom(int roomId)
    {
        var data = RoomAccess.LoadAll();
        var room = data.Find(rm => rm.Id == roomId);

        //Exception that should only be called during development when the data isn't correctly linked
        if (room == null)
            throw new ArgumentException("Logic error: The roomId was not found");

        return room;
    }

    public static RoomModel GetRoomByShowing(int showingId)
    {
        var showing = ShowingsAccess.LoadAll().Find(s => s.Id == showingId);

        if (showing == null)
            throw new ArgumentException($"Logic error: Showing with id {showingId} not found");

        return GetRoom(showing.Room);
    }

    public static List<Position> GetTakenSeats(int showingId)
    {
        // Load all reservations and filter for the relevant ones
        var relevantReservations = ReservationsAccess.LoadAll()
            .FindAll(rm => rm.ShowingId == showingId);

        // foreach (ReservationModel res in relevantReservations)
        // {
        //     System.Console.WriteLine(res.Seats);
        // }
        // Thread.Sleep(10000);
        // Transform the seats into a list of Position objects
        // var takenSeats = relevantReservations
        //     .SelectMany(rm => rm.Seats.Split(",")) // Split seat strings by ","
        //     .Select(seat =>
        //     {
        //         // Remove parentheses and split into coordinates
        //         var coordinates = seat.Trim('(', ')').Split(';');
        //         System.Console.WriteLine(coordinates[0]);
        //         System.Console.WriteLine(coordinates[1]);
        //         return new Position(int.Parse(coordinates[1]) - 1, int.Parse(coordinates[0]) - 1);
        //     })
        //     .ToList();
        var takenSeats = StringToPositions(relevantReservations.Select(r => r.Seats).ToList());

        return takenSeats;
    }

    public static string PositionsToStrings(List<Position> positions)
    {
        return string.Join(", ", positions.Select(p => $"{p.Y + 1};{p.X + 1}").ToList());
    }

    public static List<Position> StringToPositions(List<string> seats)
    {
        List<Position> positions = new();
        foreach (string reservation in seats)
        {
            foreach (string seat in reservation.Split(','))
            {
                if (seat.Split(';').Count() < 2)
                {
                    System.Console.WriteLine("ERROR");
                    Thread.Sleep(2500);
                    continue;
                }
                positions.Add(new Position(Convert.ToInt32(seat.Split(';')[1])-1, Convert.ToInt32(seat.Split(';')[0])-1));
            }
        }
        return positions;
    }

    public static string PositionsToRowSeatString(List<Position> positions)
    {
        return string.Join(", ", positions.Select(p => $"row: {Alphabet[p.Y]}, seat: {p.X + 1}"));
    }

    public static string SeatsToRowSeatString(List<Seat> seats)
    {
        return string.Join(", ", seats.Select(s => s.Position).Select(p => $"row: {Alphabet[p.Y]}, seat: {p.X + 1}"));
    }

    public static bool IsAdjacentOnSameRow(Position newSeat, List<Position> selectedSeats)
    {
        return selectedSeats.Any(selectedSeat =>
            selectedSeat.Y == newSeat.Y &&
            (selectedSeat.X == newSeat.X - 1 || selectedSeat.X == newSeat.X + 1));
    }

    public static bool CanFitAdjacentSeats(int seatCount, List<Position> takenSeats, Dictionary<int, List<int>> rowSeatMap)
    {
        foreach (var rowEntry in rowSeatMap)
        {
            var rowIndex = rowEntry.Key;
            var rowSeats = rowEntry.Value;

            var takenPositionsInRow = takenSeats
                .Where(seat => seat.Y == rowIndex)
                .Select(seat => seat.X)
                .ToHashSet();

            var currentStreak = 0;

            foreach (var seat in rowSeats)
            {
                if (!takenPositionsInRow.Contains(seat))
                {
                    currentStreak++;
                    if (currentStreak == seatCount)
                    {
                        return true;
                    }
                }
                else
                {
                    currentStreak = 0;
                }
            }
        }

        return false;
    }

    public static ConsoleColor StringToConsoleColor(string colorName)
    {
        if (Enum.TryParse(colorName, true, out ConsoleColor color))
        {
            return color;
        }
        else
        {
            throw new ArgumentException($"'{colorName}' is not a valid ConsoleColor.");
        }
    }
}