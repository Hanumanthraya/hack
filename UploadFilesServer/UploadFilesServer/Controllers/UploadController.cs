using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using UploadFilesServer.services;



namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        IUploadService uploadService;
        public UploadController(IConfiguration configuration)
        {
            this.uploadService = new UploadService(configuration) ?? throw new ArgumentNullException(nameof(uploadService));
        }
        [HttpPost(nameof(UploadFile))]
        public async Task<IActionResult> UploadFile(IFormFile files)
        {
            try
            {
                
                if (files.Length > 0)
                {
                    var fileName = files.FileName;
                    string fileURL = await uploadService.UploadAsync(files.OpenReadStream(), fileName, files.ContentType);

                    return Ok(new { fileURL });
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

       
    }
}
