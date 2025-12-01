# Weather Service

Бэкенд-сервис для автоматизированного опроса и хранения данных о погоде по городам.

## Описание

Сервис позволяет:
- Добавлять города с настраиваемой частотой опроса погоды (5-180 минут)
- Автоматически собирать данные о погоде в фоновом режиме
- Получать историю погодных данных за период
- Управлять настройками опроса для каждого города

**Технологии:** .NET 8, EF Core, MediatR, SQL Server, Clean Architecture

## Быстрый старт

### 1. Создать и применить миграции БД

```bash
dotnet ef database update --project Weather.Infrastructure --startup-project Weather.Api
```

### 2. Запустить приложение

```bash
cd Weather.Api
dotnet run
```

Приложение запустится на `https://localhost:5001`

**Swagger UI:** `https://localhost:5001/swagger`

Background service автоматически опрашивает погоду каждые 30 секунд для активных городов.

## API

**Управление городами:**
- `GET /api/cities` - список городов
- `POST /api/cities` - добавить город
- `PUT /api/cities/{id}` - обновить настройки
- `DELETE /api/cities/{id}` - удалить/деактивировать

**История погоды:**
- `GET /api/weather/{cityId}/history?from={date}&to={date}` - получить данные за период

### Пример

```bash
# Добавить город
curl -X POST https://localhost:5001/api/cities \
  -H "Content-Type: application/json" \
  -d '{"cityName": "Moscow", "pollingIntervalMinutes": 30}'

# Получить список
curl https://localhost:5001/api/cities
```

## Тесты

```bash
dotnet test
```

## Архитектура

- **Weather.Domain** - доменные сущности и бизнес-правила
- **Weather.Application** - CQRS (MediatR)
- **Weather.Infrastructure** - EF Core, репозитории
- **Weather.Api** - REST API + Background Service
- **Weather.Tests** - юнит-тесты
