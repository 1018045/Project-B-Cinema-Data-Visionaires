using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Helpers;

public static class SeatSelectionHelpers
{
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

        // Transform the seats into a list of Position objects
        var takenSeats = relevantReservations
            .SelectMany(rm => rm.Seats.Split(",")) // Split seat strings by ","
            .Select(seat =>
            {
                // Remove parentheses and split into coordinates
                var coordinates = seat.Trim('(', ')').Split(';');
                return new Position(int.Parse(coordinates[1]) - 1, int.Parse(coordinates[0]) - 1);
            })
            .ToList();

        return takenSeats;
    }

    public static List<string> PositionsToStrings(List<Position> positions)
    {
        return positions.Select(p => $"{p.Y + 1};{p.X + 1}").ToList();
    }

    public static bool IsAdjacentOnSameRow(Position newSeat, List<Position> selectedSeats)
    {
        return selectedSeats.Any(selectedSeat =>
            selectedSeat.Y == newSeat.Y &&
            (selectedSeat.X == newSeat.X - 1 || selectedSeat.X == newSeat.X + 1));
    }

    public static bool CanFitAdjacentSeats(int seatCount, List<Position> takenSeats, List<int> roomDimensions)
    {
        for (var row = 0; row < roomDimensions.Count; row++)
        {
            var rowDepth = roomDimensions[row];

            var takenPositionsInRow = takenSeats
                .Where(seat => seat.Y == row)
                .Select(seat => seat.X)
                .ToHashSet();

            var currentStreak = 0;
            for (var x = 0; x < rowDepth; x++)
            {
                if (!takenPositionsInRow.Contains(x))
                {
                    currentStreak++;
                    if (currentStreak >= seatCount)
                    {
                        // Found enough adjacent seats
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
}