using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL
{
    public class Tensor
    {
        Array _items;

        private Tensor() { }

        /// <summary>
        /// Creates a new <see cref="Tensor"/>.
        /// </summary>
        /// <param name="size">An array representing the size <see cref="Vector"/> of the <see cref="Tensor"/>.</param>
        public Tensor(params int[] size)
        {
            _items = Array.CreateInstance(typeof(double), size);
        }

        /// <summary>
        /// Creates a new <see cref="Tensor"/>.
        /// </summary>
        /// <param name="size">The size <see cref="Vector"/> of the <see cref="Tensor"/>.</param>
        public Tensor(Vector size)
            : this(size.GetRoundedLengths()) { }

        /// <summary>
        /// Returns an array representing the size <see cref="Vector"/> of the <see cref="Tensor"/>.
        /// </summary>
        public int[] GetSizeArray()
        {
            int[] size = new int[_items.Rank];
            for (int i = 0; i < _items.Rank; i++)
            {
                size[i] = _items.GetLength(i);
            }
            return size;
        }

        /// <summary>
        /// Returns the size <see cref="Vector"/> of the <see cref="Tensor"/>.
        /// </summary>
        public Vector GetSizeVector()
        {
            return new Vector(GetSizeArray());
        }

        /// <summary>
        /// Returns an item from the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="position">An array representing the position <see cref="Vector"/> of the item.</param>
        public double GetItem(params int[] position)
        {
            return (double)_items.GetValue(position);
        }

        /// <summary>
        /// Returns an item from the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="position">The position <see cref="Vector"/> of the item.</param>
        public double GetItem(Vector position)
        {
            return GetItem(position.GetRoundedLengths());
        }

        /// <summary>
        /// Sets a value of the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="position">An array representing the position <see cref="Vector"/> of the value to set.</param>
        /// <param name="item">The new value.</param>
        public void SetItem(int[] position, double item)
        {
            _items.SetValue(item, position);
        }

        /// <summary>
        /// Sets a value of the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="position">The position <see cref="Vector"/> of the value to set.</param>
        /// <param name="item">The new value.</param>
        public void SetItem(Vector position, double item)
        {
            _items.SetValue(item, position.GetRoundedLengths());
        }

        /// <summary>
        /// Gets or sets a value of the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="position">An array representing the position <see cref="Vector"/> of the value to set.</param>
        public double this[params int[] position]
        {
            get => GetItem(position);
            set => SetItem(position, value);
        }

        /// <summary>
        /// Gets or sets a value of the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="position">The position <see cref="Vector"/> of the value to set.</param>
        public double this[Vector position]
        {
            get => GetItem(position);
            set => SetItem(position, value);
        }

        /// <summary>
        /// Iterates through all items in the <see cref="Tensor"/> between the <paramref name="start"/>
        /// and <paramref name="end"/> positions.
        /// </summary>
        /// <param name="action">The action to perform at each item of the <see cref="Tensor"/>.</param>
        /// <param name="start">An array representing the position <see cref="Vector"/> of the starting position.</param>
        /// <param name="end">An array representing the position <see cref="Vector"/> of the ending position.</param>
        public void IterateArrayPosition(Action<int[]> action, int[] start, int[] end)
        {
            if (start.Length > _items.Rank)
                throw new Exception("Start position did not have an equal number of dimentions as tensor.");

            if (end.Length > _items.Rank)
                throw new Exception("End position did not have an equal number of dimentions as tensor.");

            int[] position = (int[])start.Clone();
            while (true)
            {
                action(position);
                for (int d = 0; d < _items.Rank; d++)
                {
                    if (position[d] >= Math.Min(_items.GetLength(d) - 1, end[d] - 1))
                    {
                        if (d >= _items.Rank - 1) return;
                        position[d] = 0;
                    }
                    else
                    {
                        position[d]++;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Iterates through all items in the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="action">The action to perform at each item of the <see cref="Tensor"/>.</param>
        public void IterateArrayPosition(Action<int[]> action)
        {
            IterateArrayPosition(action, new int[_items.Rank], GetSizeArray());
        }

        /// <summary>
        /// Iterates through all items in the <see cref="Tensor"/> between the <paramref name="start"/>
        /// and <paramref name="end"/> positions.
        /// </summary>
        /// <param name="action">The action to perform at each item of the tensor.</param>
        /// <param name="start">The position <see cref="Vector"/> of the starting position.</param>
        /// <param name="end">The position <see cref="Vector"/> of the ending position.</param>
        public void IterateVectorPosition(Action<Vector> action, Vector start, Vector end)
        {
            IterateArrayPosition(p => action(new Vector(p)), start.GetRoundedLengths(), end.GetRoundedLengths());
        }

        /// <summary>
        /// Iterates through all items in the <see cref="Tensor"/>.
        /// </summary>
        /// <param name="action">The action to perform at each item of the <see cref="Tensor"/>.</param>
        public void IterateVectorPosition(Action<Vector> action)
        {
            IterateArrayPosition(p => action(new Vector(p)), new int[_items.Rank], GetSizeArray());
        }

        /// <summary>
        /// Applies a function to every item in the tensor.
        /// </summary>
        /// <param name="func">The function to apply.</param>
        /// <returns>The resulting <see cref="Tensor"/>.</returns>
        public Tensor Apply(Func<double, double> func)
        {
            Tensor result = new Tensor(GetSizeArray());
            result.IterateArrayPosition(pos => result[pos] = func(this[pos]));
            return result;
        }

        /// <summary>
        /// Applies a function to every item in the tensor between the <paramref name="start"/>
        /// and <paramref name="end"/> positions.
        /// </summary>
        /// <param name="func">The function to apply.</param>
        /// <param name="start">An array representing the position <see cref="Vector"/> of the starting position.</param>
        /// <param name="end">An array representing the position <see cref="Vector"/> of the ending position.</param>
        /// <returns>The resulting <see cref="Tensor"/>.</returns>
        public Tensor Apply(Func<double, double> func, int[] start, int[] end)
        {
            Tensor result = new Tensor(GetSizeArray());
            result.IterateArrayPosition(pos => result[pos] = func(this[pos]), start, end);
            return result;
        }

        /// <summary>
        /// Applies a function to every item in the tensor between the <paramref name="start"/>
        /// and <paramref name="end"/> positions.
        /// </summary>
        /// <param name="func">The function to apply.</param>
        /// <param name="start">The position <see cref="Vector"/> of the starting position.</param>
        /// <param name="end">The position <see cref="Vector"/> of the ending position.</param>
        /// <returns>The resulting <see cref="Tensor"/>.</returns>
        public Tensor Apply(Func<double, double> func, Vector start, Vector end)
        {
            Tensor result = new Tensor(GetSizeArray());
            result.IterateVectorPosition(pos => result[pos] = func(this[pos]), start, end);
            return result;
        }

        /// <summary>
        /// Creates a copy of the tensor.
        /// </summary>
        public Tensor Copy()
        {
            return new Tensor() { _items = (Array)_items.Clone() };
        }

        /// <summary>
        /// Converts the tensor to an array.
        /// </summary>
        public Array ToArray()
        {
            return (Array)_items.Clone();
        }

        public override string ToString()
        {
            return "T" + GetSizeVector().ToString();
        }

        /// <summary>
        /// Creates a new <see cref="Tensor"/> from an array.
        /// </summary>
        public static Tensor FromArray(params double[] array)
        {
            return new Tensor() { _items = (Array)array.Clone() };
        }

        /// <summary>
        /// Creates a new <see cref="Tensor"/> from an array.
        /// </summary>
        public static Tensor FromArray(Array array)
        {
            return new Tensor() { _items = (Array)array.Clone() };
        }
    }
}