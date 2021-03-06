﻿using System;
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
        private readonly int _itemPerPage = 10;
        private readonly ILogger<NominateController> _logger;

        public VoteController(AlignmentVoteContext database, ILogger<NominateController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPages(int page = 1, string orderBy = "VoteId", bool asc = true)
        {
            page = page < 1 ? 1 : page;
            var count = await _database.Votes.CountAsync();
            if (count == 0)
                return Ok(new {maxPage = 0, page = 0, data = new List<Nominate>()});

            var maxPage = count / _itemPerPage + 1;
            page = page > maxPage ? maxPage : page;

            var result = _database.Votes.AsQueryable();
            if (string.IsNullOrWhiteSpace(orderBy))
                return BadRequest($"Invalid {nameof(orderBy)} parameter");
            try
            {
                orderBy = orderBy[0].ToString().ToUpper() + orderBy.Substring(1);
                result = asc
                    ? Queryable.OrderBy(result, (dynamic) Utils.Utils.ObjectPropertyExpression<Nominate>(orderBy))
                    : Queryable.OrderByDescending(result,
                        (dynamic) Utils.Utils.ObjectPropertyExpression<Nominate>(orderBy));
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

        [HttpGet("result")]
        public async Task<IActionResult> GetResult()
        {
            var votes = await _database.Votes.Include(t => t.Nominate).Select(t => t.Nominate).ToListAsync();
            var alignments = votes.Select(t => t.Alignment).Distinct();
            var result = alignments.Select(alignment => new
            {
                key = alignment,
                value = votes.Where(vote => vote.Alignment == alignment)
                    .GroupBy(vote => vote.Name)
                    .Select(group => new {name = group.Key, count = group.Count()})
                    .OrderByDescending(t => t.count).First().name
            }).Select(prevData => new
            {
                prevData.key,
                value = new Dictionary<string, string>
                {
                    {
                        prevData.value,
                        votes.Where(vote => vote.Alignment == prevData.key && vote.Name == prevData.value)
                            .GroupBy(vote => vote.Saying)
                            .Select(group => new {saying = group.Key, count = group.Count()})
                            .OrderByDescending(t => t.count).First().saying
                    }
                }
            }).ToDictionary(t => t.key, t => t.value);
            return Ok(result);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IList<Vote> votes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var vote in votes)
                    {
                        vote.Date = DateTime.UtcNow;
                    }
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