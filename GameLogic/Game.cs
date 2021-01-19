using System;

namespace UglyTetris.GameLogic
{
    public class Game
    {
        public Game(INextFigureFactory nextFigureFactory)
        {
            _nextFigureFactory = nextFigureFactory;
        }
        
        public bool IsFalling { get; set; }

        private int _tickCount = 0;

        int MoveDownPeriodTicks { get; } = 50;

        private int FallDownPeriodTicks { get; } = 3;


        private int _lines = 0;
        public int Lines
        {
            get => _lines;
            private set
            {
                _lines = value;
                LinesChanged?.Invoke(this, EventArgs.Empty);
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

            bool moveDown = IsFalling
                ? (_tickCount % FallDownPeriodTicks == 0)
                : (_tickCount % MoveDownPeriodTicks == 0);


            if (moveDown)
            {
                var y = FigurePositionY + 1;
                var x = FigurePositionX;

                if (!Field.IsPossibleToPlaceFigure(Figure, x, y))
                {
                    Field.LockFigure(Figure, FigurePositionX, FigurePositionY, true);
                    
                    var lineCount = Field.RemoveFullLines();
                    Lines += lineCount;

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
        
        public event EventHandler LinesChanged; // may be replaced with INotifyPropertyChanged interface implementation
        
        public event EventHandler StateChanged; // may be replaced with INotifyPropertyChanged interface implementation

        

        public int FigurePositionX { get; private set; } = 6;

        public int FigurePositionY { get; private set; } = 0;

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

        private INextFigureFactory _nextFigureFactory;
    }
}