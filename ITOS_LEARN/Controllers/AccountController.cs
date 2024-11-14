﻿using Microsoft.AspNetCore.Mvc;
using ITOS_LEARN.Models;
using ITOS_LEARN.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace ITOS_LEARN.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> passwordHasher;

        public AccountController(ApplicationDbContext context)
        {
            passwordHasher = new PasswordHasher<User>();
            _context = context;
        }

        // หน้า Login GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // หน้า Login POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Email == model.Email);

                var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (user != null)
                {
                    if (result == PasswordVerificationResult.Success) {
                        var login = new Login
                        {
                            Username = user.Username,
                            LoginTime = DateTime.Now,
                            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() // เก็บ IP Address
                        };

                        _context.Logins.Add(login); // เพิ่ม Login ลงในฐานข้อมูล
                        await _context.SaveChangesAsync(); // บันทึกการเปลี่ยนแปลง

                        // พาผู้ใช้ไปหน้า People (หน้า Details)
                        return RedirectToAction("Index", "People");
                    }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username, password, or email.");
                    }
                }
                return View(model);
            }
       

        // หน้า Register GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // หน้า Register POST
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var existingEmail = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingEmail != null) {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                var hashPassword = passwordHasher.HashPassword(user, user.Password);
                user.Password = hashPassword;
             
                // บันทึกข้อมูลผู้ใช้ใหม่
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(user);
        }
    }
}
