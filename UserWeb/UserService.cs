using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWeb
{
    public class UserService
    {
        private ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>();

        public UserService()
        {
        }

        private int GetNewId()
        {
            if (users.IsEmpty)
                return 1;
            return users.Select(x => x.Key).Max() + 1;
        }

        public User Add(User user)
        {
            ValidationService.ValidateUser(user);
            user.Id = GetNewId();
            users.GetOrAdd(user.Id, user);
            return user;
        }

        public void Remove(int id)
        {
            var userToDelete = users.GetValueOrDefault(id);
            if (userToDelete == null)
                throw new UserValidationException($"Не существует пользователя с id = {id}");
            users.Remove(id, out _);
        }
        
        public void Update(int id, User user)
        {
            var userToUpdate = users.GetValueOrDefault(id);
            if (userToUpdate == null)
                throw new UserValidationException($"Не существует пользователя с id = {id}");

            ValidationService.ValidateUser(user);
            user.Id = id;

            users.AddOrUpdate(id, user, (s, source) => user);
        }

        public List<User> GetAll()
        {
            return users.Select(x => x.Value).ToList();
        }
    }
}
