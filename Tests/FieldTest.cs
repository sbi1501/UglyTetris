using System;
using System.Collections.Generic;
using System.Windows.Documents;
using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;
using Newtonsoft.Json;

namespace Tests
{
    public class FieldTest
    {
        private static Tile T() => new Tile("Brown");

        private static List<Field> Fields = new List<Field>()
        {
            new Field(new Tile[,]
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
            new Field(new Tile[,]
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
            new Field(new Tile[,]
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
        
        private static Field EmptyField = new Field(new Tile[,]
        {
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
        public void TestCreateField()
        {
            // TODO стоит ли использовать в данном случае теорию, а не факт?
            var serializedEmptyField = JsonConvert.SerializeObject(EmptyField);
            var testField = Field.CreateField(8, 7, "White");
            var serializedTestField = JsonConvert.SerializeObject(testField);
            
            Assert.Equal(serializedEmptyField, serializedTestField);
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(8, 7)]
        public void TestIsInBound(int width, int height)
        {
            var testField = Field.CreateField(width, height, "White");

            for (var i = testField.Xmin; i <= testField.Xmax; i++)
            {
                for (var k = testField.Ymin; k <= testField.Ymax; k++)
                {
                    Assert.True(testField.IsInBounds(i, k));
                }
            }
            
            Assert.False(testField.IsInBounds(testField.Xmax + 1, testField.Ymax + 1));
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(8, 7)]
        public void TestIsEmpty(int width, int height)
        {
            var testField = Field.CreateField(width, height, "White");
            
            for (var i = testField.Xmin + 1; i < testField.Xmax - 1; i++)
            {
                for (var k = testField.Ymin; k < testField.Ymax; k++)
                {
                    Assert.True(testField.IsEmpty(i, k));
                }
            }

            var xCoords = new List<int> {testField.Xmin, testField.Xmax};

            foreach (var xCoord in xCoords)
            {
                for (var k = testField.Ymin; k < testField.Ymax; k++)
                {
                    Assert.False(testField.IsEmpty(xCoord, k));
                }   
            }

            for (var i = testField.Xmin + 1; i < testField.Xmax; i++)
            {
                Assert.False(testField.IsEmpty(i, testField.Ymax));
            }
        }
    }
}