using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Land.Bl.Dtos;
using Mayhem.Land.Bl.Interfaces;
using Mayhem.Util.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Land.Bl.Services
{
    public class LandGeneratorService : ILandGeneratorService
    {
        private readonly ILandRepository landRepository;
        private readonly IMapper mapper;

        public LandGeneratorService(ILandRepository landRepository, IMapper mapper)
        {
            this.landRepository = landRepository;
            this.mapper = mapper;
        }

        public async Task<List<ImportLandDto>> ReadLandsAsync()
        {
            string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/lands.txt";
            string stringLands = await File.ReadAllTextAsync(filePath);
            List<ImportLandDto> lands = JsonConvert.DeserializeObject<List<ImportLandDto>>(stringLands);

            return lands;
        }

        public async Task<IEnumerable<LandDto>> SaveLandsAsync(List<ImportLandDto> lands) => await landRepository.AddLandsAsync(lands.Select(x => mapper.Map<LandDto>(x, opts =>
        {
            opts.Items[MappingParamConstants.LandInstanceParam] = 1;
        })));
    }
}
