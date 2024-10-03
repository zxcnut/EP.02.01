using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using TravelPro;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private Form1 _form;

        [TestInitialize]
        public void Initialize()
        {
            _form = new Form1();
        }

        [TestMethod]
        public void ValidateLoginInput_ValidInput_ReturnsTrue()
        {
            // Arrange
            string login = "testlogin";
            string password = "testpassword";

            // Act
            bool result = _form.ValidateLoginInput(login, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateLoginInput_EmptyLogin_ReturnsFalse()
        {
            // Arrange
            string login = "";
            string password = "testpassword";

            // Act
            bool result = _form.ValidateLoginInput(login, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateLoginInput_EmptyPassword_ReturnsFalse()
        {
            // Arrange
            string login = "testlogin";
            string password = "";

            // Act
            bool result = _form.ValidateLoginInput(login, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateLoginInput_NullLogin_ReturnsFalse()
        {
            // Arrange
            string login = null;
            string password = "testpassword";

            // Act
            bool result = _form.ValidateLoginInput(login, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateLoginInput_NullPassword_ReturnsFalse()
        {
            // Arrange
            string login = "testlogin";
            string password = null;

            // Act
            bool result = _form.ValidateLoginInput(login, password);

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void AuthenticateUser_ValidCredentials_ReturnsUserRole()
        {
            // Arrange
            string login = "testlogin";
            string password = "testpassword"; // Ensure these credentials exist in your test database

            // Act
            string userRole = _form.AuthenticateUser(login, password);

            // Assert
            Assert.IsNotNull(userRole);
        }

        [TestMethod]
        public void AuthenticateUser_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            string login = "invalidlogin";
            string password = "invalidpassword";

            // Act
            string userRole = _form.AuthenticateUser(login, password);

            // Assert
            Assert.IsNull(userRole);
        }

        [TestMethod]
        //[ExpectedException(typeof(SqlException))]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AuthenticateUser_DatabaseConnectionFailure_ThrowsException()
        {
            // Arrange
            // Simulate a connection failure by using an invalid connection string
            _form.connectionString = "Invalid Connection String";

            string login = "testlogin";
            string password = "testpassword";

            // Act
            _form.AuthenticateUser(login, password);
        }

        [TestMethod]
        public void AuthenticateUser_EmptyCredentials_ReturnsNull()
        {
            // Arrange
            string login = "";
            string password = "";

            // Act
            string userRole = _form.AuthenticateUser(login, password);

            // Assert
            Assert.IsNull(userRole);
        }

        [TestMethod]
        public void AuthenticateUser_ValidCredentials_ReturnsCorrectUserRole()
        {
            // Arrange
            string login = "testlogin";
            string password = "testpassword";
            string expectedUserRole = "Мартышка";

            // Act
            string userRole = _form.AuthenticateUser(login, password);

            // Assert
            Assert.AreEqual(expectedUserRole, userRole);
        }
    }
}
