������������ �����������:
- curl -X POST http://localhost:5000/register -H "Content-Type: application/json" -d "{\"username\": \"testuser\", \"password\": \"password123\"}"

����� �� ������ �� �������:
- curl -X POST http://localhost:5000/auth -H "Content-Type: application/json" -d "{\"username\": \"testuser\", \"password\": \"password123\"}"

������ ��������:
- curl -X POST http://localhost:5000/comment -H "Content-Type: application/json" -H "Authentication: (id �����������)" -d "{\"text\": \"This is a test comment.\"}"

�������� ������������:
- curl -X GET "http://localhost:5000/get?type=users"

�������� ��������:
- curl -X GET "http://localhost:5000/get?type=comments"