using System.Collections.Generic;
using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;

namespace Tests
{
    public class FieldTest
    {
        private static Tile T(string color = "DimGray") => new Tile(color);
        private static TileXy Txy(int x, int y, string color = "DimGray") => new TileXy {Tile = T(color), X = x, Y = y};

        private static readonly Field EmptyField = new Field(
            new[,]
            {
                // 0 -> Y
                // | 
                // v X 
                {T(), T(), T(), T(), T(), T(), T(), T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {null, null, null, null, null, null, null, T(),},
                {T(), T(), T(), T(), T(), T(), T(), T(),},
            });

        private static readonly List<Field> Fields = new List<Field>
        {
            new Field(new[,]
            {
                // 0 -> Y
                // | 
                // v X 
                {T(), T(), T(), T(), T(), T(), T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {T(), T(), T(), T(), T(), T(), T(), T(),},
            }),
            new Field(new[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, null, T(), T(), T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            ),
            new Field(new[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), null, T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), null, T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, null, T(), T(), T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            )
        };

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 0)]
        public void RemoveLines(int fieldIndex, int lineCount)
        {
            var field = Fields[fieldIndex];
            field.RemoveFullLines().Should().Be(lineCount);
        }

        [Fact]
        public void CreateField()
        {
            var testField = Field.CreateField(EmptyField.Width, EmptyField.Height, "DimGray");
            testField.Should().BeEquivalentTo(EmptyField);
        }

        [Fact]
        public void IsInBounds()
        {
            for (var i = EmptyField.Xmin; i <= EmptyField.Xmax; i++)
            {
                for (var k = EmptyField.Ymin; k <= EmptyField.Ymax; k++)
                {
                    EmptyField.IsInBounds(i, k).Should().BeTrue();
                }
            }

            EmptyField.IsInBounds(EmptyField.Xmax + 1, EmptyField.Ymax + 1).Should().BeFalse();
        }

        [Fact]
        public void GetTiles()
        {
            object[] tiles =
            {
                Txy(0, 0),
                Txy(0, 1),
                Txy(1, 1),
                Txy(2, 0),
                Txy(2, 1)
            };

            var testField = Field.CreateField(1, 1, "DimGray");
            testField.GetTiles().Should().BeEquivalentTo(tiles);
        }

        [Fact]
        public void AddLineBelow()
        {
            object[] tiles =
            {
                Txy(0, 0),
                Txy(0, 1),
                Txy(0, 2),
                Txy(1, 1, "Orange"),
                Txy(1, 2),
                Txy(2, 1, "Orange"),
                Txy(2, 2),
                Txy(3, 0),
                Txy(3, 1),
                Txy(3, 2),
            };
            
            var testField = Field.CreateField(2, 2, "DimGray");
            testField.AddLineBelow(2);
            var foo = testField.GetTiles(); 
            testField.GetTiles().Should().BeEquivalentTo(tiles);
        }
    }
}