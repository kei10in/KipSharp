namespace Kip
{
    internal static class Extensions
    {
        internal static int? AsInt32(this string self)
        {
            int n;
            if (int.TryParse(self, out n)) return n;
            else return null;
        }

        internal static float? AsFloat(this string self)
        {
            float n;
            if (float.TryParse(self, out n)) return n;
            else return null;
        }
    }
}
