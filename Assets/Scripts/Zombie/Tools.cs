public static class Tools
{
    public static float MapValues(
        float x,
        float inMin,
        float inMax,
        float outMin,
        float outMax)
    {
        return (x - inMin)
            * (outMax - outMin)
            / (inMax - inMin)
            + outMin;
    }
}