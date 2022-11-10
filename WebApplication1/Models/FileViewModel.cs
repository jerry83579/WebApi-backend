using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class FileViewModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}