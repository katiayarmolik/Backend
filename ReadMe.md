Зареєструвати користувача:
- curl -X POST http://localhost:5000/register -H "Content-Type: application/json" -d "{\"username\": \"testuser\", \"password\": \"password123\"}"

Увійти за логіном та паролем:
- curl -X POST http://localhost:5000/auth -H "Content-Type: application/json" -d "{\"username\": \"testuser\", \"password\": \"password123\"}"

Додати коментар:
- curl -X POST http://localhost:5000/comment -H "Content-Type: application/json" -H "Authentication: (id користувача)" -d "{\"text\": \"This is a test comment.\"}"

Отримати користувачів:
- curl -X GET "http://localhost:5000/get?type=users"

Отримати коментарі:
- curl -X GET "http://localhost:5000/get?type=comments"