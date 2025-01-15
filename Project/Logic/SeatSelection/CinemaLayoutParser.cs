using Project.Helpers;
using static Project.Logic.SeatSelection.GridNavigator;

namespace Project.Logic.SeatSelection;

public static class CinemaLayoutParser
{
    public static List<List<Seat>> GetSeatingLayoutMap(int roomId)
    {
        var room = SeatSelectionHelpers.GetRoom(roomId);

        var y = 0;
        var x = 0;
        List<List<Seat>> layout = [];
        //x = blank, b = blue, o = orange, r = red
        foreach (var str in room.Layout)
        {
            List<Seat> rowSeats = [];
            foreach (var c in str)
            {
                var color = c switch
                {
                    'x' => ConsoleColor.Black,
                    'b' => ConsoleColor.DarkBlue,
                    'o' => ConsoleColor.DarkYellow,
                    'r' => ConsoleColor.Red,
                    _   => ConsoleColor.Black //signifies error
                };
                
                double priceMultiplier = c switch
                {
                    'x' => 0,
                    'b' => 1.0,
                    'o' => 1.25,
                    'r' => 1.5,
                    _   => 0 //signifies error
                };

                var isDecoy = 'x' == c;
                rowSeats.Add(new Seat(new Position(x, y), color, priceMultiplier, isDecoy: isDecoy));
                x++;
            }
            layout.Add(rowSeats);
            y++;
            x = 0;
        }

        return layout;
    }

    public static List<Position> GenerateNonDecoyPositions(List<List<Seat>> seats)
    {
        return seats
            .SelectMany(row => row
                .Where(seat => !seat.IsDecoy)
                .Select(seat => seat.Position))
            .ToList();
    }

    public static Position GetFirstNonDecoyPosition(List<List<Seat>> seats) => seats[0].First(seat => !seat.IsDecoy)!.Position;

    public static Seat GetSeatByPosition(Position position, int roomId)
    {
        Seat seat;

        seat = GetSeatingLayoutMap(roomId)[position.Y][position.X];

        return seat;
    }
}