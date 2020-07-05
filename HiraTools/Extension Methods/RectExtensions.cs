// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace UnityEngine
{
    public static class RectExtensions
    {
        #region Horizontal

        #region Left-Focus

        public static Rect KeepToLeftFor(this in Rect rect, in int width) =>
            new Rect(rect.x, rect.y, width, rect.height);

        public static Rect ShiftToRightBy(this in Rect rect, in int amount) =>
            new Rect(rect.x + amount, rect.y, (int) rect.width - amount, rect.height);

        public static Rect ShiftToRightBy(this in Rect rect, in int amount, in int width) =>
            new Rect(rect.x + amount, rect.y, width, rect.height);

        #endregion

        #region Right-Focus

        public static Rect KeepToRightFor(this in Rect rect, in int amount) =>
            new Rect(rect.x + rect.width - amount, rect.y, amount, rect.height);

        public static Rect ShiftToLeftBy(this in Rect rect, in int amount) =>
			new Rect(rect.x, rect.y, (int) rect.width - amount, rect.height);

        public static Rect ShiftToLeftBy(this in Rect rect, in int amount, in int width) =>
            new Rect(rect.x + rect.width - amount - width, rect.y, width, rect.height);

        #endregion

        #endregion

        #region Vertical

        #region Top-Focus

        public static Rect KeepToTopFor(this in Rect rect, in int amount) =>
            new Rect(rect.x, rect.y, rect.width, amount);

        public static Rect ShiftToBottomBy(this in Rect rect, in int amount) =>
            new Rect(rect.x, rect.y + amount, rect.width, (int) rect.height - amount);

        public static Rect ShiftToBottomBy(this in Rect rect, in int amount, in int height) =>
            new Rect(rect.x, rect.y + amount, rect.width, height);

        #endregion

        #region Bottom-Focus

        public static Rect KeepToBottomFor(this in Rect rect, in int amount) =>
            new Rect(rect.x, rect.y + rect.height - amount, rect.width, amount);

        public static Rect ShiftToTopBy(this in Rect rect, in int amount) =>
			new Rect(rect.x, rect.y, rect.width, (int) rect.height - amount);

        public static Rect ShiftToTopBy(this in Rect rect, in int amount, in int height) =>
            new Rect(rect.x, rect.y + rect.height - amount - height, rect.width, height);

        #endregion

        #endregion
    }
}