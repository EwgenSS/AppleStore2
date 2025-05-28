-- AppleStore Database Schema
-- Создание базы данных для проекта AppleStore

-- Таблица категорий товаров
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

-- Таблица товаров
CREATE TABLE Products (
    ProductId INT PRIMARY KEY,
    CategoryId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10, 2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- Таблица деталей товаров
CREATE TABLE ProductDetails (
    ProductId INT PRIMARY KEY,
    Color NVARCHAR(50),
    ScreenSize NVARCHAR(50),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Таблица клиентов
CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(200)
);

-- Таблица сотрудников
CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20)
);

-- Таблица заказов
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY,
    CustomerId INT NOT NULL,
    EmployeeId INT,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL DEFAULT 0,
    Status NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
);

-- Таблица элементов заказа
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Примеры данных для таблицы Categories
INSERT INTO Categories (CategoryId, Name, Description) VALUES
(1, 'iPhone', 'Смартфоны Apple iPhone различных моделей'),
(2, 'iPad', 'Планшеты Apple iPad различных моделей'),
(3, 'Mac', 'Компьютеры и ноутбуки Apple Mac'),
(4, 'Watch', 'Умные часы Apple Watch'),
(5, 'AirPods', 'Беспроводные наушники Apple');

-- Примеры данных для таблицы Products
INSERT INTO Products (ProductId, CategoryId, Name, Description, Price, StockQuantity) VALUES
(1, 1, 'iPhone 13 Pro', '6.1-дюймовый дисплей, A15 Bionic, 128GB', 999.99, 50),
(2, 1, 'iPhone 13', '6.1-дюймовый дисплей, A15 Bionic, 128GB', 799.99, 75),
(3, 2, 'iPad Pro 12.9', '12.9-дюймовый дисплей, M1 чип, 256GB', 1099.99, 30),
(4, 2, 'iPad Air', '10.9-дюймовый дисплей, M1 чип, 64GB', 599.99, 45),
(5, 3, 'MacBook Pro 14', '14-дюймовый дисплей, M1 Pro, 512GB SSD', 1999.99, 25),
(6, 3, 'iMac 24', '24-дюймовый дисплей, M1, 256GB SSD', 1299.99, 20),
(7, 4, 'Apple Watch Series 7', '41mm, алюминиевый корпус', 399.99, 60),
(8, 5, 'AirPods Pro', 'Активное шумоподавление, адаптивный эквалайзер', 249.99, 100);

-- Примеры данных для таблицы ProductDetails
INSERT INTO ProductDetails (ProductId, Color, ScreenSize) VALUES
(1, 'Графитовый', '6.1 дюйма'),
(2, 'Синий', '6.1 дюйма'),
(3, 'Серебристый', '12.9 дюйма'),
(4, 'Голубой', '10.9 дюйма'),
(5, 'Серый космос', '14 дюймов'),
(6, 'Серебристый', '24 дюйма'),
(7, 'Красный', '41 мм'),
(8, 'Белый', 'Не применимо');

-- Примеры данных для таблицы Customers
INSERT INTO Customers (CustomerId, FullName, Email, Phone, Address) VALUES
(1, 'Иванов Иван', 'ivanov@example.com', '+7 (900) 123-45-67', 'г. Москва, ул. Ленина, 10'),
(2, 'Петрова Анна', 'petrova@example.com', '+7 (900) 234-56-78', 'г. Санкт-Петербург, пр. Невский, 20'),
(3, 'Сидоров Алексей', 'sidorov@example.com', '+7 (900) 345-67-89', 'г. Казань, ул. Баумана, 30');

-- Примеры данных для таблицы Employees
INSERT INTO Employees (EmployeeId, FullName, Position, Email, Phone) VALUES
(1, 'Смирнов Дмитрий', 'Менеджер', 'smirnov@applestore.com', '+7 (900) 111-22-33'),
(2, 'Козлова Елена', 'Консультант', 'kozlova@applestore.com', '+7 (900) 222-33-44'),
(3, 'Морозов Сергей', 'Техник', 'morozov@applestore.com', '+7 (900) 333-44-55');

-- Примеры данных для таблицы Orders
INSERT INTO Orders (OrderId, CustomerId, EmployeeId, OrderDate, TotalAmount, Status) VALUES
(1, 1, 1, '2023-01-15 10:30:00', 999.99, 'Выполнен'),
(2, 2, 2, '2023-01-20 14:45:00', 1699.98, 'Выполнен'),
(3, 3, 1, '2023-01-25 16:20:00', 649.98, 'В обработке');

-- Примеры данных для таблицы OrderItems
INSERT INTO OrderItems (OrderItemId, OrderId, ProductId, Quantity, UnitPrice) VALUES
(1, 1, 1, 1, 999.99),
(2, 2, 2, 1, 799.99),
(3, 2, 7, 1, 399.99),
(4, 2, 8, 2, 249.99),
(5, 3, 8, 1, 249.99),
(6, 3, 4, 1, 599.99);
