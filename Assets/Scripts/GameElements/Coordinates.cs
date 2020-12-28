using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Coordinates
{
    public Row _Row { get; set; }
    public Column _Column{ get; set; }

    public Coordinates(int row, int column)
    {
        if (IsInputValid(row, column))
            throw new ArgumentException("Coordinates must be in the range from 1 to 10");
            _Row = (Row)(-row);
            _Column = (Column)column;
    }

    public Coordinates(Row row, Column column)
    {
        if (IsInputValid(row, column))
            throw new ArgumentException("Coordinates must be in the range from 1 to 10");
        _Row = row;
        _Column = column;
    }

    private static bool IsInputValid(Row row, Column column)
        => ((int)row <= 10 && (int)row >= 1
            && (int)column >= 1 && (int)column <= 10);

    private static bool IsInputValid(int row, int column)
        => (row <= 10 && row >= 1
            && column >= 1 && column <= 10);

    public enum Row
    {
        R1 = -1,
        R2 = -2,
        R3 = -3,
        R4 = -4,
        R5 = -5,
        R6 = -6,
        R7 = -7,
        R8 = -8,
        R9 = -9,
        R10 = -10,
    }

    public enum Column
    {
        A = 1,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J
    }
}

public static class CoordinatesExtension
{
    public static Coordinates Vector2IntToCoordinates(this Vector2Int vector)
    {
        return new Coordinates(vector.y, vector.x);
    }

    public static Vector2Int CoordinatesToVector2Int(this Coordinates coordinates)
    {
        return new Vector2Int((int)coordinates._Column, -(int)(coordinates._Row));
    }
}

