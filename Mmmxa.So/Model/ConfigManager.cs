using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhysicalLayer
{
    public class ConfigManager
    {
        [NotMapped]
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少 {2} 个字符。", MinimumLength =2)]
        [Display(Name = "类型名")]
        public string Types { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "键")]
        public string Keys { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "值")]
        public string Value { get; set; }
    }
}
