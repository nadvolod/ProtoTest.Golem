using System.Windows;

namespace Golem.Purple.PurpleCore
{
    //This was copied from TestStack.White source code
    //Teststack.White available at http://docs.teststack.net/White/GettingStarted.html
    public static class RectX
    {
        public static readonly Point UnlikelyWindowPosition = new Point(-10000, -10000);

        public static bool IsZeroSize(this Rect rect)
        {
            return rect.Height == 0 && rect.Width == 0;
        }

        public static Point Center(this Rect rect)
        {
            var topLeftX = rect.Left;
            var topRightX = rect.Right;
            return new Point((int) (topLeftX + (topRightX - topLeftX)/2), (int) (rect.Top + (rect.Bottom - rect.Top)/2));
        }

        public static Point East(this Rect rectangle, int by)
        {
            return new Point((int) (rectangle.Right + by), rectangle.Center().Y);
        }

        public static Point ImmediateExteriorEast(this Rect rectangle)
        {
            return new Point((int) (rectangle.Right + 1), rectangle.Center().Y);
        }

        public static Point ImmediateInteriorEast(this Rect rectangle)
        {
            return new Point((int) (rectangle.Right - 1), rectangle.Center().Y);
        }

        public static Point ImmediateExteriorWest(this Rect rectangle)
        {
            return new Point((int) (rectangle.Left - 1), rectangle.Center().Y);
        }

        public static Point ImmediateInteriorNorth(this Rect rectangle)
        {
            return new Point(rectangle.Center().X, rectangle.Top + 1);
        }

        public static Point ImmediateInteriorSouth(this Rect rectangle)
        {
            return new Point(rectangle.Center().X, rectangle.Bottom - 1);
        }
    }
}