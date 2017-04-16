public class Coordinate
{
    public int x;
    public int y;

    public Coordinate()
    {
        x = 0;
        y = 0;
    }
    public Coordinate(Coordinate coordinate)
    {
        x = coordinate.x;
        y = coordinate.y;
    }

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("Coordinates: ({0}, {1})", x, y);
    }

    public bool IsEqual(Coordinate compared)
    {
        if (this.x == compared.x && this.y == compared.y) {
            return true;
        }
        else {
            return false;
        }
    }
}