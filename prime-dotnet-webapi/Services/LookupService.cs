using AutoMapper;
using AutoMapper.QueryableExtensions;
using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

using Prime.Models;
using Prime.Models.Api;
using Prime.Models.HealthAuthorities;
using Prime.ViewModels;
namespace Prime.Services
{
    public class LookupService : BaseService, ILookupService
    {
        private readonly IMapper _mapper;
        public LookupService(
            ApiDbContext context,
            ILogger<LookupService> logger,
            IMapper mapper)
            : base(context, logger)
        {
            _mapper = mapper;
        }

        public async Task<int> GetCareSettingCountAsync()
        {
            return await _context.Set<CareSetting>().CountAsync();
        }

        public async Task<LookupEntity> GetLookupsAsync()
        {
            return new LookupEntity
            {
                Colleges = await _context.Set<College>()
                    .AsNoTracking()
                    .Include(c => c.CollegeLicenses)
                    .Include(c => c.CollegePractices)
                    .ToListAsync(),
                JobNames = await _context.Set<JobName>()
                    .AsNoTracking()
                    .ToListAsync(),
                Licenses = await _context.Set<License>()
                    .AsNoTracking()
                    .Where(l => l.CurrentLicenseDetail != null)
                    .ProjectTo<LicenseViewModel>(_mapper.ConfigurationProvider)
                    .DecompileAsync()
                    .ToListAsync(),
                CareSettings = await _context.Set<CareSetting>()
                    .AsNoTracking()
                    .ToListAsync(),
                Practices = await _context.Set<Practice>()
                    .AsNoTracking()
                    .Include(p => p.CollegePractices)
                    .ToListAsync(),
                Statuses = await _context.Set<Status>()
                    .AsNoTracking()
                    .ToListAsync(),
                Countries = await _context.Set<Country>()
                    .AsNoTracking()
                    .ToListAsync(),
                Provinces = await _context.Set<Province>()
                    .AsNoTracking()
                    .ToListAsync(),
                StatusReasons = await _context.Set<StatusReason>()
                    .AsNoTracking()
                    .ToListAsync(),
                Vendors = await _context.Set<Vendor>()
                    .AsNoTracking()
                    .ToListAsync(),
                HealthAuthorities = await _context.Set<HealthAuthority>()
                    .AsNoTracking()
                    .ToListAsync(),
                Facilities = await _context.Set<Facility>()
                    .AsNoTracking()
                    .ToListAsync(),
                CollegeLicenseGroupings = await _context.Set<CollegeLicenseGrouping>()
                    .AsNoTracking()
                    .ToListAsync(),
                CareTypes = await _context.Set<CareType>()
                    .AsNoTracking()
                    .ToListAsync(),
                SecurityGroups = await _context.Set<SecurityGroup>()
                    .AsNoTracking()
                    .ToListAsync()
            };
        }
    }
}
