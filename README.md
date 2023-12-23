
## Содержание

- [Практика №1](#d0b7d0b0d0b4d0b0d0bdd0b8d0b5-1)
- [Практика №2](#d0b7d0b0d0b4d0b0d0bdd0b8d0b5-2)
- [Практика №3](#d0b7d0b0d0b4d0b0d0bdd0b8d0b5-3)
- [Практика №4](#d0b7d0b0d0b4d0b0d0bdd0b8d0b5-4)
- [Практика №5](#d0b7d0b0d0b4d0b0d0bdd0b8d0b5-5)
- [Практика №6](#d0b7d0b0d0b4d0b0d0bdd0b8d0b5-6)

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

> В `appsettings.json` порт установлен `5120`.

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

---

## Практика №5

#### Задание

1) Вынести в отдельные файлы стили из файла `index.html`(Battle.net).  HTML файл у `Batlle.net` должен содержать папки `images`, `styles` и соответствующие файлы. Все картинки должны лежать в папке `images`

2) Перенести содержимое сайта `Battle.net` в папку `static`

3) Реализовать открытие сервером по пути `localhost:8082` страницы `index.html` с подгрузкой всех статических файлов.    
Например: при переходе по пути `localhost:8082/static/styles/all.css` должен подгружать файл `all.css` который хранится в каталоге `static`. (`Battle.net` должен отображаться корректно без подклчения к интернету)

#### MyHttpServer

- [Проект](https://github.com/2Jinx/www/tree/main/homework_5/MyHttpServer "Проект") 

> Для сервера установлен адрес - `localhost` и порт - `5120`

---

## Практика №6 

#### Задание

Необходимо реализовать отправку сообщения  на почту с логином и паролем, введеным на странице `Battle.net` на вашу почту с помощью `HttpServer`
 
1) Добавить в проект папку `Services`

2) В папке `Services` создать интерфейс `IEmailSenederServis` и класс `EmailsSenderService`

3) Добавить параметры в `appsettings.json`, а именно `EmailSender`, `PasswordSender`, `FromName`, `HttpServerHost/Port`

4) Произвести рефакторинг кода. С помощью паттерна цепочки обязанностей вынести отдельно логику загрузки статический файлов в `StaticFilesHandler`

- [Проект](https://github.com/2Jinx/www/tree/main/homework_6/MyHttpServer "Проект") 

---
