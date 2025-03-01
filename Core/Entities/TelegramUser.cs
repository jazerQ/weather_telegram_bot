using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TelegramUser
    {
        [Key] public long Id { get; set; }
        [Required][StringLength(50, MinimumLength = 5)]public string Name { get; set; } = string.Empty;
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required]public string LastName { get; set; } = string.Empty;
        [Required]public string Shortname { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }

    }
}
