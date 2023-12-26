
## Домашние работы

- [Практика №1](#практика-1)
- [Практика №2](#практика-2)
- [Практика №3](#практика-3)
- [Практика №4](#практика-4)
- [Практика №5](#практика-5)
- [Практика №6](#практика-6)
- [Практика №7](#практика-7)
- [Практика №8](#практика-8)
- [Практика №9](#практика-9)
- [Практика №10](#практика-10)

---

## Дополнительные задания

- [Дополнительное задание №1](#дополнительное-задание-№1)
- [Дополнительное задание №2](#дополнительное-задание-№2)

---

## Практика №1

#### Задание

1) Необходимо сверстать страницу поиcковика [Google](https://www.google.com/)

2) Необходимо сверстать страницу [КТТС](https://kttc.ru/wot/ru/)

#### Google

- [Проект](https://github.com/2Jinx/www/tree/main/homework_1/Google "Проект")  
- [Результат верстки](https://2Jinx.github.io/www/homework_1/Google/ "Результат верстки")

#### KTTC

- [Проект](https://github.com/2Jinx/www/tree/main/homework_1/kttc "Проект")  
- [Результат верстки](https://2Jinx.github.io/www/homework_1/kttc/ "Результат верстки")

---

## Практика №2

#### Задание

1) Необходимо сверстать страницу входа в [Battle.net](https://eu.account.battle.net/login/ru/)

#### Battle.net

- [Проект](https://github.com/2Jinx/www/tree/main/homework_2/Battle.net "Проект")  
- [Результат верстки](https://2Jinx.github.io/www/homework_2/Battle.net/ "Результат верстки")

---

## Практика №3

#### Задание

1) Переписать пример из [Метанита](https://metanit.com/sharp/net/7.1.php), указать свой порт и сделать так, чтобы он работал

2) Сделать лог в консоль о том, что сервер запущен и о том, что сервер прекратил работу

3) Добавить в проект файл `google.html`

4) При переходе в браузере на страницу, например, localhost:2323 отправлять страницу `google.html`

5) Добавить в проект файл `appsettings.json` следующего вида:
```
{
    "Port": 2323,
    "Address": http://127.0.0.1
}
```

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_3/MyHttpServer "Проект")  

> Для сервера установлен адрес - `localhost` и порт - `5120`.

---

## Практика №4

#### Задание

1) В конфиг файл `appsettings.json` добавить `StaticFilesPath = "static"`

2) По умолчанию искать в этой папке файл `index.html`, если его нет - выводить в консоль, что такой-то файл не найден

3) После пункта 1 сделать обработчики на существование этой папки, если её нет создать её

4) Сделать отдельный класс для работы и подгрузки конфигурации сервера

5) Вынести логику запуска, остановки сервера в отдельный класс `Server.cs`, т.е. в `Program.cs` должен быть только запуск вашего сервера

6) Если при переходе, например, по пути http://127.0.0.1:2323/static/google.html (только html файлы) файла нет, то печатать текст `"404 файл не найден"`

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_4/MyHttpServer "Проект") 

> Для сервера установлен адрес - `localhost` и порт - `5120`.

---

## Практика №5

#### Задание

1) Вынести в отдельные файлы стили из файла `index.html`(Battle.net).  HTML файл у `Batlle.net` должен содержать папки `images`, `styles` и соответствующие файлы. Все картинки должны лежать в папке `images`

2) Перенести содержимое сайта `Battle.net` в папку `static`

3) Реализовать открытие сервером по пути `localhost:8082` страницы `index.html` с подгрузкой всех статических файлов.    
Например: при переходе по пути `localhost:8082/static/styles/all.css` должен подгружать файл `all.css` который хранится в каталоге `static`. (`Battle.net` должен отображаться корректно без подклчения к интернету)

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_5/MyHttpServer "Проект") 

> Для сервера установлен адрес - `localhost` и порт - `5120`.

---

## Практика №6 

#### Задание

Необходимо реализовать отправку сообщения  на почту с логином и паролем, введеным на странице `Battle.net` на вашу почту с помощью `HttpServer`
 
1) Добавить в проект папку `Services`

2) В папке `Services` создать интерфейс `IEmailSenederServis` и класс `EmailsSenderService`

3) Добавить параметры в `appsettings.json`, а именно `EmailSender`, `PasswordSender`, `FromName`, `HttpServerHost/Port`

