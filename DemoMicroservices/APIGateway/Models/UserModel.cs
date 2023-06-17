using System;
namespace APIGateway.Models
{
	public class UserModel
    {
        public Int32 Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}

