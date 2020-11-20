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
            return new FloatCompression(compressed,settings.Min,settings.Max,settings.Accuracy,Log2(compressed));
        }

        public static float TotalUnCompression(FloatCompression compressed)
        {
            return Clamp((compressed._compressed * compressed._accuracy) + compressed._min, compressed._min, compressed._max);
        }
        
        public static int Log2(uint x)
        {
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x = x >> 1;
            x = x - ((x >> 1) & 0x55555555);
            x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
            x = ((x + (x >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
            return (int)x+1;
        }
        
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) { return min; }
            else if (val.CompareTo(max) > 0) { return max; }
            else { return val; }          
        }
    }
     
    [System.Serializable]
    public struct FloatCompression  : IEquatable<FloatCompression>
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

        public bool Equals(FloatCompression other)
        {
            if (other._compressed == this._compressed)
            {
                return true;
            }
                return false;
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
