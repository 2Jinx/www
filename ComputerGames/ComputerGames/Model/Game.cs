namespace ComputerGames.Model
{
    public class Game
    {
        public string Picture { get; set; } // картинка
        public string Name { get; set; } // название
        public string Genre { get; set; } // жанр
        public string ReleaseDate { get; set; } // дата выхода
        public int Rating { get; set; } // оценка
        public string Available { get; set; } // доступно к покупке
        public int Price { get; set; } // цена

        public Game(string picture, string name, string genre, object releaseDate, int rating, bool available, int price)
        {
            this.Picture = "static/" + picture;
            this.Name = name;
            this.Genre = genre;
            this.ReleaseDate = releaseDate.ToString();
            this.Rating = rating;

            if (available)
                this.Available = "In stock";
            else
                this.Available = "Out of stock";

            this.Price = price;
        }
    }
}



