using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserWeb
{
    public static class ValidationService
    {
        const string emailPattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        const string fioPattern = @"^[а-яА-ЯеЁ]+$";

        private static bool IsEmptyString(this string str)
        {
            if (str == null || str.Length == 0)
                return true;
            return false;
        }

        public static void ValidateUser(User user)
        {
            if (user.Name.IsEmptyString())
                throw new UserValidationException("Не заполнено поле Имя");
            if (user.SecondName.IsEmptyString())
                throw new UserValidationException("Не заполнено поле Фамилия");
            if (user.Email.IsEmptyString())
                throw new UserValidationException("Не заполнено поле Почта");

            if (!Regex.Match(user.Name, fioPattern).Success)
                throw new UserValidationException("Неправильно указано поле Имя"); 
            
            if (!Regex.Match(user.SecondName, fioPattern).Success)
                throw new UserValidationException("Неправильно указано поле Фамилия");

            if (!Regex.Match(user.Email, emailPattern).Success)
                throw new UserValidationException("Неправильно указано поле Почта");

            if (user.Age < 14 || user.Age > 150)
                throw new UserValidationException("Неправильно указан возраст пользователя. Он может быть от 14 до 150");
        }
    }
}
