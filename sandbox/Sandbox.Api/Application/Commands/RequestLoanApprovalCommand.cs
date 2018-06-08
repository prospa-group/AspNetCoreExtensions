using System.ComponentModel.DataAnnotations;

namespace Sandbox.Api.Application.Commands
{
    public class RequestLoanApprovalCommand
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string Value { get; set; }
    }
}