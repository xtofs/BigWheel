using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
                             
namespace Xtof.Numeric
{

    [Serializable]
    public struct Rational : IConvertible, IComparable<Rational> , IEquatable<Rational>
    {
        public Rational(long numerator, long denominator = 1)
        {
            Normalize(ref numerator, ref denominator);
            this.Numerator = denominator == 0 ? Math.Sign(numerator) : numerator;
            this.Denominator = numerator == 0 ? 1 : denominator;
        }

        public static Rational Zero = new Rational(0, 1);
        public static Rational One = new Rational(1, 1);
        public static Rational MinusOne = new Rational(-1, 1);

        public long Numerator { get; }

        public long Denominator { get; }

        public bool IsInf {  get { return Denominator == 0; } }
        public bool IsPositiveInf { get { return Denominator == 0 && Numerator == 1; } }
        public bool IsNegativeInf { get { return Denominator == 0 && Numerator == -1; } }


        private static void Normalize(ref long numerator, ref long denominator)        
        {
            if (denominator < 0) // denominator is always positive
            {
                numerator = -numerator;
                denominator = -denominator;
            }        
            var gcd = GCD(numerator, denominator);
            if (gcd != 1 && gcd != 0)
            {
                numerator /= gcd;
                denominator /= gcd;
            }
        }

        //  Lowest Common Denominator
        private static long LCD(long a, long b)
        {
            if (a == 0 && b == 0)
                return 0;

            return (a * b) / GCD(a, b);
        }

        // Greatest Common Devisor
        private static long GCD(long a, long b)
        {
            if (a < 0)
                a = -a;
            if (b < 0)
                b = -b;

            while (a != b)
            {
                if (a == 0)
                    return b;
                if (b == 0)
                    return a;

                if (a > b)                
                    a %= b;                
                else                
                    b %= a;                
            }
            return a;
        }

        #region IConvertible

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Numerator) / Convert.ToDouble(Denominator);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            if (Denominator == 0)
                return Numerator > 0 ? decimal.MaxValue : decimal.MinValue;
            return Convert.ToDecimal(Numerator) / Convert.ToDecimal(Denominator);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)Numerator).ToSingle(provider) / ((IConvertible)Denominator).ToSingle(provider);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return this != Zero;
        }

        public char ToChar(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToChar(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToSByte(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToByte(provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToInt16(provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToUInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToInt32(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToUInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToInt64(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToUInt64(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)this.ToDecimal(provider)).ToDateTime(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion

        public int CompareTo(Rational other)
        {
            return this.ToDecimal(null).CompareTo(other.ToDecimal(null));
        }

        public bool Equals(Rational other)
        {
            return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
        }

        public override bool Equals(object obj)
        {
            return obj is Rational && Equals((Rational)obj);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Numerator, Denominator).GetHashCode();
        }

        public static bool operator == (Rational a, Rational b)
        {
            return a.Equals(b);
        }
        public static bool operator != (Rational a, Rational b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return String.Format("{0}/{1}", Numerator, Denominator);
        }

        public static implicit operator Rational(int n)
        {
            return new Rational(n);
        }

        public static implicit operator double(Rational r)
        {
            return (double)r.Numerator / (double)r.Denominator;
        }

        public static implicit operator decimal(Rational r)
        {
            return (decimal)r.Numerator / (decimal)r.Denominator;
        }

        public static implicit operator Rational(double value)
        {
            int n = 9;  // 2^10 ~= 10^3 , 2^32 ~= 10^9  
            var denom = Math.Pow(10, n);
            return new Rational((long)(value * denom), (long)denom);
        }

        #region operators

        public static Rational operator +(Rational r, Rational q)
        {
            return new Rational(r.Numerator * q.Denominator + q.Numerator * r.Denominator, r.Denominator * q.Denominator);
        }

        public static Rational operator -(Rational r)
        {
            return new Rational(-r.Numerator, r.Denominator);
        }

        public static Rational operator -(Rational r, Rational q)
        {
            return r + (-q);
        }


        public static Rational operator * (Rational r, Rational q)
        {
            return new Rational(r.Numerator * q.Numerator, r.Denominator * q.Denominator);
        }

        public static Rational operator / (Rational r, Rational q)
        {
            return new Rational(r.Numerator * q.Denominator, r.Denominator * q.Numerator);
        }

        #endregion
    }
}
