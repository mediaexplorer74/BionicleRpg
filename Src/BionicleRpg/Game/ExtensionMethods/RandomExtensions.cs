
// Type: GameManager.RandomExtensions
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer


    using SharpDX;
    using System;

    #nullable disable
    namespace GameManager
    {
        public static class RandomExtensions
        {
            public static float NextFloat(this Random random, float min, float max)
            {
                return MathUtil.Lerp(min, max, (float)random.NextDouble());
            }

            public static double NextDoubleRange(this Random random, double min, double max)
            {
                return MathUtil.Lerp(min, max, random.NextDouble());
            }
        }
    }

    public static class MathUtil
    {
        public static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }

        public static double Lerp(double start, double end, double amount)
        {
            return start + (end - start) * amount;
        }
    }
    

