using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace XOProject.Controller
{
    [Route("api/Trade/")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }

        public TradeController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portFolioid)
        {
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));
            return Ok(trade);
        }


        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, 
        /// average and Sum of all the trades for that share individually grouped into Buy trade and Sell trade.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public async Task<IActionResult> GetAnalysis([FromRoute]int portFolioid)
        {
            var shares = _tradeRepository.Query().Where(x => x.Symbol.Equals(portFolioid));
            var q_sell = shares.Where(i => i.Action.Equals("SELL"));
            var q_buy = shares.Where(i => i.Action.Equals("BUY"));

            TradeAnalysis sell = new TradeAnalysis();
            if (q_sell.Count() > 0)
            {
                sell.Sum = q_sell.Sum(i => i.NoOfShares);
                sell.Average = q_sell.Average(i => i.NoOfShares);
                sell.Maximum = q_sell.Max(i => i.NoOfShares);
                sell.Minimum = q_sell.Min(i => i.NoOfShares);
            }
            sell.Action = "SELL";

            TradeAnalysis buy = new TradeAnalysis();
            if (q_buy.Count() > 0)
            {
                buy.Sum = q_buy.Sum(i => i.NoOfShares);
                buy.Average = q_buy.Average(i => i.NoOfShares);
                buy.Maximum = q_buy.Max(i => i.NoOfShares);
                buy.Minimum = q_buy.Min(i => i.NoOfShares);
            }
            buy.Action = "BUY";

            var result = new List<TradeAnalysis>();
            result.Add(sell);
            result.Add(buy);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Trade value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeRepository.InsertAsync(value);

            return Created($"Trade/{value.Id}", value);
        }
    }
}
