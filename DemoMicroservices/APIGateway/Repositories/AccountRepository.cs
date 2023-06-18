using System;
using APIGateway.DataContext;
using APIGateway.DTOs;
using APIGateway.Interfaces;
using APIGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.Repositories
{
	public class AccountRepository: IAccountInterface
	{
        private readonly GatewayDBContext _context;

        public AccountRepository(GatewayDBContext context)
        {
            _context = context;
        }

        public Boolean Register(RegistrationDTO register, String hashedPassword)
        {
            UserModel newUser = new UserModel()
            {
                Username = register.Username,
                Email = register.Email,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
            return Save();
        }

        public UserModel? GetAccount(String username)
        {
            return _context.Users.Where(d => d.Username == username).FirstOrDefault();
        }

        public Boolean Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

