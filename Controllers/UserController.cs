﻿using System.Security.Claims;
using BaseAuth.Database;
using BaseAuth.Extension;
using BaseAuth.Middleware;
using BaseAuth.Model.Dto;
using BaseAuth.Model.Request;
using BaseAuth.Service;
using Microsoft.AspNetCore.Mvc;

namespace BaseAuth.Controllers;

public class UserController(AppDbContext appDbContext, IUserService userService) : AppController
{
    [HttpGet("get-all-users", Name = "GetAllUsers")]
    [Authorised]
    [ResponseBody(typeof(List<UserInfo>))]
    public IActionResult GetAllUsers()
    {
        var claimPrincipal = User;
        var roles = claimPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        Console.WriteLine("Roles: " + string.Join(", ", roles));
        return Ok(userService.GetAllUsers());
    }

    [HttpPost("create-user", Name = "CreateUser")]
    [Authorised("Admin")]
    // [RequestBody(typeof(UserRequest))]
    public IActionResult CreateUser([FromBody] UserRequest request)
    {
        var transaction = appDbContext.Database.BeginTransaction();
        var user = userService.CreateUser(request);
        Console.WriteLine("User created: " + user.FirstName + " " + user.LastName);
        transaction.Commit();
        return Ok(user);
    }
    
}