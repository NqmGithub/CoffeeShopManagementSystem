using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string name, IFormFile file, string folder)
        {
            try
            {
                var folderName = Path.Combine("wwwroot", folder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file != null && file.Length > 0)
                {
                    var fileName = name;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    var existingFiles = Directory.GetFiles(pathToSave, name + ".*");
                    foreach (var existingFile in existingFiles)
                    {
                        System.IO.File.Delete(existingFile);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("update-name")]
        public IActionResult UpdateFileName(string oldName, string newName)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var existingFile = Directory.GetFiles(pathToSave, oldName).FirstOrDefault();
                if (existingFile == null)
                {
                    return NotFound("File not found.");
                }

                var extension = Path.GetExtension(existingFile);

                var newFullPath = Path.Combine(pathToSave, newName);

                System.IO.File.Move(existingFile, newFullPath);

                return Ok(new { message = "File name updated successfully.", newPath = newFullPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
