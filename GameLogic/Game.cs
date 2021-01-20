using System;

namespace UglyTetris.GameLogic
{
    public class Game
    {
        public Game(INextFigureFactory nextFigureFactory)
        {
            _nextFigureFactory = nextFigureFactory;
        }

        private bool IsFalling { get; set; }

        private int _tickCount;

        private int MoveDownPeriodTicks { get; set; } = 50;

        private int FallDownPeriodTicks { get; set; } = 3;

        private int _score;

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private int _level = 1;

        public int Level
        {
            get => _level;
            private set
            {
                _level = value;
                LevelChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private GameState _state = GameState.Running;

        public GameState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Tick()
        {
            if (State == GameState.GameOver)
            {
                return;
            }

            _tickCount++;

            var moveDown = IsFalling
                ? (_tickCount % FallDownPeriodTicks == 0)
                : (_tickCount % MoveDownPeriodTicks == 0);

            if (!moveDown) return;
            var y = FigurePositionY + 1;
            var x = FigurePositionX;

            if (!Field.IsPossibleToPlaceFigure(Figure, x, y))
            {
                Field.LockFigure(Figure, FigurePositionX, FigurePositionY, true);

                var lineCount = Field.RemoveFullLines();
                if (lineCount != 0)
                {
                    var score = lineCount switch
                    {
                        1 => 100,
                        2 => 300,
                        3 => 700,
                        4 => 1500,
                        _ => 0
                    };

                    Score += score;
                }

                RaiseFigureStateChanged();

                var figure = _nextFigureFactory.GetNextFigure();

                if (!ResetFigure(figure))
                {
                    State = GameState.GameOver;
                }

                _tickCount = 0;
                IsFalling = false;
            }
            else
            {
                FigurePositionX = x;
                FigurePositionY = y;
                RaiseFigureStateChanged();
            }
        }

        public void MoveLeft()
        {
            FigurePositionX--;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionX++;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void MoveRight()
        {
            FigurePositionX++;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionX--;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void MoveDown()
        {
            FigurePositionY++;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionY--;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void Rotate()
        {
            Figure.RotateRight();
            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                Figure.RotateLeft();
            }

            RaiseFigureStateChanged();
        }

        public void Drop()
        {
            IsFalling = true;
        }

        public Figure Figure { get; private set; } = new Figure();

        public event EventHandler FigureStateChanged;

        protected void RaiseFigureStateChanged()
        {
            FigureStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler StateChanged;
        public event EventHandler ScoreChanged;
        public event EventHandler LevelChanged;


        public int FigurePositionX { get; private set; } = 6;

        public int FigurePositionY { get; private set; }

        public Field Field;


        public bool ResetFigure(Figure newFigure)
        {
            FigurePositionX = (Field.Xmax - Field.Xmin) / 2;
            FigurePositionY = 0;

            if (Field.IsPossibleToPlaceFigure(newFigure, FigurePositionX, FigurePositionY))
            {
                Figure = newFigure;
                return true;
            }

            return false; //cannot reset figure
        }

        private readonly INextFigureFactory _nextFigureFactory;

        public void CheckLevelChange()
        {
            var level = Score / 1000;
            if (level != Level)
            {
                Level = level;
            }
        }

        public void GoToNextLevel()
        {
            MoveDownPeriodTicks /= 2;

            Field.AddLineBelow();
        }
    }
}