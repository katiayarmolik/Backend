using System;
using System.Collections.Generic;
using System.Linq;
using BlogApi.Entities;

namespace BlogApi.Data
{
    public static class DataWrapper
    {
        public static List<User> Users = new List<User>();
        public static List<Comment> Comments = new List<Comment>();

        public static User? FindUserByUsername(string username) =>
            Users.FirstOrDefault(u => u.Username == username);

        public static User? FindUserById(Guid userId) =>
            Users.FirstOrDefault(u => u.Id == userId);
    }
}
