using System;
using System.Collections;

namespace Battleships
{
    // Imagine a game of battleships.
    //   The player has to guess the location of the opponent's 'ships' on a 10x10 grid
    //   Ships are one unit wide and 2-4 units long, they may be placed vertically or horizontally
    //   The player asks if a given co-ordinate is a hit or a miss
    //   Once all cells representing a ship are hit - that ship is sunk.
    public class Game
    {
        // ships: each string represents a ship in the form first co-ordinate, last co-ordinate
        //   e.g. "3:2,3:5" is a 4 cell ship horizontally across the 4th row from the 3rd to the 6th column
        // guesses: each string represents the co-ordinate of a guess
        //   e.g. "7:0" - misses the ship above, "3:3" hits it.
        // returns: the number of ships sunk by the set of guesses
        public static int Play(string[] ships, string[] guesses)
        {
            int sunkShipCount = 0; int shipNo = 1;
            string[,] battleshipBoard = new string[10, 10]; //2D array to place ships on 10x10 grid.
            for (int i = 0; i < battleshipBoard.GetLength(0); i++)
            {
                for (int j = 0; j < battleshipBoard.GetLength(1); j++)
                {
                    battleshipBoard[i, j] = ".";
                }
            }
            Hashtable shipSinkStatus = new Hashtable();
            foreach (var ship in ships)
            {
                var shipCoordinates = ship.Split(',');
                var startColumn = Convert.ToInt32(shipCoordinates[0].Split(':')[1]);
                var startRow = Convert.ToInt32(shipCoordinates[0].Split(':')[0]);
                int endRow = Convert.ToInt32(shipCoordinates[1].Split(':')[0]);
                int endColumn = Convert.ToInt32(shipCoordinates[1].Split(':')[1]);
                if (startRow < 0 || startColumn < 0 || endRow < 0 || endColumn < 0)
                {
                    Console.WriteLine("Invalid Coordinates. Ship coordinates cannot be less than 0.");
                    return 0;
                }
                if (startRow > 9 || startColumn > 9 || endRow > 9 || endColumn > 9)
                {
                    Console.WriteLine("Invalid Coordinates. Ship Coordinates cannot be more than 9 on 10x10 Grid.");
                    return 0;
                }
                if (startRow != endRow && startColumn != endColumn)
                {
                    Console.WriteLine("Invalid Width of Ship");
                    return 0;
                }
                var orientation = startRow == endRow ? "horizontal" : startColumn == endColumn ? "vertical" : "horizontal";
                if (orientation.Equals("horizontal"))
                {
                    if (endColumn - startColumn < 2 || endColumn - startColumn > 4)
                    {
                        Console.WriteLine("Invalid Length of Ship");
                        return 0;
                    }
                }
                else
                {
                    if (endRow - startRow < 2 || endRow - startRow > 4)
                    {
                        Console.WriteLine("Invalid Length of Ship");
                        return 0;
                    }
                }
                if (orientation.Equals("horizontal"))
                {
                    for (int i = 0; i <= endColumn - startColumn; i++)
                    {
                        if (battleshipBoard[startRow, startColumn + i] == ".")
                        {
                            battleshipBoard[startRow, startColumn + i] = "B" + shipNo;
                        }
                        else
                        {
                            Console.WriteLine("Intersecting Ships are not allowed");
                            return 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= endRow - startRow; i++)
                    {
                        if (battleshipBoard[startRow + i, startColumn] == ".")
                        {
                            battleshipBoard[startRow + i, startColumn] = "B" + shipNo;
                        }
                        else
                        {
                            Console.WriteLine("Intersecting Ships are not allowed");
                            return 0;
                        }
                    }
                }
                shipSinkStatus.Add("B" + shipNo, 1);
                shipNo++;
            }
            foreach (var guess in guesses)
            {
                var coordinates = guess.Split(':');
                int rowNum = Convert.ToInt32(coordinates[0]);
                int colNum = Convert.ToInt32(coordinates[1]);
                if (battleshipBoard[rowNum, colNum] != ".")
                {
                    battleshipBoard[rowNum, colNum] = ".";
                }
            }
            for (int i = 0; i < battleshipBoard.GetLength(0); i++)
            {
                for (int j = 0; j < battleshipBoard.GetLength(1); j++)
                {
                    if (battleshipBoard[i, j] != ".")
                    {
                        if (shipSinkStatus.ContainsKey(battleshipBoard[i, j]))
                            shipSinkStatus[battleshipBoard[i, j]] = 0;
                    }
                }
            }
            foreach (DictionaryEntry item in shipSinkStatus)
            {
                if (Convert.ToInt32(item.Value) == 1)
                {
                    sunkShipCount++;
                }
            }
            return sunkShipCount;
        }

    }
}
