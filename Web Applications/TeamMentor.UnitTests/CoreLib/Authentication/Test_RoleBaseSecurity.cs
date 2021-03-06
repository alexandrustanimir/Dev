﻿using System;
using System.Web;
using FluentSharp.CoreLib;
using NUnit.Framework;
using TeamMentor.CoreLib;

namespace TeamMentor.UnitTests.Authentication
{
    [TestFixture]
    public class Test_RoleBaseSecurity
    {
        readonly HttpContextBase httpContext;
        readonly string[]        emptyList = new string[] {};

        public Test_RoleBaseSecurity()
        {
            HttpContextFactory.Context = new API_Moq_HttpContext().httpContext();
            httpContext                = HttpContextFactory.Context;                    // also ensure that HttpContextFactory.Context is being correctly set
        }
        
        [Test] public void getCurrentUserRoles()
        {
            var currentRoles = httpContext.getCurrentUserRoles();
            Assert.NotNull(currentRoles);
            Assert.AreEqual(emptyList, currentRoles);
        }
        [Test] public void setCurrentUserRoles()
        {
            var currentUser = httpContext.User;
            Assert.AreEqual("genericIdentity", currentUser.Identity.Name);
            Assert.AreEqual(emptyList, httpContext.getCurrentUserRoles());
            
            Action<UserGroup> assignToGroup = 
                (userGroup)=>{                  
                                var updatedUser = httpContext.setCurrentUserRoles(userGroup);            
                                Assert.NotNull(currentUser);
                                Assert.NotNull(updatedUser);
                                Assert.AreNotEqual(currentUser,updatedUser);
                                Assert.AreEqual(httpContext.User,updatedUser);
                                Assert.AreEqual(userGroup.userRoles().toStringArray(), httpContext.getCurrentUserRoles());
                            };
            foreach(UserGroup userGroup in Enum.GetValues(typeof(UserGroup)))
                assignToGroup(userGroup);
        }
        [Test] public void getThreadPrincipalWithRoles()
        {
            var currentRoles = httpContext.getThreadPrincipalWithRoles();
            Assert.NotNull(currentRoles);
            Assert.AreEqual(emptyList, currentRoles);
        }
        [Test] public void setThreadPrincipalWithRoles()
        {            
            
            Action<UserGroup> assignToGroup = 
                (userGroup)=>{                  
                                var expectedRoles = userGroup.userRoles().toStringArray();
                                userGroup.setThreadPrincipalWithRoles();                                            
                                Assert.AreEqual(expectedRoles, httpContext.getThreadPrincipalWithRoles());
                            };
            foreach(UserGroup userGroup in Enum.GetValues(typeof(UserGroup)))
                assignToGroup(userGroup);
        }        

        [Test] public void setPrivilege()
        {            
            Assert.AreEqual(emptyList, httpContext.getThreadPrincipalWithRoles());
            Action<UserRole> checkMapping = 
                (userRole)=>
                    {
                        userRole.setPrivilege();
                        Assert.AreEqual(new [] {userRole.str()}, httpContext.getThreadPrincipalWithRoles());
                    };
            
            foreach(UserRole userRole in Enum.GetValues(typeof(UserRole)))
                checkMapping(userRole);            
        }
        
    }
}
