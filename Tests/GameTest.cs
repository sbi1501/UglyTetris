using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;

namespace Tests
{
    public class GameTest
    {
        private static Tile T() => new Tile();
        
        [Fact]
        public void RotateFigure()
        {
            //SUT: 
            // Game:
            //   - field
            //   - figure
            // Game.RotateACW

            var game = new Game(new NextFigureFactoryStub());

            game.Field = new Field(new[,]
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

            var figureFactory = new FigureFactory();

            var figure = figureFactory.CreateStandardFigure(FigureType.I);

            var notRotatedFigure = new Figure(figure);

            game.ResetFigure(figure).Should().BeTrue();

            game.Figure.Should().BeEquivalentTo(notRotatedFigure);

            game.Rotate();

            game.Figure.Should().NotBeEquivalentTo(notRotatedFigure);

            game.MoveLeft();
            game.Rotate();
            game.MoveLeft();
            game.Rotate();
            game.MoveLeft();
            game.Rotate();
            game.MoveLeft();
            game.Rotate();
            game.MoveLeft();
            game.Rotate();
            game.MoveLeft();

            // now the figure is at the most left
            // the wall should not let it rotate

            var figureAtLeftWallCopy = new Figure(game.Figure);

            game.Rotate();

            game.Figure.Should().BeEquivalentTo(figureAtLeftWallCopy);
        }

        [Fact]
        public void LevelChanged()
        {
            var game = new Game(new NextFigureFactoryStub());
            
            game.Field = new Field(new[,]
            {
                // 0 -> Y
                // | 
                // v X 
                {T(), T(), T(), T(), T(), T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {null, null, null, null, null, T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {null, null, null, null, T(), T(), T(), T(), T(),},
                {T(), T(), T(), T(), T(), T(), T(), T(), T(),},
            });
            
            var figureFactory = new FigureFactory();

            var figure = figureFactory.CreateStandardFigure(FigureType.I);
            
            game.ResetFigure(figure).Should().BeTrue();

            for (var i = 0; i < 100; i++)  // two offsets down for figure
            {
                game.Tick();
            }
            
            game.CheckLevelChange();

            Assert.Equal(1, game.Level);
            Assert.Equal(1500, game.Score);
        }
        
        [Fact]
        public void LevelNotChanged()
        {
            var game = new Game(new NextFigureFactoryStub());
            
            game.Field = new Field(new[,]
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
                {null, null, null, null, T(), T(), T(), T(),},
                {T(), T(), T(), T(), T(), T(), T(), T(),},
            });
            
            var figureFactory = new FigureFactory();

            var figure = figureFactory.CreateStandardFigure(FigureType.I);
            
            game.ResetFigure(figure).Should().BeTrue();

            for (var i = 0; i < 100; i++)  // two offsets down for figure
            {
                game.Tick();
            }
            
            game.CheckLevelChange();

            Assert.Equal(0, game.Level);
            Assert.Equal(700, game.Score);
        }

        private class NextFigureFactoryStub : INextFigureFactory
        {
            public Figure GetNextFigure()
            {
                return new Figure();
            }
        }
    }
}