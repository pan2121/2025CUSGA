using System;

namespace Utils
{
    public static class MathUtils
    {
        public enum RangeCompareMode
        {
            /// <summary>
            /// 闭区间
            /// </summary>
            Both,
            /// <summary>
            /// 左闭右开
            /// </summary>
            LeftContains,
            /// <summary>
            /// 左开右闭
            /// </summary>
            RightContains,
            /// <summary>
            /// 开区间
            /// </summary>
            None
        }
        
        public static bool Contains(this (int, int) range, int value,RangeCompareMode compareMode = RangeCompareMode.Both)
        {
            switch (compareMode)
            {
                case RangeCompareMode.Both:
                    return value >= range.Item1 && value <= range.Item2;
                case RangeCompareMode.LeftContains:
                    return value >= range.Item1 && value < range.Item2;
                case RangeCompareMode.RightContains:
                    return value > range.Item1 && value <= range.Item2;
                case RangeCompareMode.None:
                    return value > range.Item1 && value < range.Item2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compareMode), compareMode, null);
            }
        }
    }
}