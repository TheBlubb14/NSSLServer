﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSSLServer.Tests.ServerCommunication;
using NSSLServer.Shared;
using NSSL.Models;
using System.Threading.Tasks;

namespace NSSLServer.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public async Task CreateUser()
        {
            var now = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
            now = now.Replace(':', '-');
            var user = await UserSync.Create("test-user_" + now, "mail+test-user_" + now + "@susch.eu", "123456");
            Assert.IsTrue(user.Success);
            Assert.IsNotNull(user.Id);
            Assert.IsNotNull(user.EMail);
            Assert.IsNotNull(user.Username);
            File.WriteAllLines("TestUser\\test-user_" + now + ".txt", new string[] { user.Id.ToString(), user.EMail, user.Username });
        }

        [TestMethod]
        public async Task CreateTestContributor()
        {
            var now = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
            now = now.Replace(':', '-');
            var user = await UserSync.Create("test-user_03.02.2018_13-11", "test-user_03.02.2018_13-11@testmail.testmail", "123456");
            Assert.IsTrue(user.Success);
            Assert.IsNotNull(user.Id);
            Assert.IsNotNull(user.EMail);
            Assert.IsNotNull(user.Username);
        }

        [TestMethod]
        public async Task Info()
        {
            var testUser = User.GetUser(out var tokenSuccess);

            Assert.IsNotNull(testUser);
            Assert.IsTrue(tokenSuccess);

            var info = await UserSync.Info();

            Assert.AreEqual(info.EMail, testUser.Email);
            Assert.AreEqual(info.Id, testUser.Id);
            Assert.AreEqual(info.Username, testUser.Username);
        }

        [TestMethod]
        public async Task DeleteUser()
        {
            //UserSync.DeleteUser();
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task ChangePassword()
        {

        }

        [TestMethod]
        public async Task LoginUsername()
        {
            var userInfo = User.GetUser();
            var user = UserSync.Login(userInfo.Username, "123456").Result;

            Assert.IsTrue(user.Success);
            Assert.IsNotNull(user.Id);
            Assert.IsNotNull(user.EMail);
            Assert.IsNotNull(user.Username);
            Assert.IsNotNull(user.Token);

            Assert.IsTrue(JsonWebToken.Decode(user.Token, out NsslSession payload));

            Assert.AreEqual(payload.Id, userInfo.Id);
            if (!File.Exists("TestUser\\"+ userInfo.Username + "_token.txt"))
                File.WriteAllLines("TestUser\\" + userInfo.Username + "_token.txt", new string[] { user.Id.ToString(), user.Token, payload.Expires.ToString() });
        }

        [TestMethod]
        public async Task LoginEmail()
        {
            var userInfo = User.GetUser();
            var user = UserSync.LoginEmail(userInfo.Email, "123456").Result;
            Assert.IsTrue(user.Success);
            Assert.IsNotNull(user.Id);
            Assert.IsNotNull(user.EMail);
            Assert.IsNotNull(user.Username);
            Assert.IsNotNull(user.Token);

            Assert.IsTrue(JsonWebToken.Decode(user.Token, out NsslSession payload));

            Assert.AreEqual(payload.Id, userInfo.Id);
            if (!File.Exists("TestUser\\" + userInfo.Username + "_token.txt"))
                File.WriteAllLines("TestUser\\" + userInfo.Username + "_token.txt", new string[] { user.Id.ToString(), user.Token, payload.Expires.ToString() });
        }

        [TestMethod]
        public async Task RefreshToken()
        {

        }
    }

}
