using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using laptops.classes;
using System.Data;

namespace Laptop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly string _mysqlConnString;

        public LaptopController(IConfiguration configuration)
        {
            _mysqlConnString = configuration.GetConnectionString("MySqlConn")
                ?? throw new InvalidOperationException("MySQL connection string not found");
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddLaptop([FromBody] LaptopClass laptop)
        {
            if (laptop == null)
                return BadRequest("Laptop data is missing.");

            using var connection = new MySqlConnection(_mysqlConnString);
            connection.Open();

            var query = @"INSERT INTO `laptop dataset` 
                        (`link`, `name`, `user rating`, `price`, `Sales Package`, `Model Number`, `Part Number`, `Model Name`, `Series`, `Color`)
                        VALUES (@link, @name, @userrating, @price, @SalesPackages, @ModelNumber, @PartNumber, @ModelName, @Series, @Color);";

            using var cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@link", laptop.link);
            cmd.Parameters.AddWithValue("@name", laptop.name);
            cmd.Parameters.AddWithValue("@userrating", laptop.UserRating);
            cmd.Parameters.AddWithValue("@price", laptop.price);
            cmd.Parameters.AddWithValue("@SalesPackages", laptop.SalesPackage);
            cmd.Parameters.AddWithValue("@ModelNumber", laptop.ModelNumber);
            cmd.Parameters.AddWithValue("@PartNumber", laptop.PartNumber);
            cmd.Parameters.AddWithValue("@ModelName", laptop.ModelName);
            cmd.Parameters.AddWithValue("@Series", laptop.Series);
            cmd.Parameters.AddWithValue("@Color", laptop.Color);

            try
            {
                cmd.ExecuteNonQuery();
                return Ok("Laptop added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllLaptops()
        {
            using var connection = new MySqlConnection(_mysqlConnString);
            connection.Open();
            var query = "SELECT * FROM `laptop dataset`;";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            var laptops = new List<LaptopClass>();

            if (!reader.HasRows)
            {
                return NotFound("No laptops found.");
            }

            while (reader.Read())
            {
                LaptopClass laptop = new()
                {
                    link = reader.GetString(reader.GetOrdinal("link")),
                    name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name")),
                    UserRating = reader.IsDBNull(reader.GetOrdinal("user rating")) ? 0 : reader.GetFloat(reader.GetOrdinal("user rating")),
                    price = reader.IsDBNull(reader.GetOrdinal("price")) ? 0 : reader.GetInt32(reader.GetOrdinal("price")),
                    SalesPackage = reader.IsDBNull(reader.GetOrdinal("Sales Package")) ? null : reader.GetString(reader.GetOrdinal("Sales Package")),
                    ModelNumber = reader.IsDBNull(reader.GetOrdinal("Model Number")) ? null : reader.GetString(reader.GetOrdinal("Model Number")),
                    PartNumber = reader.IsDBNull(reader.GetOrdinal("Part Number")) ? null : reader.GetString(reader.GetOrdinal("Part Number")),
                    ModelName = reader.IsDBNull(reader.GetOrdinal("Model Name")) ? null : reader.GetString(reader.GetOrdinal("Model Name")),
                    Series = reader.IsDBNull(reader.GetOrdinal("Series")) ? null : reader.GetString(reader.GetOrdinal("Series")),
                    Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color"))
                };

                laptops.Add(laptop);
            }

            return Ok(laptops);
        }
    }
}
