using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Walks.Api.Model.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
