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

CREATE TABLE customers(C_id INT AUTO_INCREMENT PRIMARY KEY, User VARCHAR(15) NOT NULL UNIQUE);
INSERT INTO customers VALUES(0, 'Jerry');
INSERT INTO customers VALUES(0, 'Willson');
INSERT INTO customers VALUES(0, 'Bob');
CREATE TABLE Passwords (C_id INT, Password VARCHAR(20) NOT NULL, FOREIGN KEY(C_id) REFERENCES customers(C_id), PRIMARY KEY(C_id));
INSERT INTO Passwords VALUES(5, 'qwe123');
INSERT INTO Passwords VALUES(1, 'zxc555');
INSERT INTO Passwords VALUES(3, 'asd123');
INSERT INTO Passwords VALUES((SELECT C_id FROM customers WHERE User = 'Willson'), 'qwe123');
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
SELECT * FROM customers JOIN Passwords WHERE customers.C_id = Passwords.C_id;
SELECT User FROM customers WHERE User = 'willson';
SELECT Password FROM passwords WHERE C_id = (SELECT C_id FROM customers WHERE User = 'willson');
SELECT orders.O_id, customers.User, stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;
SELECT customers.User, stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;
SELECT stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id WHERE User = 'willson';
SELECT stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id WHERE User = 'Jerry';
SELECT stocks.S_id FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id WHERE User = 'Jerry';
SELECT stocks.Date FROM orders JOIN stocks ON stocks.S_id = 2330 WHERE orders.S_id = 2330;
SELECT * FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;
SELECT S_id FROM stocks WHERE S_id = '2330';
SELECT S_id FROM stocks;
SELECT * FROM orders;
SELECT * FROM Passwords;
SELECT * FROM stocks;
SELECT * FROM customers;
DELETE FROM stocks WHERE S_id='2330';
DELETE FROM customers WHERE C_id='1';
DROP TABLE passwords;
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

CREATE DATABASE MemberSys;
USE MemberSys;
SHOW TABLES;
CREATE TABLE IF NOT EXISTS `MemberSys`.`Usertable` (
  `User_ID` INT NOT NULL auto_increment,
  `Username` VARCHAR(45) NOT NULL UNIQUE,
  `Password` VARCHAR(45) NOT NULL,
  `Realname` VARCHAR(45) NOT NULL,
  `Birthday` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  `Buildday` VARCHAR(45) NOT NULL,
  `Authority` INT NOT NULL,
  PRIMARY KEY (`User_ID`))
ENGINE = InnoDB;
DESCRIBE Usertable;
DROP TABLE Usertable;
INSERT INTO MemberSys.Usertable VALUES(0, 'root', '1234', 'god', '0/1/1', 'god@gmail.com', '2021/2/21', '3');
INSERT INTO MemberSys.Usertable VALUES(0, 'tar', '1234', 'god', '0/1/1', 'god@gmail.com', '2021/2/21', '2');
SELECT * FROM MemberSys.Usertable;
SELECT Username, Password FROM MemberSys.Usertable WHERE Username = 'root';
SELECT User_ID FROM MemberSys.Usertable WHERE Username = 'root';
SELECT Username FROM MemberSys.Usertable WHERE User_ID = 1;
SELECT Username, Authority FROM MemberSys.Usertable;
UPDATE MemberSys.Usertable SET Realname= 'good', Birthday= '1/1/1' , Email= 'good@mail.com' WHERE Username='root';
UPDATE MemberSys.Usertable SET Password= '1245' WHERE Username='root';
UPDATE MemberSys.Usertable SET Authority= 3 WHERE Username='root';
UPDATE MemberSys.Usertable SET Authority= '2' WHERE Username='1';