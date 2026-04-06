using System;
using UnityEngine;

public enum Direction {Left, Right, Up, Down}

public static class DirectionExtensions
{
    public static Vector2 ToVector(this Direction dir)
    {
        return dir switch
        {
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            _ => throw new Exception("There is no direction:" + dir),
        };
    }
}

public class DirectionHelper
{
    public static Direction FromVector(Vector2 vector)
    {
        if (vector == Vector2.left)  return Direction.Left;
        if (vector == Vector2.right) return Direction.Right;
        if (vector == Vector2.up)    return Direction.Up;
        if (vector == Vector2.down)  return Direction.Down;

        throw new Exception($"There is no direction for vector: {vector}!");
    }
}