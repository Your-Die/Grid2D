using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chinchillada.Grid
{
    [Serializable]
    public class Grid2D<T> : IGrid2D<T>
    {
        private readonly T[,] items;

        public int Height { get; }

        public int Width { get; }

        public int Size => this.Width * this.Height;

        public Vector2Int Shape => new(this.Width, this.Height);


        public IEnumerable<Vector2Int> Coordinates
        {
            get
            {
                for (int x = 0; x < this.Width; x++)
                for (int y = 0; y < this.Height; y++)
                    yield return new Vector2Int(x, y);
            }
        }

        public IEnumerable<T> Values => this.Coordinates.Select(coordinate => this[coordinate]);

        public T this[int x, int y]
        {
            get => this.items[x, y];
            set => this.items[x, y] = value;
        }

        public T this[Vector2Int position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        public Grid2D(int width, int height, T value) : this(width, height, () => value)
        {
        }

        public Grid2D(int width, int height, Func<T> valueInitializer) : this(width, height)
        {
            for (int x = 0; x < width; x++)
            for (int y = 0; y < this.Height; y++)
                this.items[x, y] = valueInitializer.Invoke();
        }

        public Grid2D(Vector2Int shape) : this(shape.x, shape.y)
        {
        }

        public Grid2D(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.items = new T[width, height];
        }

        public Grid2D(T[,] items)
        {
            this.Width = items.GetLength(0);
            this.Height = items.GetLength(1);

            this.items = items;
        }
        
        public void Write(T[] flatGrid)
        {
            for (var y = 0; y < this.Height; y++)
            for (var x = 0; x < this.Width; x++)
            {
                var invertedY = this.Height - (y + 1);
                this.items[x, y] = flatGrid[x + invertedY * this.Width];
            }
        }

        public Grid2D<T> Copy()
        {
            var itemsCopy = (T[,])this.items.Clone();
            return new Grid2D<T>(itemsCopy);
        }

        public Grid2D<T> CopyShape() => new(this.Width, this.Height);

        public Grid2D<TOutput> CopyShape<TOutput>() => new(this.Width, this.Height);

        public bool MatchesShape(Grid2D<T> other) => this.Width == other.Width && this.Height == other.Height;

        public bool Contains(Vector2Int position) => this.Contains(position.x, position.y);
        public bool Contains(int x, int y) => this.ContainsX(x) && this.ContainsY(y);
        public bool ContainsX(int x) => x >= 0 && x < this.Width;
        public bool ContainsY(int y) => y >= 0 && y < this.Height;
        
        public override string ToString() => $"{typeof(T)}[{this.Width}, {this.Height}]";
        
        #region IEquality

        public bool Equals(Grid2D<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            // Compare dimensions
            if (this.Width != other.Width || this.Height != other.Height)
                return false;

            // Compare contents.
            for (int x = 0; x < this.Width; x++)
            for (int y = 0; y < this.Height; y++)
            {
                if (!Equals(this[x, y], other[x, y]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Grid2D<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (this.items != null ? this.items.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Width;
                hashCode = (hashCode * 397) ^ this.Height;
                return hashCode;
            }
        }

        #endregion
    }
}