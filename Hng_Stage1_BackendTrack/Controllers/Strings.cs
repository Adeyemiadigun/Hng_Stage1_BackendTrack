using Hng_Stage1_BackendTrack.Dtos;
using Hng_Stage1_BackendTrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hng_Stage1_BackendTrack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Strings(StringAnalyzerService _stringService) : ControllerBase
    {
        [HttpPost]
        public IActionResult ProcessString([FromBody] PostStringDto value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.Value))
                return BadRequest(new { error = "Missing 'value' field" });

            if (value.Value.GetType() != typeof(string))
                return UnprocessableEntity(new { error = "'value' must be a string" });

            var response = _stringService.AnalyzeString(value.Value);

            if (response == null)
                return Conflict(new { error = "String already exists" });
            return CreatedAtAction("String analyzed and Saved Successfully", response);
        }
        [HttpGet("{string_value}")]
        public IActionResult GetString(string string_value)
        {
            var response = _stringService.GetByValue(string_value);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetByQuery([FromQuery] QueryStringDto value)
        {
            var response = _stringService.GetByQuery(value);
            return Ok(response);

        }
        [HttpGet("filter-by-natural-language")]
        public IActionResult FilterByNaturalLanguage([FromQuery] string query)
        {
            
                var result = _stringService.FilterByNaturalLanguage(query);
                return Ok(result);
        }
        [HttpDelete("{string_value}")]
        public IActionResult DeleteString(string string_value)
        {
             _stringService.DeleteString(string_value);
            return NoContent();
        }
    }
}
