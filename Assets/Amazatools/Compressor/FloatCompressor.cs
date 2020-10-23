using System;

namespace Amazatools.Compression
{
    public static class FloatCompressor
    {
        public static uint SimpleCompress(float value, int min, int max, float accuracy)
        {
            return Math.Min((uint)((value + -min) * (1.0f / accuracy)), (uint)((max - min) * (1.0 / accuracy)));
        }

        public static float SimpleDecompress(uint value, int min, int max, float accuracy)
        {
            return Clamp((value * accuracy) + min, min, max);
        }

        public static FloatCompression TotalCompression(float value, FloatCompressionSettings settings)
        {
            uint compressed = Math.Min((uint)((value + -settings.Min) * (1.0f / settings.Accuracy)),(uint)((settings.Max - settings.Min) * (1.0 / settings.Accuracy)));
            return new FloatCompression(compressed,settings.Min,settings.Max,settings.Accuracy,(int)BitTotal(compressed));
        }

        public static float TotalUnCompression(FloatCompression compressed)
        {
            return Clamp((compressed._compressed * compressed._accuracy) + compressed._min, compressed._min, compressed._max);
        }

        public static uint BitTotal(uint value)
        {
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            return DeBruijnLSBsSet[unchecked((value | value >> 16) * 0x07c4acddu) >> 27];
        }

        public static byte[] DeBruijnLSBsSet = new byte[] { 0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30, 8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31 };

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }

    [System.Serializable]
    public struct FloatCompression
    {
        public readonly uint _compressed;
        public readonly float _min;
        public readonly float _max;
        public readonly float _accuracy;
        public readonly int _bits;

        public FloatCompression(uint compressed, float min, float max, float accuracy, int bits)
        {
            this._compressed = compressed;
            this._min = min;
            this._max = max;
            this._accuracy = accuracy;
            this._bits = bits;
        }
    }

    [System.Serializable]
    public struct FloatCompressionSettings
    {
        public int Min;
        public int Max;
        public float Accuracy;
    }
}