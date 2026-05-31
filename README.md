# Karp5Shop

Karp5Shop - клиент-серверное приложение для управления товарами интернет-магазина.

Проект реализован на C# по ТЗ практического занятия:

- `Karp5Shop.Client` - Blazor WebAssembly интерфейс.
- `Karp5Shop.Server` - ASP.NET Core host и Web API.
- `Karp5Shop.Shared` - общая DTO-модель товара.
- Entity Framework Core + SQLite используются для хранения данных.
- Поддержаны светлая и темная темы оформления.

## Функции

- просмотр списка товаров;
- добавление товара;
- изменение товара;
- удаление товара с подтверждением;
- хранение данных в файле SQLite `products.db`;
- API:
  - `GET /api/products`
  - `POST /api/products`
  - `PUT /api/products/{id}`
  - `PUT /api/products`
  - `DELETE /api/products/{id}`

## Требования

- .NET SDK 8.0 или новее.

Проверить установленный SDK:

```powershell
dotnet --info
```

## Как запустить

Из папки проекта:

```powershell
dotnet restore
dotnet run --project Karp5Shop.Server
```

После запуска откройте в браузере:

```text
http://localhost:5183
```

Swagger для проверки API доступен по адресу:

```text
http://localhost:5183/swagger
```

При первом запуске сервер автоматически создаст SQLite-базу `Karp5Shop.Server/products.db` и добавит 3 тестовых товара.

## Как проверить работоспособность

### 1. Проверка сборки

```powershell
dotnet build Karp5Shop.sln
```

Ожидаемый результат: `Сборка успешно завершена`, ошибок нет.

### 2. Проверка загрузки списка товаров

1. Запустите сервер командой `dotnet run --project Karp5Shop.Server`.
2. Откройте `http://localhost:5183`.
3. Убедитесь, что таблица содержит стартовые товары: ноутбук, смартфон и наушники.

Результат: товары отображаются в таблице, значит `GET /api/products` работает.

### 3. Проверка добавления товара

1. В форме справа введите:
   - название: `Игровая мышь`
   - описание: `USB, 6 кнопок`
   - цена: `2490`
   - количество: `12`
2. Нажмите `Добавить`.

Результат: товар `Игровая мышь` появится в таблице. После обновления страницы товар должен остаться в списке, потому что он сохранен в БД.

### 4. Проверка изменения товара

1. В строке товара `Игровая мышь` нажмите `Изменить`.
2. Измените цену на `2990`, количество на `7`.
3. Нажмите `Сохранить`.

Результат: в таблице для товара отобразятся новая цена `2 990,00 ₽` и остаток `7`.

### 5. Проверка удаления товара

1. В строке товара `Игровая мышь` нажмите `Удалить`.
2. В окне подтверждения нажмите `OK`.

Результат: товар исчезнет из таблицы.

Дополнительно проверьте отмену удаления: создайте товар снова, нажмите `Удалить`, затем в подтверждении нажмите `Отмена`. Товар должен остаться в таблице.

### 6. Проверка валидации формы

1. Очистите поле `Название`.
2. В поле `Цена` введите `0`.
3. Нажмите `Добавить`.

Результат: форма покажет ошибки валидации, запрос на сервер не будет выполнен.

### 7. Проверка переключения темы

1. Нажмите кнопку `Темная тема` в правом верхнем углу.
2. Убедитесь, что фон и таблица переключились на темное оформление.
3. Обновите страницу.

Результат: выбранная тема сохранится после обновления страницы.

### 8. Проверка API через PowerShell

Сначала запустите сервер, затем выполните:

```powershell
$base = 'http://localhost:5183/api/products'

Invoke-RestMethod -Uri $base -Method Get

$created = Invoke-RestMethod -Uri $base -Method Post -ContentType 'application/json; charset=utf-8' -Body (@{
    name = 'Тестовый товар'
    description = 'Проверка POST'
    price = 1234.56
    stock = 9
} | ConvertTo-Json)

Invoke-RestMethod -Uri "$base/$($created.id)" -Method Put -ContentType 'application/json; charset=utf-8' -Body (@{
    id = $created.id
    name = 'Тестовый товар изменен'
    description = 'Проверка PUT'
    price = 1500
    stock = 4
} | ConvertTo-Json)

Invoke-RestMethod -Uri "$base/$($created.id)" -Method Delete
```

Результат: товар создается, изменяется и удаляется без ошибок HTTP.

## Структура проекта

```text
Karp5Shop.sln
Karp5Shop.Client/
  Pages/Home.razor
  Layout/MainLayout.razor
  wwwroot/css/app.css
Karp5Shop.Server/
  Controllers/ProductsController.cs
  Data/AppDbContext.cs
  Models/Product.cs
Karp5Shop.Shared/
  ProductDto.cs
```

## Основные компоненты и файлы

`Karp5Shop.sln` - файл решения, объединяет клиентский, серверный и общий проекты.

`Karp5Shop.Shared/ProductDto.cs` - общая модель товара, которая используется и клиентом, и сервером. В ней описаны поля товара: `Id`, `Name`, `Description`, `Price`, `Stock`, а также правила валидации формы.

`Karp5Shop.Server/Program.cs` - точка входа серверного приложения. Здесь подключаются контроллеры, Entity Framework Core, SQLite, Swagger, статические файлы Blazor WebAssembly и fallback на `index.html`.

`Karp5Shop.Server/Data/AppDbContext.cs` - контекст Entity Framework Core. Описывает таблицу `Products`, настройки поля цены и стартовые тестовые товары, которые появляются при первом запуске.

`Karp5Shop.Server/Models/Product.cs` - серверная сущность товара, которая хранится в базе данных SQLite.

`Karp5Shop.Server/Controllers/ProductsController.cs` - API-контроллер товаров. Реализует получение списка, добавление, изменение и удаление товаров через маршруты `/api/products`.

`Karp5Shop.Server/appsettings.json` - настройки серверного приложения, включая строку подключения к SQLite-базе `products.db`.

`Karp5Shop.Client/Program.cs` - точка входа Blazor WebAssembly клиента. Создает приложение и настраивает `HttpClient` для запросов к API.

`Karp5Shop.Client/App.razor` - корневой компонент Blazor, который подключает маршрутизацию страниц приложения.

`Karp5Shop.Client/Layout/MainLayout.razor` - общий макет интерфейса. Содержит верхнюю панель, название приложения и кнопку переключения светлой/темной темы.

`Karp5Shop.Client/Pages/Home.razor` - главная страница приложения. Здесь находится таблица товаров, форма добавления и редактирования, логика загрузки данных из API, сохранения и удаления товаров.

`Karp5Shop.Client/wwwroot/index.html` - HTML-оболочка Blazor WebAssembly. Здесь подключаются стили, скрипт Blazor и JavaScript-функции для темы и подтверждения удаления.

`Karp5Shop.Client/wwwroot/css/app.css` - основные стили интерфейса, включая светлую и темную тему, таблицу товаров, форму и адаптивную верстку.

`Karp5Shop.Client/wwwroot/css/bootstrap/bootstrap.min.css` - Bootstrap, используется для базовых кнопок, таблиц, форм и уведомлений.
