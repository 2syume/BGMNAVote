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
    public class VoteController : Controller
    {
        private readonly AlignmentVoteContext _database;
        private readonly ILogger<VoteController> _logger;
        private readonly int _itemPerPage = 10;

        public VoteController(AlignmentVoteContext database, ILogger<VoteController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, string orderBy = "VoteId", bool asc = true)
        {
            page = page < 1 ? 1 : page;
            var count = await _database.Votes.CountAsync();
            if (count == 0)
                return Ok(new {maxPage = 0, page = 0, data = new List<Vote>()});

            var maxPage = count / _itemPerPage + 1;
            page = page > maxPage ? maxPage : page;

            var result = _database.Votes.AsQueryable();
            if (string.IsNullOrWhiteSpace(orderBy))
                return BadRequest($"Invalid {nameof(orderBy)} parameter");
            try
            {
                orderBy = orderBy[0].ToString().ToUpper() + orderBy.Substring(1);
                result = asc
                    ? Queryable.OrderBy(result, (dynamic) Utils.Utils.ObjectPropertyExpression<Vote>(orderBy))
                    : Queryable.OrderByDescending(result, (dynamic) Utils.Utils.ObjectPropertyExpression<Vote>(orderBy));
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

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<Vote> votes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _database.Votes.AddRangeAsync(votes);
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