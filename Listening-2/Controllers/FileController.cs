using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.IO;
using listening.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace listening.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private string _path;

        public FileController(IConfigurationRoot configuration)
        {
            _path = Directory.GetCurrentDirectory() + configuration["FileStorage:AudioPath"];
        }

        [HttpPut("{id}")]
        public JsonResult PutAudioFile(string id, IList<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName
                        .Trim('"').Replace("\\", "").Replace("/", "");
                    var filesInFolder = Directory.GetFiles(_path);
                    filesInFolder = filesInFolder
                        .Select(x => x.Split('/').Last()).ToArray();

                    if (filesInFolder.Contains(fileName))
                        throw new FileUploadException($"File with name {fileName} is already exists.");

                    using (var fs = new FileStream(Path.Combine(_path, fileName), FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                }
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("");
        }

        [HttpDelete("{name}")]
        public JsonResult DeleteFile(string name)
        {
            var fullPath = $"{_path}/{name}";

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
            else
                throw new FileUploadException($"Can`t remove file with name {name} due to his inexistence.");

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("");
        }
    }
}
