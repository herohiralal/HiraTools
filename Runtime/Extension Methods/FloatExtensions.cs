namespace UnityEngine
{
    public static class FloatExtensions
    {
        public static float AsDegrees360(this float input)
        {
            input %= 360;
            if (input < 0) input += 360;
            return input;
        }

        public static void ConvertToDegrees360(this ref float input)
        {
            input %= 360;
            if (input < 0) input += 360;
        }
    }
}