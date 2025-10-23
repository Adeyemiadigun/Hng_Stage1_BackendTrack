using Hng_Stage1_BackendTrack.Dtos;
using Hng_Stage1_BackendTrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng_Stage1_BackendTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Strings(StringAnalyzerService _stringService) : ControllerBase
    {
        [HttpPost]
        public IActionResult ProcessString([FromBody] PostStringDto value)
        {
            var response = _stringService.AnalyzeString(value.Value);
            return CreatedAtAction("String analyzed and Saved Successfully", response);
        }
        [HttpGet("{string_value}")]
        public IActionResult GetString(string string_value)
        {
            var response = _stringService.GetByValue(string_value);
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
