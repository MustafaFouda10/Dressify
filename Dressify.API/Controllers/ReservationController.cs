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
    public class ReservationController : ControllerBase
    {
        private readonly DressifyDbContext _db;
        private readonly IMapper _mapper;

        public ReservationController(DressifyDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // Public: create reservation (simple double-book check)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            var eventDate = dto.EventDate.Date;

            var dress = await _db.Dresses.FindAsync(dto.DressId);
            if (dress == null || !dress.IsAvailable) return BadRequest("Dress not available.");

            var exists = await _db.Reservations
                .AsNoTracking()
                .Where(r => r.DressId == dto.DressId && r.EventDate.Date == eventDate && r.Status != ReservationStatus.Canceled)
                .AnyAsync();

            if (exists) return Conflict(new { message = "This dress is already reserved on that date." });

            var reservation = _mapper.Map<Reservation>(dto);
            reservation.EventDate = eventDate;
            reservation.Status = ReservationStatus.Pending;
            reservation.CreatedAt = DateTime.UtcNow;

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            var result = await _db.Reservations.Include(r => r.Dress).FirstOrDefaultAsync(r => r.Id == reservation.Id);
            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, _mapper.Map<ReservationDto>(result));
        }

        // Admin-only: list all reservations
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Reservations.Include(r => r.Dress).AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(list));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var r = await _db.Reservations.Include(r => r.Dress).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (r == null) return NotFound();
            return Ok(_mapper.Map<ReservationDto>(r));
        }

        // Admin: update status (enum binding)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ReservationStatus status)
        {
            var r = await _db.Reservations.FindAsync(id);
            if (r == null) return NotFound();

            // if confirming, ensure no other confirmed/active reservation exists for same date
            if (status == ReservationStatus.Confirmed)
            {
                var exists = await _db.Reservations
                    .Where(x => x.DressId == r.DressId && x.EventDate.Date == r.EventDate.Date && x.Id != r.Id && x.Status != ReservationStatus.Canceled)
                    .AnyAsync();
                if (exists) return Conflict(new { message = "Another reservation exists for that dress on the same date." });
            }

            r.Status = status;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
