public static class InteropConversions
{
	public static bool ToBoolean(this byte b) => b != 0;
	public static byte ToByte(this bool b) => (byte) (b ? 1 : 0);
}