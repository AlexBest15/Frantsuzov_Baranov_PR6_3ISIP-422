using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Registration;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]

        public void AuthTestSuccess()
        {
            var page = new LoginWindow();
            //Авторизация
            //1: Ввод некорректного логина
            Assert.IsFalse(page.Auth("12345", "Alex"));
            //2: Ввод некорректного пароля
            Assert.IsFalse(page.Auth("Kostya", "12345"));
            //3: Пустые поля авторизации
            Assert.IsFalse(page.Auth("", ""));
            //4: Чувствительность к регистру
            Assert.IsFalse(page.Auth("koStya", "AlEx"));
            //5: Превышение максимальной длины
            Assert.IsFalse(page.Auth("k5F8j2L9p3R7x1Y4v6Q0w9T2z5N8b3M7s4K1d6H0g2V4f9J8r3X6", "Alex"));
            //6: Отображение ошибки
            Assert.IsFalse(page.Auth("incorrect@example.com", "incorrect"));
            //Регистрация
            var page2 = new MainWindow();
            //7: Пустые обязательные поля регистрации
            Assert.IsFalse(page2.Auth2("", "","",""));
            //8: Ввод некорректных данных в форме регистрации
            Assert.IsFalse(page2.Auth2("!@#", "newuser", "123", "123"));
            //9: Проверка формата email
            Assert.IsFalse(page2.Auth2("newuser", "newuser", "Qwerty!123", "Qwerty123!"));
            //10: Несовпадение Пароля и Подтверждения пароля
            Assert.IsFalse(page2.Auth2("newuser", "newuser@example.com", "Qwerty123!", "Qwerty123"));
        }
    }
}
