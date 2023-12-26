using System.Net.Sockets;
using System.Text.Json;

namespace PixelBattle
{
    public partial class MainPage : ContentPage
    {
        private Color _playerColor;

        public MainPage()
        {
            GeneratePage();
        }

        /// <summary>
        /// Обрабатываем нажатие кнопки входа на главной странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Parent is StackLayout stackLayout)
                {
                    var nameEntry = stackLayout.Children.FirstOrDefault(c => c is Entry) as Entry;

                    if (!string.IsNullOrEmpty(nameEntry?.Text))
                    {
                        string userName = nameEntry.Text;
                        nameEntry.Text = string.Empty;

                        await Navigation.PushAsync(new GamePage(userName));
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Неверное имя пользователя", "OК");
                    }
                }
            }
        }
    }

    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Верстка всей главной страницы кодом
        /// Вынес отдельно для лучшей читаемости кода
        /// </summary>
        private async void GeneratePage()
        {
            // Блок с контентом для всей страницы
            var grid = new Grid();

            // Фон главной страницы
            var backgroundImage = new Image
            {
                Source = "background.png",
                Aspect = Aspect.AspectFill
            };

            grid.Children.Add(backgroundImage); // Добавили фон на экран

            // Отдельный блок для кнопки и поля с вводом никнейма
            var mainGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // Устанавливаем расположение элементов в самом блоке
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Добавляем два блока для отступа и элементов интерфейса
            var topStackLayout = new StackLayout(); // Для отступа
            var bottomStackLayout = new StackLayout // Для кнопки и поля с вводом никнейма
            {
                VerticalOptions = LayoutOptions.Center
            };

            // Поле для ввода никнейма пользователя
            var nameEntry = new Entry
            {
                Placeholder = "Введите ваше имя",
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.White,
                TextColor = Colors.Black
            };
            bottomStackLayout.Children.Add(nameEntry); // Добавили поле с вводом никнейма на экран

            // Кнопка для входа в игру
            var submitButton = new Button
            {
                Text = "Войти",
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(30)
            };
            submitButton.Clicked += SubmitClicked; // Добавили обработчик событий на кнопку (на нажатие)
            bottomStackLayout.Children.Add(submitButton); // Добавили кнопку на экран

            mainGrid.Children.Add(topStackLayout);
            mainGrid.Children.Add(bottomStackLayout);

            // Перенос всего в нижнюю часть экрана
            Grid.SetRow(topStackLayout, 0); 
            Grid.SetRow(bottomStackLayout, 1); 

            grid.Children.Add(mainGrid); // Добавили все в главный блок страницы

            Content = grid; // Установили главный блок контентом страницы
        }
    }
}
