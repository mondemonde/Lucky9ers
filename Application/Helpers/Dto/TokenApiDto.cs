using Lucky9.Application.Common.Models;
using Lucky9.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Lucky9.Application.Helpers.Dto
{
    public class TokenApiDto
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        
        public List<ValidationDto> ValidationResult { get; set; }
    }
}
