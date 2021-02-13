#創建資料庫
CREATE DATABASE test1;
#進入資料庫
USE test1;
#顯示全部資料表名稱
SHOW TABLES;
#創建資料表
CREATE TABLE mytable(school CHAR(5), name VARCHAR(10), id INT);
CREATE TABLE mytable2(school CHAR(5) PRIMARY KEY, name VARCHAR(10) UNIQUE, id INT);
SHOW TABLES;
#查閱資料表屬性
DESCRIBE mytable;
#列出資料表
SELECT * FROM mytable;
#輸入資料
INSERT INTO mytable(school, name, id) VALUES ('NCTU', 'Jerry', '123');
INSERT INTO mytable VALUES('NCTU', 'Jerry', '123');
SET SQL_SAFE_UPDATES=0;
#更新資料
UPDATE mytable SET name='HaHa' WHERE id='123';
#刪除資料
DELETE FROM mytable WHERE name='HaHa';
SET GLOBAL local_infile=1;
LOAD DATA LOCAL INFILE "c:\\C:\Users\willson\Desktop\Software Power\stock_project__\SQL\data.txt" INTO TABLE mytable;
INSERT INTO mytable VALUES('NCTU', 'Jerry', '123');
INSERT INTO mytable VALUES('NTHU', 'Tom', '123');
INSERT INTO mytable VALUES('NTU', 'Spike', '123');
INSERT INTO mytable2 VALUES(3 , NULL , '123');
SELECT * FROM mytable2 WHERE id='123' ORDER BY name DESC;
DROP TABLE mytable;
ALTER TABLE mytable ADD COLUMN recordtime TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;

CREATE DATABASE StockAPP;
USE StockAPP;
SHOW TABLES;
CREATE TABLE customers(C_id INT AUTO_INCREMENT PRIMARY KEY, User VARCHAR(15) NOT NULL);
INSERT INTO customers VALUES(0, 'Jerry');
INSERT INTO customers VALUES(0, 'Willson');
INSERT INTO customers VALUES(0, 'Bob');
CREATE TABLE Passwords (C_id INT, Password VARCHAR(20) NOT NULL, FOREIGN KEY(C_id) REFERENCES customers(C_id), PRIMARY KEY(C_id));
INSERT INTO Passwords VALUES(1, 'qwe123');
INSERT INTO Passwords VALUES(2, 'zxc555');
INSERT INTO Passwords VALUES(3, 'asd123');
CREATE TABLE stocks(S_id INT PRIMARY KEY, S_name CHAR(10) NOT NULL, Open_price VARCHAR(10), End_price VARCHAR(10), High_price VARCHAR(10), Low_price VARCHAR(10), Rang VARCHAR(10), Vol VARCHAR(20), Vol_unit VARCHAR(20), Vol_price VARCHAR(30), Date VARCHAR(10));
INSERT INTO stocks(S_id, S_name, End_price) VALUES(2330, 'TSMC', 200.15);
INSERT INTO stocks(S_id, S_name, End_price) VALUES(2634, '漢翔', 28.5);
INSERT INTO stocks(S_id, S_name, End_price) VALUES(2412, '中華電', 108.5);
CREATE TABLE orders (O_id INT NOT NULL AUTO_INCREMENT, C_id INT, S_id INT, PRIMARY KEY (O_id), FOREIGN KEY (C_id) REFERENCES customers(C_id), FOREIGN KEY (S_id) REFERENCES stocks(S_id));
CREATE TABLE orders (C_id INT, S_id INT, FOREIGN KEY(C_id) REFERENCES customers(C_id), FOREIGN KEY(S_id) REFERENCES stocks(S_id), PRIMARY KEY(C_id, S_id));
INSERT INTO orders(C_id, S_id) VALUES(1, 2330);
INSERT INTO orders(C_id, S_id) VALUES(1, 2634);
INSERT INTO orders(C_id, S_id) VALUES(1, 2412);
UPDATE stocks SET Date='107/02/05' WHERE S_id='2330';
UPDATE stocks SET Open_price= 123, End_price= '321' , High_price= '1234567', Low_price='123', Rang= '321', Vol= '222' , Vol_unit= '123', Vol_price='321', Date='1234567' WHERE S_id='2330';
SELECT customers.User, Passwords.Password FROM customers JOIN Passwords WHERE customers.C_id = Passwords.C_id;
SELECT orders.O_id, customers.User, stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;
SELECT customers.User, stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;
SELECT stocks.Date FROM orders JOIN stocks ON stocks.S_id = 2330 WHERE orders.S_id = 2330;
SELECT * FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;
SELECT * FROM orders;
SELECT * FROM Passwords;
SELECT * FROM stocks;
SELECT * FROM customers;
DELETE FROM stocks WHERE S_id='2330';
DELETE FROM customers WHERE C_id='1';
DROP TABLE stocks;
DROP TABLE orders;
DROP TABLE customers;
DESCRIBE stocks;
CREATE TABLE users(
    userID INT(50) PRIMARY KEY,
    userName VARCHAR(30)
);

CREATE TABLE orderss(
    orderID INT(50) PRIMARY KEY,
    userID INT(50),
    product VARCHAR(100),
    price INT(11),
    FOREIGN KEY(userID) REFERENCES users(userID)
);
INSERT INTO users VALUES(1, 'Blueberry');
INSERT INTO orderss VALUES(1, 1, 'phone', 20000);
SELECT * FROM orderss;
