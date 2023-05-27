using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;
using WebAPI.Scratches;

namespace WebAPI
{
    public class LandmassCardsRepository : ILandmassCardsRepository
    {
        private readonly PlayerContext _playerContext;
        public LandmassCardsRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public async Task<List<Card>> GetLandmassCardsAsync(Guid gameId)
        {
            var cards = await _playerContext.Cards.Where(x => x.GameId == gameId).ToListAsync();
            return cards;
        }
    }
}
