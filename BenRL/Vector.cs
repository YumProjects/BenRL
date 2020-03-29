using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL
{
    public class Vector
    {
        /// <summary>
        /// The number of dimentions in the <see cref="Vector"/>.
        /// </summary>
        public int dimentions => lengths.Length;

        /// <summary>
        /// The magnitude of the <see cref="Vector"/>.
        /// </summary>
        public double magnitude => Math.Sqrt(lengths.Sum(l => Math.Pow(l, 2)));

        double[] lengths;

        /// <summary>
        /// Creates a new <see cref="Vector"/>.
        /// </summary>
        /// <param name="lengths">An array representing the dimentions 
        /// of the <see cref="Vector"/>.</param>
        public Vector(params double[] lengths)
        {
            this.lengths = (double[])lengths.Clone();
        }

        /// <summary>
        /// Creates a new <see cref="Vector"/>.
        /// </summary>
        /// <param name="lengths">An array representing the dimentions 
        /// of the <see cref="Vector"/>.</param>
        public Vector(params int[] lengths)
        {
            this.lengths = new double[lengths.Length];
            for(int i = 0; i < lengths.Length; i++)
            {
                this.lengths[i] = lengths[i];
            }
        }

        /// <summary>
        /// Returns an array representing the dimentions of the <see cref="Vector"/>.
        /// </summary>
        public double[] GetLengths()
        {
            return (double[])lengths.Clone();
        }

        /// <summary>
        /// Returns an integer array representing the rounded dimentions of the <see cref="Vector"/>.
        /// </summary>
        public int[] GetRoundedLengths()
        {
            int[] result = new int[dimentions];
            for(int i = 0; i < dimentions; i++)
            {
                result[i] = (int)Math.Round(lengths[i]);
            }
            return result;
        }

        /// <summary>
        /// Returns the total area inside the <see cref="Vector"/>.
        /// </summary>
        public double GetArea()
        {
            if (dimentions == 0) return 0;
            double area = lengths[0];
            for(int i = 1; i < dimentions; i++)
            {
                area *= lengths[i];
            }
            return area;
        }

        /// <summary>
        /// Returns a copy of the current <see cref="Vector"/> with the
        /// specified number of dimentions.
        /// </summary>
        public Vector GetResized(int dimentions)
        {
            Vector result = new Vector(dimentions);
            int length = Math.Min(this.dimentions, dimentions);
            for(int i = 0; i < length; i++)
            {
                result[i] = this[i];
            }
            return result;
        }

        /// <summary>
        /// Gets the length of the specified dimention of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="dimention">The dimention to return.</param>
        public double GetLength(int dimention)
        {
            return lengths[dimention];
        }

        /// <summary>
        /// Sets the length of the specified dimention of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="dimention">The dimention to set.</param>
        public void SetLength(int dimention, double length)
        {
            lengths[dimention] = length;
        }

        /// <summary>
        /// Gets or sets the length of the specified dimention of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="dimention">The dimention to get or set.</param>
        public double this[int index]
        {
            get => GetLength(index);
            set => SetLength(index, value);
        }

        /// <summary>
        /// Applies a function to all dimentions of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="func">The function to apply.</param>
        public Vector Apply(Func<double, double> func)
        {
            Vector result = Zero(dimentions);
            for (int i = 0; i < dimentions; i++)
            {
                result[i] = func(this[i]);
            }
            return result;
        }

        /// <summary>
        /// Returns true all dimentions of the <see cref="Vector"/> meet a condition.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        public bool All(Func<double, bool> condition)
        {
            for (int i = 0; i < dimentions; i++)
            {
                if (!condition(this[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a copy of the <see cref="Vector"/>
        /// </summary>
        public Vector Copy()
        {
            return new Vector(lengths);
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", lengths) + "]";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Vector)) return false;
            return this == (Vector)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Creates a new <see cref="Vector"/> with zero length and
        /// the specified number of dimentions.
        /// </summary>
        /// <param name="dimentions">The number of dimentions for the <see cref="Vector"/>.</param>
        public static Vector Zero(int dimentions)
        {
            return new Vector(new double[dimentions]);
        }

        /// <summary>
        /// Combines to <see cref="Vector"/>s using the specified function.
        /// </summary>
        /// <param name="a">The first <see cref="Vector"/>.</param>
        /// <param name="b">The second <see cref="Vector"/>.</param>
        /// <param name="func">The function used to combine.</param>
        public static Vector Combine(Vector a, Vector b, Func<double, double, double> func)
        {
            Vector result = Zero(Math.Min(a.dimentions, b.dimentions));
            for (int i = 0; i < result.dimentions; i++)
            {
                result.SetLength(i, func(a[i], b[i]));
            }
            return result;
        }

        /// <summary>
        /// Returns true if all dimentions of two <see cref="Vector"/>s meet the specified condition.
        /// </summary>
        /// <param name="a">The first <see cref="Vector"/>.</param>
        /// <param name="b">The second <see cref="Vector"/>.</param>
        /// <param name="condition">The condition to check.</param>
        public static bool CombineAll(Vector a, Vector b, Func<double, double, bool> condition)
        {
            int length = Math.Min(a.dimentions, b.dimentions);
            for (int i = 0; i < length; i++)
            {
                if (!condition(a[i], b[i])) 
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the sum of two <see cref="Vector"/>s.
        /// </summary>
        public static Vector operator +(Vector a, Vector b)
        {
            return Combine(a, b, (x, y) => x + y);
        }

        /// <summary>
        /// Returns the difference of two <see cref="Vector"/>s.
        /// </summary>
        public static Vector operator -(Vector a, Vector b)
        {
            return Combine(a, b, (x, y) => x - y);
        }

        /// <summary>
        /// Returns a <see cref="Vector"/> multiplied by a value.
        /// </summary>
        public static Vector operator *(Vector a, double b)
        {
            return a.Apply(x => x * b);
        }

        /// <summary>
        /// Returns a <see cref="Vector"/> divided by a value.
        /// </summary>
        public static Vector operator /(Vector a, double b)
        {
            return a.Apply(x => x / b);
        }

        /// <summary>
        /// Returns true if all dimentions of two <see cref="Vector"/>s are equal.
        /// </summary>
        public static bool operator ==(Vector a, Vector b)
        {
            return CombineAll(a, b, (x, y) => x == y);
        }

        /// <summary>
        /// Returns true if any dimentions of two <see cref="Vector"/>s are not equal.
        /// </summary>
        public static bool operator !=(Vector a, Vector b)
        {
            return !CombineAll(a, b, (x, y) => x == y);
        }

        /// <summary>
        /// Returns true if one <see cref="Vector"/> is greater than another in any dimention.
        /// </summary>
        public static bool operator >(Vector a, Vector b)
        {
            return !CombineAll(a, b, (x, y) => x <= y);
        }

        /// <summary>
        /// Returns true if one <see cref="Vector"/> is less than another in any dimention.
        /// </summary>
        public static bool operator <(Vector a, Vector b)
        {
            return !CombineAll(a, b, (x, y) => x >= y);
        }
    }
}