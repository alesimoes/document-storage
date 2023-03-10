using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Controllers.Documents
{
    public class FileMetaData
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
