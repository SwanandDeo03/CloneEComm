using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MySql.Data.MySqlClient;
using laptops.classes;
using System.Data;

namespace Laptop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
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
                        (`link`, `name`, `userrating`, `Price`, `SalesPackage`, `ModelNumber`, `PartNumber`, `ModelName`, `Series`, `Color`)
                        VALUES (@link, @name, @userrating, @price, @SalesPackages, @ModelNumber, @PartNumber, @ModelName, @Series, @Color);";

            using var cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@link", laptop.link);
            cmd.Parameters.AddWithValue("@name", laptop.name);
            cmd.Parameters.AddWithValue("@userrating", laptop.userrating);
            cmd.Parameters.AddWithValue("@price", laptop.Price);
            cmd.Parameters.AddWithValue("@SalesPackages", laptop.SalesPackage);
            cmd.Parameters.AddWithValue("@ModelNumber", laptop.ModelNumber);
            cmd.Parameters.AddWithValue("@PartNumber", laptop.PartNumber);
            cmd.Parameters.AddWithValue("@ModelName", laptop.ModelName);
            cmd.Parameters.AddWithValue("@Series", laptop.Series);
            cmd.Parameters.AddWithValue("@Color", laptop.Color);

            try
            {
                cmd.ExecuteNonQuery();
                return Ok(new
                {
                    success = true,
                    message = "Laptop Dataset added successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllLaptops()
        {
            try
            {
                using var connection = new MySqlConnection(_mysqlConnString);
                connection.Open();

                var query = "SELECT * FROM `laptop dataset`;";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                var laptops = new List<LaptopClass>();

                while (reader.Read())
                {
                    string priceStr = reader.IsDBNull(reader.GetOrdinal("Price")) ? "0" : reader.GetString("Price");
                    // Remove ₹ symbol, commas, and any whitespace
                    priceStr = priceStr.Replace("₹", "").Replace(",", "").Replace(" ", "").Trim();

                    // Try to parse the price, default to 0 if parsing fails
                    int price = 0;
                    int.TryParse(priceStr, out price);

                    string ratingStr = reader.IsDBNull(reader.GetOrdinal("userrating")) ? "0" : reader.GetString("userrating");
                    float rating = 0;
                    float.TryParse(ratingStr, out rating);

                    var laptop = new LaptopClass
                    {
                        link = reader.IsDBNull(reader.GetOrdinal("link")) ? string.Empty : reader.GetString("link"),
                        name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString("name"),
                        userrating = rating,
                        Price = price,
                        SalesPackage = reader.IsDBNull(reader.GetOrdinal("SalesPackage")) ? null : reader.GetString("SalesPackage"),
                        ModelNumber = reader.IsDBNull(reader.GetOrdinal("ModelNumber")) ? null : reader.GetString("ModelNumber"),
                        PartNumber = reader.IsDBNull(reader.GetOrdinal("PartNumber")) ? null : reader.GetString("PartNumber"),
                        ModelName = reader.IsDBNull(reader.GetOrdinal("ModelName")) ? null : reader.GetString("ModelName"),
                        Series = reader.IsDBNull(reader.GetOrdinal("Series")) ? null : reader.GetString("Series"),
                        Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString("Color")
                    };
                    laptops.Add(laptop);
                }

                if (!laptops.Any())
                {
                    return NotFound("No laptops found.");
                }

                return Ok(laptops);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return StatusCode(500, $"Data format error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult SearchLaptops([FromQuery] string searchTerm)
        {
            try
            {
                using var connection = new MySqlConnection(_mysqlConnString);
                connection.Open();

                var query = @"SELECT * FROM `laptop dataset` 
                            WHERE LOWER(`name`) LIKE @searchTerm 
                            OR LOWER(`ModelNumber`) LIKE @searchTerm 
                            OR LOWER(`Series`) LIKE @searchTerm 
                            OR LOWER(`ModelName`) LIKE @searchTerm;";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm.ToLower()}%");

                using var reader = cmd.ExecuteReader();
                var laptops = new List<LaptopClass>();

                while (reader.Read())
                {
                    string priceStr = reader.IsDBNull(reader.GetOrdinal("Price")) ? "0" : reader.GetString("Price");
                    priceStr = priceStr.Replace("₹", "").Replace(",", "").Replace(" ", "").Trim();
                    int.TryParse(priceStr, out int price);

                    string ratingStr = reader.IsDBNull(reader.GetOrdinal("userrating")) ? "0" : reader.GetString("userrating");
                    float.TryParse(ratingStr, out float rating);

                    var laptop = new LaptopClass
                    {
                        link = reader.IsDBNull(reader.GetOrdinal("link")) ? string.Empty : reader.GetString("link"),
                        name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString("name"),
                        userrating = rating,
                        Price = price,
                        SalesPackage = reader.IsDBNull(reader.GetOrdinal("SalesPackage")) ? null : reader.GetString("SalesPackage"),
                        ModelNumber = reader.IsDBNull(reader.GetOrdinal("ModelNumber")) ? null : reader.GetString("ModelNumber"),
                        PartNumber = reader.IsDBNull(reader.GetOrdinal("PartNumber")) ? null : reader.GetString("PartNumber"),
                        ModelName = reader.IsDBNull(reader.GetOrdinal("ModelName")) ? null : reader.GetString("ModelName"),
                        Series = reader.IsDBNull(reader.GetOrdinal("Series")) ? null : reader.GetString("Series"),
                        Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString("Color")
                    };
                    laptops.Add(laptop);
                }

                return Ok(new { 
                    success = true,
                    data = laptops,
                    count = laptops.Count,
                    message = laptops.Any() ? "Laptops found" : "No laptops found matching your criteria"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error searching laptops", 
                    error = ex.Message 
                });
            }
        }
    }
}
