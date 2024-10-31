using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class RegisterDto
	{
		[Required]
		public string DisplayName { get; set; } = null!;


		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		//[Required]
        public string? Phone { get; set; }


        [Required]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
			ErrorMessage ="Password must have 1 Uppercase, 1 LowerCase, 1 number, 1 non alphanumeric and at least 6 characters")]
		public string Password { get; set; } = null!;

    }
}
