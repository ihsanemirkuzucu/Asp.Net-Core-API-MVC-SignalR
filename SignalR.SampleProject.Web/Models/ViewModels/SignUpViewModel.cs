using System.ComponentModel.DataAnnotations;

namespace SignalR.SampleProject.Web.Models.ViewModels
{
    public record SignUpViewModel([Required] string Email, [Required] string Password, [Required] string ConfirmPassword);
}
