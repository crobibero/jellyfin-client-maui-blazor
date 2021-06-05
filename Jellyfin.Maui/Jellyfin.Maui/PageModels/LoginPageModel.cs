using System.ComponentModel.DataAnnotations;

namespace Jellyfin.Maui.PageModels
{
    /// <summary>
    /// The login page model.
    /// </summary>
    public class LoginPageModel
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [Required]
        [Url]
        [DataType(DataType.Url)]
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [DataType(DataType.Password)]
        public string? Password { get; set; } = string.Empty;
    }
}