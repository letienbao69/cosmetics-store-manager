-- Database demo cho bài tập lớn nhóm
-- Lưu ý: script này reset lại dữ liệu mẫu để chạy demo từ đầu.

IF DB_ID(N'CosmeticsStoreDB') IS NULL
BEGIN
    CREATE DATABASE CosmeticsStoreDB;
END
GO

USE CosmeticsStoreDB;
GO

IF OBJECT_ID(N'OrderDetails', N'U') IS NOT NULL DROP TABLE OrderDetails;
IF OBJECT_ID(N'Orders', N'U') IS NOT NULL DROP TABLE Orders;
IF OBJECT_ID(N'Products', N'U') IS NOT NULL DROP TABLE Products;
IF OBJECT_ID(N'Customers', N'U') IS NOT NULL DROP TABLE Customers;
IF OBJECT_ID(N'Users', N'U') IS NOT NULL DROP TABLE Users;
GO

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Staff'))
);

CREATE TABLE Customers
(
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(255) NULL,
    Email NVARCHAR(100) NULL
);

CREATE TABLE Products
(
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductCode NVARCHAR(30) NOT NULL UNIQUE,
    ProductName NVARCHAR(150) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Brand NVARCHAR(100) NOT NULL,
    ImportPrice DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (ImportPrice >= 0),
    SalePrice DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (SalePrice >= 0),
    QuantityInStock INT NOT NULL DEFAULT 0 CHECK (QuantityInStock >= 0),
    ExpiryDate DATE NULL,
    Description NVARCHAR(500) NULL
);

CREATE TABLE Orders
(
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerId INT NOT NULL,
    UserId INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (TotalAmount >= 0),
    Status NVARCHAR(30) NOT NULL DEFAULT N'Completed',
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE OrderDetails
(
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice >= 0),
    LineTotal DECIMAL(18,2) NOT NULL CHECK (LineTotal >= 0),
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);
GO

INSERT INTO Users(Username, PasswordHash, Role)
VALUES ('admin', '123456', 'Admin'),
       ('staff', '123456', 'Staff');

INSERT INTO Customers(FullName, Phone, Address, Email)
VALUES (N'Nguyễn Thu Hà', '0901234567', N'Hà Nội', 'ha@gmail.com'),
       (N'Trần Mỹ Linh', '0912345678', N'Hải Phòng', 'linh@gmail.com'),
       (N'Lê Ngọc Anh', '0987654321', N'Đà Nẵng', 'ngocanh@gmail.com');

INSERT INTO Products(ProductCode, ProductName, Category, Brand, ImportPrice, SalePrice, QuantityInStock, ExpiryDate, Description)
VALUES ('MP001', N'Son lì Black Rouge', N'Trang điểm', 'Black Rouge', 120000, 180000, 50, '2027-12-31', N'Son lì bán chạy'),
       ('MP002', N'Kem chống nắng Anessa', N'Chăm sóc da', 'Anessa', 280000, 350000, 20, '2027-10-15', N'Chống nắng phổ rộng'),
       ('MP003', N'Sữa rửa mặt Cetaphil', N'Chăm sóc da', 'Cetaphil', 190000, 250000, 15, '2027-08-20', N'Dịu nhẹ cho da nhạy cảm'),
       ('MP004', N'Nước tẩy trang Bioderma', N'Chăm sóc da', 'Bioderma', 250000, 320000, 8, '2027-09-10', N'Làm sạch dịu nhẹ'),
       ('MP005', N'Phấn nước Missha', N'Trang điểm', 'Missha', 180000, 260000, 12, '2027-06-25', N'Che phủ tự nhiên');
GO
