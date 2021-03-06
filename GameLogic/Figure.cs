﻿using System;
using System.Linq;

namespace UglyTetris.GameLogic
{
    public class Figure
    {
        public Figure()
        {
            
        }

        public Figure(Figure figure)
        {
            Tiles = new Tile[figure.Width, figure.Height];

            for (var x = 0; x < figure.Width; x++)
            for (var y = 0; y < figure.Height; y++)
            {
                var tile = figure.Tiles[x, y];
                Tiles[x, y] = tile == null ? null : new Tile(tile.Color);
            }
        }

        public Figure(string tileMap, string color)
        {
            var lines = tileMap.Split(Environment.NewLine);

            var width = lines.Max(l => l.Length);
            var height = lines.Length;

            Tiles = new Tile[width, height];

            for (var y = 0; y < height; y++)
            {
                var line = lines[y];
                for (var x = 0; x < width; x++)
                {
                    char c = ' ';

                    if (x < line.Length)
                    {
                        c = line[x];
                    }

                    var tile = char.IsWhiteSpace(c) ? null : new Tile(color);

                    Tiles[x, y] = tile;
                }
            }
        }

        public Tile[,] Tiles = new Tile[,]
        {
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null}
        };

        public int Width => Tiles.GetUpperBound(0) + 1;
        public int Height => Tiles.GetUpperBound(1) + 1;
        
        public void RotateLeft()
        {
            var newTiles = new Tile[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[j, Width - i - 1] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }

        public void RotateRight()
        {
            var newTiles = new Tile[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[Height - j - 1, i] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }
    }
}