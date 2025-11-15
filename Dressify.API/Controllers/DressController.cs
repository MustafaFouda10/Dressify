using AutoMapper;
using Dressify.API.Data;
using Dressify.API.DTOs;
using Dressify.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dressify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DressController : ControllerBase
    {
        private readonly DressifyDbContext _db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public DressController(DressifyDbContext db, IMapper mapper, IWebHostEnvironment env)
        {
            _db = db;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dresses = await _db.Dresses.AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<IEnumerable<DressDto>>(dresses);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var d = await _db.Dresses.FindAsync(id);
            if (d == null) return NotFound();
            return Ok(_mapper.Map<DressDto>(d));
        }

        // Admin only: create dress with optional image file
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateDressDto dto, IFormFile? image)
        {
            var dress = _mapper.Map<Dress>(dto);

            if (image != null && image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var relativePath = Path.Combine("images", fileName).Replace("\\", "/");
                var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fullPath = Path.Combine(uploads, fileName);
                using var stream = System.IO.File.Create(fullPath);
                await image.CopyToAsync(stream);
                dress.ImagePath = relativePath;
            }

            _db.Dresses.Add(dress);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = dress.Id }, _mapper.Map<DressDto>(dress));
        }

        // Admin only: update with optional new image
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateDressDto dto, IFormFile? image)
        {
            var dress = await _db.Dresses.FindAsync(id);
            if (dress == null) return NotFound();

            _mapper.Map(dto, dress);

            if (image != null && image.Length > 0)
            {
                // delete old file if exists
                if (!string.IsNullOrEmpty(dress.ImagePath))
                {
                    var oldFull = Path.Combine(_env.WebRootPath ?? "wwwroot", dress.ImagePath);
                    if (System.IO.File.Exists(oldFull))
                        System.IO.File.Delete(oldFull);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var relativePath = Path.Combine("images", fileName).Replace("\\", "/");
                var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fullPath = Path.Combine(uploads, fileName);
                using var stream = System.IO.File.Create(fullPath);
                await image.CopyToAsync(stream);
                dress.ImagePath = relativePath;
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dress = await _db.Dresses.FindAsync(id);
            if (dress == null) return NotFound();

            if (!string.IsNullOrEmpty(dress.ImagePath))
            {
                var full = Path.Combine(_env.WebRootPath ?? "wwwroot", dress.ImagePath);
                if (System.IO.File.Exists(full)) System.IO.File.Delete(full);
            }

            _db.Dresses.Remove(dress);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // optional: upload image separately
        [Authorize(Roles = "Admin")]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file");
            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(uploads, fileName);
            using var stream = System.IO.File.Create(path);
            await file.CopyToAsync(stream);
            var relative = $"/images/{fileName}";
            return Ok(new { url = relative });
        }
    }
}
