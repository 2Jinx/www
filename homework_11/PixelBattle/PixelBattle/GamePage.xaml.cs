using System.Net.Sockets;
using System.Text.Json;
using GameUtils.Paths;


namespace PixelBattle
{
    public partial class GamePage : ContentPage
    {
        private Color _PlayerColor;
        private string _PlayerName;
        private TcpClient _client;
        private string _Host = "127.0.0.1";
        private int _Port = 8888;
        private StreamReader _Reader;
        private StreamWriter _Writer;

        public GamePage(string userName)
        {
            _PlayerName = userName;
            InitializeComponent();
            SizeChanged += OnPageSizeChanged;
            CreateUser();
            GeneratePage();
        }

        private async Task CreateUser()
        {
            // Создаем клиента только при первом входе
            if (_client == null)
            {
                var rand = new Random();
                _client = new TcpClient();
                _client.Connect(_Host, _Port);

                _Reader = new StreamReader(_client.GetStream());
                _Writer = new StreamWriter(_client.GetStream()) { AutoFlush = true };

                Task.Run(() => ReceiveMessageAsync(_Reader));

                await EnterUserAsync(_Writer, _PlayerName);
            }
        }

        private async Task EnterUserAsync(StreamWriter writer, string userName)
        {
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
        }

        private async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message)) continue;

                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        Print(message);
                    });
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void Print(string message)
        {
            var messageSplit = message.Split();
            var messageType = messageSplit[0];
            var messageJson = messageSplit[1];

            int lastMargin = 90;
            switch (messageType)
            {
                case "SendList":
                    {
                        _PlayersList = JsonSerializer.Deserialize<List<AddUser>>(message)
                            ?? throw new ArgumentNullException(nameof(message));

                        int marginTop = 90;
                        foreach (var player in _PlayersList)
                        {
                            _PlayerColor = Color.FromHex(player.Color!);
                            var playerLabel = new Label
                            {
                                TextColor = _PlayerColor,
                                Text = player.UserName,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Margin = new Thickness(30, marginTop, 0, 0),
                                FontSize = 40,
                                FontFamily = "pixel-font"
                            };

                            _grid.Children.Add(playerLabel);
                            marginTop += 80;
                        }
                        lastMargin = marginTop + 80;
                        break;
                    }
                case "AddUser":
                    {
                        var addUser = JsonSerializer.Deserialize<AddUser>(messageJson)
                                      ?? throw new ArgumentNullException(nameof(messageJson));
                        _PlayerColor = Color.FromHex(addUser.Color!);
                        var playerLabel = new Label
                        {
                            TextColor = _PlayerColor,
                            Text = addUser.UserName,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Margin = new Thickness(30, lastMargin, 0, 0),
                            FontSize = 40,
                            FontFamily = "pixel-font"
                        };

                        _grid.Children.Add(playerLabel);
                        break;
                    }
                case "SendPoint":
                    {
                        var point = JsonSerializer.Deserialize<SendPoint>(messageJson)
                                    ?? throw new ArgumentNullException(nameof(messageJson));

                        int index = (int)(point.Point.X * _columns + point.Point.Y);
                        _boxes[index].BackgroundColor = Color.FromHex(point.Color!);
                        break;
                    }
            }
            
        }
    }

    public partial class GamePage : ContentPage
    {
        // Список с квадратиками
        private List<BoxView> _boxes;
        // Создаем сетку квадратиков
        private const double _rows = 15;
        private const double _columns = 25;
        private double _boxSize = 50;
        private Grid _grid;
        private Grid _grid1;
        private List<AddUser> _PlayersList;

        /// <summary>
        /// Верстка всей игровой страницы кодом
        /// Вынес отдельно для лучшей читаемости кода
        /// </summary>
        private async void GeneratePage()
        {
            _boxes = new List<BoxView>(); // Список с квадратиками
            _grid = new Grid(); // Для всей страницы
            _grid1 = new Grid(); // Для квадратиков

            for (int i = 0; i < _rows; i++)
            {
                _grid1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_boxSize) });
            }

            for (int i = 0; i < _columns; i++)
            {
                _grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_boxSize) });
            }

            // Наполняем сетку квадратиками
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    var box = new BoxView
                    {
                        Margin = new Thickness(2),
                        CornerRadius = 5
                    };

                    box.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            if (box.Color != _PlayerColor)
                            {
                                box.Color = _PlayerColor;
                                var sendPoint = new SendPoint(new System.Drawing.Point(Grid.GetRow(box), Grid.GetColumn(box)), _PlayerColor.ToHex());
                                var json = JsonSerializer.Serialize(sendPoint);
                                _Writer.WriteLine(json);
                                _Writer.Flush();
                            }
                        })
                    });

                    _grid1.Children.Add(box);
                    Grid.SetRow(box, row);
                    Grid.SetColumn(box, column);

                    _boxes.Add(box);
                }
            }

            // Добавляем вертикальную полосу слева от сетки

            var verticalLine = new BoxView
            {
                Color = Colors.Gray,
                WidthRequest = 2,
                HeightRequest = Application.Current.MainPage.Height,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(300, 0, 0, 0)
            };

            var horizontalLine = new BoxView
            {
                Color = Colors.Gray,
                HeightRequest = 2,
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 70, 0, 0)
            };

            var playersLabel = new Label
            {
                TextColor = Colors.Gray,
                Text = "Players",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(80, 15, 0, 0),
                FontSize = 40,
                FontFamily = "pixel-font"
            };

            var gameLabel = new Label
            {
                TextColor = Colors.Gray,
                Text = "Pixel Battle",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(200, 30, 0, 0),
                FontSize = 80,
                FontFamily = "pixel-font"
            };

            _grid.Children.Add(verticalLine);
            _grid.Children.Add(horizontalLine);
            _grid.Children.Add(playersLabel);
            _grid.Children.Add(gameLabel);
            _grid.Children.Add(_grid1);
            _grid1.HorizontalOptions = LayoutOptions.Start;
            _grid1.VerticalOptions = LayoutOptions.Start;
            _grid1.Margin = new Thickness(400, 200, 100, 0);
            // Добавляем все на экран
            Content = _grid;
        }

        private void AdjustLayout()
        {
            // Обновляем размер вертикальной линии
            var verticalLine = (BoxView)_grid.Children.FirstOrDefault(c => c.GetType() == typeof(BoxView) && _grid.GetColumn(c) == 0 && _grid.GetRow(c) == 0);
            if (verticalLine != null)
            {
                verticalLine.HeightRequest = Application.Current.MainPage.Height;
            }

            var horizontalLine = (BoxView)_grid.Children.FirstOrDefault(c => c.GetType() == typeof(BoxView) && _grid.GetColumn(c) == 1 && _grid.GetRow(c) == 0);
            if (horizontalLine != null)
            {
                horizontalLine.WidthRequest = 300;
            }

            if (_grid1 != null)
            {
                _grid1.Margin = new Thickness(400, 200, 100, 0);
            }
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            AdjustLayout(); // Вызываем метод для подстройки макета при изменении размера страницы
        }
    }
}


