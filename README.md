# ModulHW1 2020 WebApi

#### Код для создания БД
    CREATE DATABASE MyWebApi.Dev;
#### Код для создания таблиц
    CREATE TABLE employees (
        "id" uuid NOT NULL,
        "username" text NOT NULL,
        "email" text NOT NULL,
        "passwordhash" text NOT NULL,
        "salt" text NOT NULL,
        PRIMARY KEY (id),
        UNIQUE(username, email)
    );
    CREATE TABLE accountnumbers (
        "number" text NOT NULL,
        "accountholderemail" text NOT NULL,
        "balance" decimal NOT NULL,
        PRIMARY KEY (number)
    ); 
#### Имя пользователя БД: postgres
#### Пароль БД: modulpass20
# Описание API
## Регистрация 
Registration [HttpPost("signup")]

Body:
- email - почтовый ящик пользователя
- username - имя пользователя
- password - пароль

Пример вызова:
 ```
POST https://localhost:5001/api/token/signup
Content-Type: application/json
{
    "email": "modul@gmail.com",
    "username": "modul",
    "password": "modulpass"
}
```
##### Ответ:
```json
{
    "access_token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb2R1bEBnbWFpbC5jb20iLCJqdGkiOiJjYzliYmI4OS0yOWFjLTQzNDctOGJmOC02NzFlZjNiNTViNzkiLCJuYmYiOjE1ODkyMDQzODcsImV4cCI6MTU4OTIwNDM4NywiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.jJBF-_GsiHJpxdw454JbMt_i6ZyftwkTkiBZjhflMhQ",
    "expiration": "2020-05-11T13:39:47Z"
}
```

## Авторизация 
Authorisation  [HttpPost("signin")]

Body:
- email - почтовый ящик пользователя
- password - пароль

Пример вызова:
```
POST https://localhost:5001/api/token/signin
Content-Type: application/json    
{
    "email": "modul@gmail.com",
    "password": "modulpass"
}
```
##### Ответ:
```json
{
    "access_token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb2R1bEBnbWFpbC5jb20iLCJqdGkiOiJhYzJjMzA5NC02MjlkLTQzYmYtODZjYS0zYmI5NDk1ZTllNjUiLCJuYmYiOjE1ODkyMDQ0NDEsImV4cCI6MTU4OTIwNDQ0MSwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.a2f7LX1Yjg8uYOOUgGKVhg7szRZwFAZlw02tCdKFkNI",
    "expiration": "2020-05-11T13:40:41Z"
}
```

## Открыть новый счет 
CreateNewAccountNumber [HttpPost("create")]

Пример вызова:
```
POST https://localhost:5001/api/transaction/create
Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb2R1bEBnbWFpbC5jb20iLCJqdGkiOiIzMjAwMmNkYS1kNmFhLTQxNzItOTBhMi1kOTg5YTFjMWIzYTYiLCJuYmYiOjE1ODkyMDQ3MzAsImV4cCI6MTU4OTIwNDczMCwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.lmq3qzrsNv5CuxJeoozPIXQaUglcFqVQVM9pJ61KsWQ
```
##### Ответ:
```json
{
    "number": "4980569749",
    "balance": 0,
    "accountHolderEmail": "modul@gmail.com"
}
```


## Пополнить счет 
TopUp [HttpPost("topup")]

Parameters:
- number - номер счета, который нужно пополнить
- amount - сумма пополнения 

Пример вызова:
```
POST https://localhost:5001/api/transaction/topup
Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb2R1bEBnbWFpbC5jb20iLCJqdGkiOiIzMjAwMmNkYS1kNmFhLTQxNzItOTBhMi1kOTg5YTFjMWIzYTYiLCJuYmYiOjE1ODkyMDQ3MzAsImV4cCI6MTU4OTIwNDczMCwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.lmq3qzrsNv5CuxJeoozPIXQaUglcFqVQVM9pJ61KsWQ
Content-Type: application/json    
{
	"number":"4980569749",
	"amount":2000
}
```
##### Ответ:
```json
{
    "number": "4980569749",
    "accountHolderEmail": "modul@gmail.com",
    "balance": 2000
}
```

## Выполнить перевод между счетами 
MakeTransaction [HttpPost("/accounts/makeTransaction")]

Parameters:
- ownaccount - номер счета отправителя
- payeeaccount - номер счета получателя
- amount - сумма перевода

Пример вызова:
```
POST https://localhost:5001/api/transaction/transfer
Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb2R1bEBnbWFpbC5jb20iLCJqdGkiOiJiNWVlNDVjYS1mMTQ2LTRhNzUtYTFlOC04YTkwOTE0OGJhY2MiLCJuYmYiOjE1ODkyMDUwOTYsImV4cCI6MTU4OTIwNTA5NiwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.u2vvl1DryzXM8MxnOJbQijFKUoftaPT_xaKENmhHTFQ
Content-Type: application/json    
{
    "ownaccount": "4980569749",
    "payeeaccount": "4541374053",
    "amount": 1400
} 
```
##### Ответ:
```json
{
    "number": "4980569749",
    "accountHolderEmail": "modul@gmail.com",
    "balance": 600
}
```

