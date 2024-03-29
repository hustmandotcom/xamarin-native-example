﻿using System;
using System.Linq;
using NUnit.Framework;
using PracticalCodingTest.Data;
using PracticalCodingTest.Database;
using PracticalCodingTest.DatabaseInterfaces;
using PracticalCodingTest.Domain;
using PracticalCodingTest.DomainInterfaces;

namespace PracticalCodingTest.UnitTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IDatabase<User> _inMemoryDatabase;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _inMemoryDatabase = new InMemoryDatabase();
            _userRepository = new UserRepository(_inMemoryDatabase);
        }

        [Test]
        public void AddingLocalUserAllowsToGetUserByUsername()
        {
            // arrange
            var username = "test user";
            var password = "123rtas54";
            var user = new User(username, password);

            // act
            _userRepository.AddUser(user);
            var userFromDatabase = _userRepository.GetUserByUsername(user.Username);

            // assert
            Assert.IsTrue(userFromDatabase.Username.Equals(username));
        }

        [Test]
        public void GettingUnknownUsernameThrowsIndexOutOfRangeException()
        {
            // arrange
            var usernameToTestFor = "test user";

            // act / assert
            var ex = Assert.Throws<IndexOutOfRangeException>(() => _userRepository.GetUserByUsername(usernameToTestFor));
            Assert.That(ex.Message.Equals(ExceptionMessagesConstant.UserDoesNotExist));
        }

        [Test]
        public void UpdatingLocalUserDoesNotUpdateUserInDatabase()
        {
            // arrange
            var username = "test user";
            var newUsername = "test user 2";
            var password = "123rtas54";
            var user = new User(username, password);

            // act
            _userRepository.AddUser(user);
            var userFromDatabase = _userRepository.GetUserByUsername(user.Username);
            user.Username = newUsername;

            // assert
            Assert.IsFalse(userFromDatabase.Username.Equals(user.Username));
        }

        [Test]
        public void AddingUserWithDuplicateUsernameThrowsInvalidOperationException()
        {
            // arrange
            var username = "test user";
            var password = "123rtas54";
            var user = new User(username, password);

            // act
            _userRepository.AddUser(user);

            // assert
            var ex = Assert.Throws<InvalidOperationException>(() => _userRepository.AddUser(user));
            Assert.That(ex.Message.Equals(ExceptionMessagesConstant.UserAlreadyExists));
        }

        [Test]
        public void CanGetAllUsers()
        {
            // arrange
            var username1 = "test user";
            var password1 = "123rtas54";
            var user1 = new User(username1, password1);

            var username2 = "test user2";
            var password2 = "123rtas54";
            var user2 = new User(username2, password2);

            // act
            _userRepository.AddUser(user1);
            _userRepository.AddUser(user2);

            // assert
            Assert.IsTrue(_userRepository.Users.Count() == 2);
        }
    }
}
