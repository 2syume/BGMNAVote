using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGMNANotebookGrab.Data;
using BGMNANotebookGrab.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BGMNANotebookGrab.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class NominateController : Controller
    {
        private readonly AlignmentVoteContext _database;
        private readonly ILogger<NominateController> _logger;
        private readonly int _itemPerPage = 10;

        public NominateController(AlignmentVoteContext database, ILogger<NominateController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPages(int page = 1, string orderBy = "NominateId", bool asc = true)
        {
            page = page < 1 ? 1 : page;
            var count = await _database.Nominates.CountAsync();
            if (count == 0)
                return Ok(new {maxPage = 0, page = 0, data = new List<Nominate>()});

            var maxPage = count / _itemPerPage + 1;
            page = page > maxPage ? maxPage : page;

            var result = _database.Nominates.AsQueryable();
            if (string.IsNullOrWhiteSpace(orderBy))
                return BadRequest($"Invalid {nameof(orderBy)} parameter");
            try
            {
                orderBy = orderBy[0].ToString().ToUpper() + orderBy.Substring(1);
                result = asc
                    ? Queryable.OrderBy(result, (dynamic) Utils.Utils.ObjectPropertyExpression<Nominate>(orderBy))
                    : Queryable.OrderByDescending(result, (dynamic) Utils.Utils.ObjectPropertyExpression<Nominate>(orderBy));
            }
            catch (ArgumentException)
            {
                return BadRequest($"Invalid {nameof(orderBy)} parameter");
            }

            result = result.Skip((page - 1) * _itemPerPage);
            if (page != maxPage)
                result = result.Take(_itemPerPage);

            return Ok(new {maxPage, page, data = result});
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _database.Nominates.FindAsync(id));
        }

        [HttpGet("{alignment}")]
        public async Task<IActionResult> GetNominateUser(string alignment)
        {
            return Ok(await _database.Nominates.Where(t => t.Alignment == alignment).ToListAsync());
        }

        [HttpGet("{alignment}/{name}")]
        public async Task<IActionResult> GetNominateSaying(string alignment, string name)
        {
            name = name.Replace("%2F", "/");
            return Ok((await _database.Nominates
                .Where(t => t.Alignment == alignment && t.Name == name).ToListAsync())
                .GroupBy(t => t.Saying)
                .Select(t => t.First())
                .ToList());
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<Nominate> nominates)
        {
            return BadRequest();
            try
            {
                if (ModelState.IsValid)
                {
                    await _database.Nominates.AddRangeAsync(nominates);
                    await _database.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Cannot save changes to database");
                ModelState.AddModelError("", "无法提交数据，请联系群管理员");
            }

            return BadRequest(ModelState);
        }
    }
}