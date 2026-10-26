namespace LanternTrail.Core
{
    public static class StarRating
    {
        /// <summary>
        /// Calculates star rating per Section 4.4:
        /// 0 golden lanterns = 1 star
        /// 1 to 2 golden lanterns = 2 stars
        /// All 3 golden lanterns = 3 stars
        /// </summary>
        public static int Calculate(int goldenLanterns)
        {
            if (goldenLanterns >= 3) return 3;
            if (goldenLanterns >= 1) return 2;
            return 1;
        }
    }
}
