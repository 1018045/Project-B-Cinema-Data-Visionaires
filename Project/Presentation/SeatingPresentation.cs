﻿using Project.Logic;

namespace Project.Presentation;

public class SeatingPresentation
{
    public static List<string> Present(int showingId)
    {
        int seatCount = -1;
        for (bool resolved = false; resolved == false;)
        {
            Console.WriteLine("How many seats would you like to book?");
            var seatAmountInput = Console.ReadLine() ?? "";

            resolved = int.TryParse(seatAmountInput, out seatCount);
        }

        if (seatCount == -1)
            return [];

        return SeatSelectionLogic.StartSeatSelection(showingId, seatCount);
    }

    //used to constantly refresh the seating
    public static void UpdateSeatingPresentation(string presentation, int seatsLeft)
    {
        Console.Clear();

        Console.WriteLine("Select your seat using the arrow keys (< ^ > v) and use the enter key to lock in your seat(s) (your choice is final)");
        Console.WriteLine("The seats you select must be adjacent on the same row without gaps");
        Console.WriteLine($"You need to select {seatsLeft} more seat(s)");
        Console.WriteLine();

        Console.WriteLine(presentation);
    }

    public static void SuccessfulSelection(string seatDisplay)
    {
        Console.Clear();

        Console.WriteLine($"You've successfully selected the following seats: {seatDisplay}");

        Thread.Sleep(5000);

        Console.Clear();
    }
}