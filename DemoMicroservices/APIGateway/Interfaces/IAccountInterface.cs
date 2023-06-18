using System;
using APIGateway.DTOs;
using APIGateway.Models;

namespace APIGateway.Interfaces
{
	public interface IAccountInterface
	{
        public Boolean Register(RegistrationDTO register, String hashedPassword);
        public UserModel? GetAccount(String username);
    }
}