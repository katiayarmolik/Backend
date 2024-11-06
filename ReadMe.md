Register User
- curl -X POST http://localhost:5000/register -H "Content-Type: application/json" -d "{\"username\": \"testuser\", \"password\": \"password123\"}"

Auth User
- curl -X POST http://localhost:5000/auth -H "Content-Type: application/json" -d "{\"username\": \"testuser\", \"password\": \"password123\"}"

Add comment
- curl -X POST http://localhost:5000/comment -H "Content-Type: application/json" -H "Authentication: (id користувача)" -d "{\"text\": \"This is a test comment.\"}"

Get users
- curl -X GET "http://localhost:5000/get?type=users"

Get comments
- curl -X GET "http://localhost:5000/get?type=comments"