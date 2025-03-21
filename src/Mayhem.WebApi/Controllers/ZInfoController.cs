using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.WebApi.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.WebApi.Controllers
{
    /// <summary>
    /// This controller is used only for testers. This should finally be removed.
    /// </summary>
    [ApiController]
    [Route("api/Info")]
    public class ZInfoController : OwnControllerBase
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;
        public ZInfoController(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets the lands.
        /// </summary>
        /// <returns></returns>
        [Route("Lands")] [HttpGet] public async Task<IActionResult> GetLands() => Ok(await mayhemDataContext.Lands.ToListAsync());
        /// <summary>
        /// Gets the land ids.
        /// </summary>
        /// <returns></returns>
        [Route("LandIds")] [HttpGet] public async Task<IActionResult> GetLandIds() => Ok(await mayhemDataContext.Lands.Select(x => x.Id).ToListAsync());
        /// <summary>
        /// Gets the NPCS.
        /// </summary>
        /// <returns></returns>
        [Route("Npcs")] [HttpGet] public async Task<IActionResult> GetNpcs() => Ok(await mayhemDataContext.Npcs.ToListAsync());
        /// <summary>
        /// Gets the NPC ids.
        /// </summary>
        /// <returns></returns>
        [Route("NpcIds")] [HttpGet] public async Task<IActionResult> GetNpcIds() => Ok(await mayhemDataContext.Npcs.Select(x => x.Id).ToListAsync());
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        [Route("Items")] [HttpGet] public async Task<IActionResult> GetItems() => Ok(await mayhemDataContext.Items.ToListAsync());
        /// <summary>
        /// Gets the item ids.
        /// </summary>
        /// <returns></returns>
        [Route("ItemIds")] [HttpGet] public async Task<IActionResult> GetItemIds() => Ok(await mayhemDataContext.Items.Select(x => x.Id).ToListAsync());

        /// <summary>
        /// Adds the resource to user.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        [Route("Resource/User/{amount:long}")]
        [HttpGet]
        public async Task<IActionResult> AddResourceToUser(long amount)
        {
            List<UserResource> resource = await mayhemDataContext
                .UserResources
                .Where(x => x.UserId == UserId).ToListAsync();

            foreach (UserResource item in resource)
            {
                item.Value += amount;
            }

            await mayhemDataContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Add resources to guild
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        [Route("Resource/Guild/{amount:long}")]
        [HttpGet]
        public async Task<IActionResult> AddResourcesToGuild(long amount)
        {
            Guild guild = await mayhemDataContext.Guilds.Include(x => x.GuildResources).SingleOrDefaultAsync(x => x.OwnerId == UserId);

            if (guild == null)
            {
                return NotFound();
            }

            foreach (GuildResource item in guild.GuildResources)
            {
                item.Value += amount;
            }

            await mayhemDataContext.SaveChangesAsync();

            return Ok();

        }

        /// <summary>
        /// Gets the NPC by identifier.
        /// </summary>
        /// <param name="npcId">The NPC identifier.</param>
        /// <returns></returns>
        [Route("Npc/{npcId:long}")]
        [HttpGet]
        public async Task<IActionResult> GetNpcById([FromRoute] long npcId) => Ok(await mayhemDataContext
            .Npcs
            .Include(x => x.Attributes)
            .SingleOrDefaultAsync(x => x.Id == npcId));

        /// <summary>
        /// Gets the item by identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns></returns>
        [Route("Item/{itemId:long}")]
        [HttpGet]
        public async Task<IActionResult> GetItemById([FromRoute] long itemId) => Ok(await mayhemDataContext
            .Items
            .Include(x => x.ItemBonuses)
            .SingleOrDefaultAsync(x => x.Id == itemId));

        /// <summary>
        /// Gets the land by identifier.
        /// </summary>
        /// <param name="landId">The land identifier.</param>
        /// <returns></returns>
        [Route("Land/{landId:long}")]
        [HttpGet]
        public async Task<IActionResult> GetLandById([FromRoute] long landId) => Ok(await mayhemDataContext
            .Lands
            .Include(x => x.TravelsFrom)
            .Include(x => x.TravelsTo)
            .Include(x => x.Buildings)
            .Include(x => x.Jobs)
            .SingleOrDefaultAsync(x => x.Id == landId));

        /// <summary>
        /// Gets the guild by identifier.
        /// </summary>
        /// <param name="guildId">The guidl identifier.</param>
        /// <returns></returns>
        [Route("Guild/{guildId:int}")]
        [HttpGet]
        public async Task<IActionResult> GetGuildById([FromQuery] int guildId) => Ok(await mayhemDataContext
            .Guilds
            .Include(x => x.GuildResources)
            .Include(x => x.GuildInvitations)
            .Include(x => x.GuildBuildings)
            .Include(x => x.GuildImprovements)
            .Include(x => x.Users)
            .SingleOrDefaultAsync(x => x.Id == guildId));

        [Route("land/add")]
        [HttpPost]
        public async Task<IActionResult> AddLand([FromBody] LandDto land)
        {
            EntityEntry<Land> landDb = await mayhemDataContext.Lands.AddAsync(mapper.Map<Land>(land));
            await mayhemDataContext.SaveChangesAsync();

            return Ok(landDb.Entity);
        }
    }
}