4) Произвести рефакторинг кода. С помощью паттерна цепочки обязанностей вынести отдельно логику загрузки статический файлов в `StaticFilesHandler`

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_6/MyHttpServer "Проект")

> Для сервера установлен адрес - `localhost` и порт - `5120`.  
> Для проверки отправки сообщений необходимо в файле `appsettings.json` поменять значение у `FromEmail` на адрес своей почты.

---

## Практика №7

#### Задание

1) Добавить в проект папку `Controllers` добавить в неё файл `AuthenticationController` с методом `SendEmail`

2) В папку `Handlers` добавить `ControllersHandler`. Реализовать в нем логику вызова методов по пути, например, `localhost:2323/{controllerName}/{methodName}`

3) `controllerName` должен браться из аттрибута `HttpController`(реализовать аттрибут)

4) `methodName` должен браться из имени метода класса (реализовать аттрибут)

5) В `SendEmail` перенести логику отправки сообщения на почту. Теперь `Battle.net` должен отправлять данные с формы по пути `localhost:2323/Authentication/SendEmail`

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_7/MyHttpServer "Проект")

> Для сервера установлен адрес - `localhost` и порт - `5120`. 
> Для проверки отправки сообщений необходимо в файле `appsettings.json` поменять значение у `FromEmail` на адрес своей почты.

---

## Практика №8

#### Задание

1) Добавить в AccountControllers 5 методов:` Add [POST]`, `Delete [POST]`, `Update [POST]`, `GetAll [GET]`, `GetById [GET]`

2) Добавить статический список с моделью `Аккаунт`. И реализовать логику всех 5 методов. 

3) Т.е. при переходи по пути `localhost:2323/Account/GetAll` в браузере в виде `json` должен отобразится все данные со списка.

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_8/MyHttpServer "Проект")

> Для сервера установлен адрес - `localhost` и порт - `5120`. 
> Для проверки отправки сообщений необходимо в файле `appsettings.json` поменять значение у `FromEmail` на адрес своей почты.

> Для хранения списка аккаунтов используется json-файл `accounts_list.json`, который будет считываться при каждом вызове методов `AccountsController` и перезаписываться при изменении данных (т.е. после выполнения методов `Add [POST]`, `Delete [POST]`, `Update [POST]`).    
> Для вызова GET-методов необходимо прописать их вызов в аддресную строку браузера: `localhost:5120/Account/GetAll` и `localhost:5120/Account/GetById/1`.   
> Для вызова POST-методов на главной странице была добавлена кнопка с иконкой космонавта для перехода на страницу управления аккаунтами (первый ряд с кнопками, последняя кнопка).

---

## Практика №9

#### Задание

1) Создать базу данных `BattleDB` 

2) Добавить таблицу `Account`

3) Переделать логику методов `AccountController`. Данные теперь должны браться не из статическго списка, а из БД и соответственно там изменяться и записываться.

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_9/MyHttpServer "Проект")

> Для сервера установлен адрес - `localhost` и порт - `5120`. 
> Для проверки отправки сообщений необходимо в файле `appsettings.json` поменять значение у `FromEmail` на адрес своей почты.
> Для работы с базой данных была добавлена ORM с методами `Select`, `SelectById`, `Add`, `Delete`, `Update`, которые вызывааются в методах `AccountController`.  

#### SQL-запросы для создания BattleDB

```
create table account(
	id integer,
	email varchar(100),
	password varchar(100)
);

insert into account(id, email, password) values(1, 'firstUser@mail.ru', 'firstUser');
insert into account(id, email, password) values(2, 'secondUser@mail.ru', 'secondUser');
insert into account(id, email, password) values(3, 'thirdUser@mail.ru', 'thirdUser');
```

---

## Практика №10

#### Задание

1) Необходимо реализовать шаблонизатор в отдельном проекте библиотеки с синтаксисом `if`, `for`, `@имя переменной`.
Шаблонизатор должен принимать строку(шаблон) и объект. Данные с объекта должны подставляться в нужные места в шаблоне.

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_10/MyHttpServer "Проект")

