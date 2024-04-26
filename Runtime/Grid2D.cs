using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.Grid
{
    [Serializable]
    public class Grid2D<T> : IEnumerable<T>, IGrid2D<T>
    {
        [SerializeField] private T[,] items;

        [SerializeField] private int width;

        [SerializeField] private int height;

        public int Height => this.height;

        public int Size => this.Width * this.Height;

        public Vector2Int Shape => new(this.Width, this.Height);

        public int Width
        {
            get => this.width;
            set => this.width = value;
        }

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

        public Grid2D()
        {
        }

        public Grid2D(int width, int height, T value) 
            : this(width, height, () => value)
        {
        }

        public Grid2D(int width, int height, Func<T> valueInitializer)
        {
            this.width = width;
            this.height = height;

            this.items = new T[this.width, this.height];

            for (var x = 0; x < width; x++)
            for (var y = 0; y < this.height; y++)
                this.items[x, y] = valueInitializer.Invoke();
        }
        
        public Grid2D(T[,] items)
        {
            this.Width = items.GetLength(0);
            this.height = items.GetLength(1);

            this.items = items;
        }

        public Grid2D(int width, int height)
        {
            this.Width = width;
            this.height = height;

            this.items = new T[width, height];
        }

        public Grid2D(Vector2Int shape)
            : this(shape.x, shape.y)
        {
        }



        public Grid2D<T> Copy()
        {
            var itemsCopy = this.CopyItems();
            return new Grid2D<T>(itemsCopy);
        }

        public T[,] CopyItems()
        {
            var copy = new T[this.width, this.height];
            
            for (var x = 0; x < this.width; x++)
            for (var y = 0; y < this.height; y++)
                copy[x, y] = this.items[x, y];

            return copy;
        }
        
        public Grid2D<T> CopyShape() => new(this.Width, this.Height);

        public Grid2D<TOutput> CopyShape<TOutput>() => new(this.Width, this.Height);

        public bool Contains(Vector2Int position)
        {
            return position.x >= 0 && position.x < this.Width &&
                   position.y >= 0 && position.y < this.Height;
        }

        public GridNeighborhood GetRegion(int x, int y, int radius) => new(this, x, y, radius);

        public IEnumerator<T> GetEnumerator()
        {
            for (var x = 0; x < this.Width; x++)
            for (var y = 0; y < this.Height; y++)
                yield return this[x, y];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}