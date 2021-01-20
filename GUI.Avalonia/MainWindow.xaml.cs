using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using UglyTetris.GameLogic;

namespace UglyTetris.AvaloniaGUI
{
    public class MainWindow : Window
    {
        private readonly Game _game;
        private readonly DispatcherTimer _timer;
        private readonly FieldDrawer _fieldDrawer;
        private readonly FigureDrawer _figureDrawer;
        private readonly FigureFactory _figureFactory = new FigureFactory();


        public MainWindow()
        {
            InitializeComponent();

            var mainCanvas = this.FindControl<Canvas>("MainCanvas");
            _figureDrawer = new FigureDrawer(new TileDrawer(mainCanvas));
            _fieldDrawer = new FieldDrawer(new TileDrawer(mainCanvas));

            _game = new Game(new RandomNextFigureFactory());
            _game.FigureStateChanged += GameOnFigureStateChanged;
            _game.ScoreChanged += GameOnScoreChanged;
            _game.StateChanged += GameOnStateChanged;
            _game.LevelChanged += GameOnLevelChanged;

            _game.Field = Field.CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight, "DimGray");
            _game.ResetFigure(_figureFactory.CreateRandomFigure());

            _figureDrawer.DrawFigure(_game.Figure, _game.FigurePositionX, _game.FigurePositionY);
            _fieldDrawer.AttachToField(_game.Field);

            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};
            _timer.Tick += (sender, args) => { _game.Tick(); };
            _timer.Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void GameOnStateChanged(object sender, EventArgs e)
        {
            if (_game.State == GameState.GameOver)
            {
                _timer.Stop();

                var gameOverTextBlock = this.FindControl<TextBlock>("ScoreTextBlock");
                gameOverTextBlock.IsVisible = true; //todo this does not work
            }
        }

        private void GameOnScoreChanged(object sender, EventArgs e)
        {
            var scoreTextBlock = this.FindControl<TextBlock>("ScoreTextBlock");
            scoreTextBlock.Text = _game.Score.ToString(CultureInfo.InvariantCulture);
            _game.CheckLevelChange();
        }

        private void GameOnLevelChanged(object sender, EventArgs e)
        {
            var levelTextBlock = this.FindControl<TextBlock>("LevelTextBlock");
            levelTextBlock.Text = _game.Level.ToString(CultureInfo.InvariantCulture);
            _game.GoToNextLevel();
        }

        private void GameOnFigureStateChanged(object sender, EventArgs e)
        {
            _figureDrawer.DrawFigure(_game.Figure, _game.FigurePositionX, _game.FigurePositionY);
        }

        private void MoveLeft()
        {
            _game.MoveLeft();
        }

        private void MoveRight()
        {
            _game.MoveRight();
        }

        private void MoveDown()
        {
            _game.MoveDown();
        }

        private void Rotate()
        {
            _game.Rotate();
        }

        private void Drop()
        {
            _game.Drop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    MoveLeft();
                    break;
                case Key.Right:
                    MoveRight();
                    break;
                case Key.Up:
                    Rotate();
                    break;
                case Key.Down:
                    MoveDown();
                    break;
                case Key.Space:
                    Drop();
                    break;
            }
        }

        private void HandleLeftButton(object sender, RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void HandleRightButton(object sender, RoutedEventArgs e)
        {
            MoveRight();
        }

        private void HandleUpButton(object sender, RoutedEventArgs e)
        {
            Rotate();
        }

        private void HandleDownButton(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }

        private void HandleSpaceButton(object sender, RoutedEventArgs e)
        {
            Drop();
        }
    }

    internal class RandomNextFigureFactory : INextFigureFactory
    {
        private readonly FigureFactory _figureFactory = new FigureFactory();

        public Figure GetNextFigure()
        {
            return _figureFactory.CreateRandomFigure();
        }
    }
}