> Для сервера установлен адрес - `localhost` и порт - `5120`. 
> Для проверки отправки сообщений необходимо в файле `appsettings.json` поменять значение у `FromEmail` на адрес своей почты.    
> Кнопки для регистрации на главной страницы подставляются в html при помощи шаблонизатора.   
> Шаблонизатор имеет класс `TemplateParser`, в котором реализованы методы для синтаксиса `for` и `@имя переменной`. Для хранения html самих кнопок был создан json-файл `buttons.json`, в котором хранятся кнопки для первого и второго ряда отдельно. Для получения всех кнопок сервером из json-файла была добавлена модель `Buttons`. С помощью `HtmlAgiityPack` происходит получение фрагмента из html, который необходимо обработать шаблонизатором перед отправкой страницы пользователю. Для этого был создан отдельный сервис `HtmlTemplateReader`, который после всех преобразований отправит страницу пользователю.

[SQL-запросы для создания BattleDB](#sql-запросы-для-создания-battledb)

---

## Дополнительное задание №1

#### Задание 

Необходимо с помощью `HtmlAgilityPack` получить 15 предметов с [торговой площадки Steam](https://steamcommunity.com/market/) и динамически сверстать новую страницу с ними.

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/steam_task/MyHttpServer "Проект")

> Чтобы получить новую страницу с предметами торговой площадки Steam, необходимо в браузере на странице `Battle.net` нажать на кнопку с иконкой Steam.   
> Для получения всех предметов и динамического генерирования новой страницы был добавлен новый контроллер - `OrdersController`.

---

## Дополнительное задание №2

#### Задание 

Необходимо создать базу данных для магазина игр, сверстать новую HTML-страницу с полями для использования фильтров поиска игр.

#### ComputerGames

- [Проект](https://github.com/2Jinx/www/tree/main/games_task/ComputerGames "Проект")

#### SQL-скрипты для создания базы данных `ComputerGames`

##### Таблица:

```
CREATE TABLE public.games (
    picture character varying(200),
    game_name character varying(100),
    genre character varying(100),
    release_date date,
    rating integer,
    available boolean,
    price integer
);
```

##### Заполнение данными:

```
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/the_witcher3.jpg', 'The Witcher 3: Wild Hunt', 'Action RPG', '2015-05-19', 95, true, 40);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/gta5.jpg', 'Grand Theft Auto V', 'Action-Adventure', '2013-09-17', 97, true, 60);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/zelda.png', 'The Legend of Zelda: Breath of the Wild', 'Action-Adventure', '2017-03-03', 97, true, 50);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/rdr2.jpg', 'Red Dead Redemption 2', 'Action-Adventure', '2018-10-26', 97, true, 60);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/dark_souls3.jpg', 'Dark Souls III', 'Action RPG', '2016-04-12', 89, true, 40);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/overwatch.jpg', 'Overwatch', 'First-Person Shooter', '2016-05-24', 90, true, 40);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/fortnite.jpg', 'Fortnite', 'Battle Royale', '2017-07-25', 85, true, 0);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/minecraft.png', 'Minecraft', 'Sandbox', '2011-11-18', 93, true, 30);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/fifa22.jpg', 'FIFA 22', 'Sports', '2021-10-01', 84, true, 60);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/ac_odyssey.jpeg', 'Assassins Creed Odyssey', 'Action-Adventure', '2018-10-05', 89, true, 50);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/cyberpunk2077.jpg', 'Cyberpunk 2077', 'Action RPG', '2020-12-10', 71, true, 50);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/among_us.jpg', 'Among Us', 'Social Deduction', '2018-06-15', 88, true, 5);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/hades.jpg', 'Hades', 'Action RPG', '2020-09-17', 93, true, 25);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/mario.jpg', 'Super Mario Odyssey', 'Platformer', '2017-10-27', 97, false, 60);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/skyrim.jpeg', 'The Elder Scrolls V: Skyrim', 'Action RPG', '2011-11-11', 94, true, 40);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/death_stranding.jpg', 'Death Stranding', 'Action', '2019-11-08', 86, true, 40);
INSERT INTO public.games (picture, game_name, genre, release_date, rating, available, price) VALUES ('images/valorant.jpg', 'Valorant', 'First-Person Shooter', '2020-06-02', 85, true, 0);
```

> Были добавлены: модель для игры - `Game.cs`, сервис для отображения списка игр, в котором динамически генерируется страница магазина - `StoreWraper.cs` и хэндлер - `StoreHandler.cs`. Также была сверстана главная страница магазина.

---
