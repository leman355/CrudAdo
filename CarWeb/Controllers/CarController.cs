using CarWeb.Data;
using CarWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CarWeb.Controllers
{
    public class CarController : Controller
    {

        private readonly IConfiguration _configuration;

        private readonly AppDbContext _context;

        public CarController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        public async Task<IActionResult> Index()
        {
            var car = await _context.Cars.ToListAsync();
            return View(car);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Car car, bool IsDeleted)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO public.\"Cars\"(\"Model\", \"Year\", \"Price\", \"IsDeleted\") VALUES(@Model, @Year, @Price, @IsDeleted) RETURNING \"Id\"";
                command.Parameters.AddWithValue("@Model", car.Model);
                command.Parameters.AddWithValue("@Year", car.Year);
                command.Parameters.AddWithValue("@Price", car.Price);
                command.Parameters.AddWithValue("@IsDeleted", IsDeleted);
                var insertedId = await command.ExecuteScalarAsync();
                if (insertedId != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var edit = _context.Cars.FirstOrDefault(e => e.Id == id);
            return View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Car car, bool IsDeleted)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE public.\"Cars\" SET \"Model\" = @Model, \"Year\" = @Year, \"Price\" = @Price, \"IsDeleted\" = @IsDeleted WHERE \"Id\" = @Id";
                command.Parameters.AddWithValue("@Id", car.Id);
                command.Parameters.AddWithValue("@Model", car.Model);
                command.Parameters.AddWithValue("@Year", car.Year);
                command.Parameters.AddWithValue("@Price", car.Price);
                command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                await command.ExecuteNonQueryAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _context.Cars.SingleOrDefaultAsync(x => x.Id == id);
            return View(delete);    
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Car car)
        {
            try
            {
                var c = await _context.Cars.SingleOrDefaultAsync(x => x.Id == car.Id);
                c.IsDeleted = true;
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
