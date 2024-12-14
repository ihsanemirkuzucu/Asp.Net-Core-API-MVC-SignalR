using System.ComponentModel.DataAnnotations;

namespace SignalR.SampleProject.Web.Models.ViewModels
{
    public record SignInViewModel([Required] string Email, [Required] string Password);
}
