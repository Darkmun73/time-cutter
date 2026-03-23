using System;
using UnityEngine;

class Utility
{
    public static Vector2 GetDirectionVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                return Vector2.left;
            case Direction.Right:
                return Vector2.right;
            case Direction.Up:
                return Vector2.up;
            case Direction.Down:
                return Vector2.down;
            default:
                throw new Exception("There is no direction:" + dir);
        }
    }
}