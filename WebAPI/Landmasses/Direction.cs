namespace WebAPI.Landmasses;

public enum Direction
{
    South,
    North,
    West,
    East,

    NorthWest,
    NorthEast,
    SouthWest,
    SouthEast,
}




public static class DirectionHelper
{
    public static IReadOnlyList<Direction> NonRedundantNeighborDirection = new List<Direction>()
    {
            Direction.West,
            Direction.NorthWest,
            Direction.North,
            Direction.NorthEast,

    }.AsReadOnly();

    public static Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.South: return Direction.North;
            case Direction.North: return Direction.South;

            case Direction.West: return Direction.East;
            case Direction.East: return Direction.West;

            case Direction.NorthWest: return Direction.SouthEast;
            case Direction.NorthEast: return Direction.SouthWest;

            case Direction.SouthWest: return Direction.NorthEast;
            case Direction.SouthEast: return Direction.NorthWest;

            default: throw new Exception("error");
        }
    }

    public static bool IsEastern(Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                {
                    return true;
                }
            case Direction.NorthEast:
                {
                    return true;

                }
            case Direction.SouthEast:
                {
                    return true;

                }
        }
        return false;
    }
}
