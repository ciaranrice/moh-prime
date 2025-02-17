using AutoMapper;
using AutoMapper.QueryableExtensions;
using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

using Prime.Configuration.Auth;
using Prime.HttpClients;
using Prime.HttpClients.DocumentManagerApiDefinitions;
using Prime.Models;
using Prime.Models.Api;
using Prime.Models.VerifiableCredentials;
using Prime.ViewModels;

namespace Prime.Services
{
    public class EnrolleeService : BaseService, IEnrolleeService
    {
        private readonly IBusinessEventService _businessEventService;
        private readonly IDocumentManagerClient _documentClient;
        private readonly IMapper _mapper;

        public EnrolleeService(
            ApiDbContext context,
            ILogger<EnrolleeService> logger,
            IBusinessEventService businessEventService,
            IDocumentManagerClient documentClient,
            IMapper mapper)
            : base(context, logger)
        {
            _businessEventService = businessEventService;
            _documentClient = documentClient;
            _mapper = mapper;
        }

        public async Task<bool> EnrolleeExistsAsync(int enrolleeId)
        {
            return await _context.Enrollees
                .AsNoTracking()
                .AnyAsync(e => e.Id == enrolleeId);
        }

        public async Task<bool> UserIdExistsAsync(Guid userId)
        {
            return await _context.Enrollees
                .AsNoTracking()
                .AnyAsync(e => e.UserId == userId);
        }

        public async Task<bool> GpidExistsAsync(string gpid)
        {
            return await _context.Enrollees
                .AsNoTracking()
                .AnyAsync(e => e.GPID == gpid);
        }

        public async Task<EnrolleeStub> GetEnrolleeStubAsync(Guid userId)
        {
            return await _context.Enrollees
                .AsNoTracking()
                .Where(e => e.UserId == userId)
                .Select(e => new EnrolleeStub { Id = e.Id, UserId = e.UserId })
                .SingleOrDefaultAsync();
        }

        public async Task<PermissionsRecord> GetPermissionsRecordAsync(int enrolleeId)
        {
            return await _context.Enrollees
                .AsNoTracking()
                .Where(e => e.Id == enrolleeId)
                .Select(e => new PermissionsRecord { UserId = e.UserId })
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets the GPID for an Enrollee.
        /// Returns null if no Enrollee exists with the given UserId or if the Enrollee is in the 'Declined' status
        /// </summary>
        /// <param name="userId"></param>
        public async Task<string> GetActiveGpidAsync(Guid userId)
        {
            return await _context.Enrollees
                .Where(enrollee => enrollee.UserId == userId
                    && enrollee.CurrentStatus.StatusCode != (int)StatusType.Declined)
                .Select(enrollee => enrollee.GPID)
                .DecompileAsync()
                .SingleOrDefaultAsync();
        }

        public async Task<EnrolleeViewModel> GetEnrolleeAsync(int enrolleeId)
        {
            var newestAgreementIds = _context.AgreementVersions
                .Select(a => a.AgreementType)
                .Distinct()
                .Select(type => _context.AgreementVersions
                    .OrderByDescending(a => a.EffectiveDate)
                    .First(a => a.AgreementType == type)
                    .Id
                );

            var unlinkedPaperEnrolments = _context.Enrollees
                .Where(e => e.GPID.StartsWith(Enrollee.PaperGpidPrefix)
                    && !_context.EnrolleeLinkedEnrolments
                        .Any(link => link.PaperEnrolleeId == e.Id));

            var dto = await _context.Enrollees
                .AsNoTracking()
                .Where(e => e.Id == enrolleeId)
                .ProjectTo<EnrolleeDTO>(_mapper.ConfigurationProvider, new { newestAgreementIds, unlinkedPaperEnrolments })
                .DecompileAsync()
                .SingleOrDefaultAsync();

            var linkedGpid = await _context.EnrolleeLinkedEnrolments
                .Where(ele => ele.EnrolleeId == enrolleeId
                    && ele.PaperEnrolleeId.HasValue)
                .Select(ele => ele.UserProvidedGpid)
                .SingleOrDefaultAsync();

            dto.UserProvidedGpid = linkedGpid;
            return _mapper.Map<EnrolleeViewModel>(dto);
        }

        public async Task<PaginatedList<EnrolleeListViewModel>> GetEnrolleesAsync(EnrolleeSearchOptions searchOptions = null, ClaimsPrincipal user = null)
        {
            searchOptions ??= new EnrolleeSearchOptions();

            IQueryable<int> newestAgreementIds = _context.AgreementVersions
                .Select(a => a.AgreementType)
                .Distinct()
                .Select(type => _context.AgreementVersions
                    .OrderByDescending(a => a.EffectiveDate)
                    .First(a => a.AgreementType == type)
                    .Id
                );

            var unlinkedPaperEnrolments = _context.Enrollees
                .Where(e => e.GPID.StartsWith(Enrollee.PaperGpidPrefix)
                    && !_context.EnrolleeLinkedEnrolments
                        .Any(link => link.PaperEnrolleeId == e.Id));

            var query = _context.Enrollees
                .AsNoTracking()
                .If(!string.IsNullOrWhiteSpace(searchOptions.TextSearch), q => q
                    .Search(e => e.FirstName,
                        e => e.LastName,
                        e => e.FullName,
                        e => e.Email,
                        e => e.Phone,
                        e => e.DisplayId.ToString())
                    .SearchCollections(e => e.Certifications.Select(c => c.LicenseNumber))
                    .Containing(searchOptions.TextSearch)
                )
                .If(searchOptions.StatusCode.HasValue, q => q
                    .Where(e => e.CurrentStatus.StatusCode == searchOptions.StatusCode.Value)
                )
                .If(searchOptions.IsRenewedManualEnrolment == true, q => q
                    .Where(e => e.CurrentStatus.StatusCode == (int)StatusType.UnderReview
                        && e.EnrolmentStatuses
                            .Any(es => es.EnrolmentStatusReasons
                                .Any(esr => esr.StatusReasonCode == (int)StatusReasonType.Manual)))
                )
                .If(searchOptions.IsLinkedPaperEnrolment == true, q => q
                    .Where(e => _context.EnrolleeLinkedEnrolments.Any(link => link.PaperEnrolleeId == e.Id))
                )
                .If(searchOptions.IsLinkedPaperEnrolment == false, q => q
                    .Where(e => e.GPID.StartsWith(Enrollee.PaperGpidPrefix)
                        && !_context.EnrolleeLinkedEnrolments.Any(link => link.PaperEnrolleeId == e.Id))
                )
                .If(!string.IsNullOrWhiteSpace(searchOptions.AssignedTo), q => q
                    .Where(e => e.Adjudicator.IDIR == searchOptions.AssignedTo)
                )
                .If(searchOptions.RenewalDateRangeStart.HasValue && searchOptions.RenewalDateRangeEnd.HasValue, q => q
                    .Where(e => e.ExpiryDate != null
                        && e.ExpiryDate >= searchOptions.RenewalDateRangeStart
                        && e.ExpiryDate <= searchOptions.RenewalDateRangeEnd
                    )
                )
                .If(searchOptions.AppliedDateRangeStart.HasValue && searchOptions.AppliedDateRangeEnd.HasValue, q => q
                    .Where(e => e.AppliedDate != null
                        && e.AppliedDate >= searchOptions.AppliedDateRangeStart
                        && e.AppliedDate <= searchOptions.AppliedDateRangeEnd
                    )
                )
                // By default postgres treats NULL values as infinitely large, where as angular material table sort does the opposite.
                // Added the first OrderBy to maintain NULL being infinitely small.
                .If(!string.IsNullOrWhiteSpace(searchOptions.SortOrder), q =>
                        searchOptions.SortOrder switch
                        {
                            "renewalDate_asc" => q.OrderByDescending(e => e.ExpiryDate.HasValue).ThenBy(e => e.ExpiryDate).ThenBy(e => e.Id),
                            "renewalDate_desc" => q.OrderBy(e => e.ExpiryDate.HasValue).ThenByDescending(e => e.ExpiryDate).ThenBy(e => e.Id),
                            "appliedDate_asc" => q.OrderByDescending(e => e.AppliedDate.HasValue).ThenBy(e => e.AppliedDate).ThenBy(e => e.Id),
                            "appliedDate_desc" => q.OrderBy(e => e.AppliedDate.HasValue).ThenByDescending(e => e.AppliedDate).ThenBy(e => e.Id),
                            "displayId_desc" => q.OrderByDescending(e => e.Id),
                            _ => q.OrderBy(e => e.Id),
                        }
                )
                // If not in role of `ViewEnrollee`, filter/restrict to Claimed and Unclaimed Paper Enrolments
                // (same as logic above)
                .If(user != null && !user.IsInRole(Roles.ViewEnrollee), q => q
                    .Where(e => _context.EnrolleeLinkedEnrolments.Any(link => link.PaperEnrolleeId == e.Id) ||
                                (e.GPID.StartsWith(Enrollee.PaperGpidPrefix)
                                    && !_context.EnrolleeLinkedEnrolments.Any(link => link.PaperEnrolleeId == e.Id)))
                )
                .ProjectTo<EnrolleeListViewModel>(_mapper.ConfigurationProvider, new { newestAgreementIds, unlinkedPaperEnrolments })
                .DecompileAsync();

            return await PaginatedList<EnrolleeListViewModel>.CreateAsync(query, searchOptions.Page ?? 1);
        }

        public async Task<EnrolleeNavigation> GetAdjacentEnrolleeIdAsync(int enrolleeId)
        {
            var nextId = await _context.Enrollees
                .Where(e => e.Id > enrolleeId)
                .OrderBy(e => e.Id)
                .Select(e => e.Id)
                .FirstOrDefaultAsync();

            var previousId = await _context.Enrollees
                .Where(e => e.Id < enrolleeId)
                .OrderByDescending(e => e.Id)
                .Select(e => e.Id)
                .FirstOrDefaultAsync();

            return new EnrolleeNavigation { NextId = nextId, PreviousId = previousId };
        }

        public async Task<int> CreateEnrolleeAsync(EnrolleeCreateModel createModel)
        {
            createModel.ThrowIfNull(nameof(createModel));

            var enrollee = _mapper.Map<Enrollee>(createModel);
            enrollee.Addresses = new List<EnrolleeAddress>();

            UpdateAddress(enrollee, createModel.MailingAddress);
            UpdateAddress(enrollee, createModel.PhysicalAddress);
            UpdateAddress(enrollee, createModel.VerifiedAddress);

            enrollee.AddEnrolmentStatus(StatusType.Editable);
            _context.Enrollees.Add(enrollee);

            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Could not create enrollee.");
            }

            await _businessEventService.CreateEnrolleeEventAsync(enrollee.Id, "Enrollee Created");

            return enrollee.Id;
        }

        public async Task<int> UpdateEnrolleeAsync(int enrolleeId, EnrolleeUpdateModel updateModel, bool profileCompleted = false)
        {
            var enrollee = await _context.Enrollees
                .Include(e => e.Addresses)
                    .ThenInclude(ea => ea.Address)
                .Include(e => e.Certifications)
                .Include(e => e.EnrolleeRemoteUsers)
                .Include(e => e.RemoteAccessSites)
                .Include(e => e.RemoteAccessLocations)
                    .ThenInclude(ral => ral.PhysicalAddress)
                .Include(e => e.EnrolleeCareSettings)
                .Include(e => e.EnrolleeHealthAuthorities)
                .Include(e => e.SelfDeclarations)
                .Include(e => e.OboSites)
                    .ThenInclude(s => s.PhysicalAddress)
                .Include(e => e.SelfDeclarationDocuments)
                .SingleAsync(e => e.Id == enrolleeId);

            _context.Entry(enrollee).CurrentValues.SetValues(updateModel);

            // TODO currently doesn't update the date of birth
            if (enrollee.IdentityProvider != AuthConstants.BCServicesCard)
            {
                enrollee.FirstName = updateModel.PreferredFirstName;
                enrollee.LastName = updateModel.PreferredLastName;
                enrollee.GivenNames = $"{updateModel.PreferredFirstName} {updateModel.PreferredMiddleName}";
            }

            UpdateAddress(enrollee, updateModel.PhysicalAddress);
            UpdateAddress(enrollee, updateModel.MailingAddress);
            UpdateAddress(enrollee, updateModel.VerifiedAddress);
            ReplaceExistingItems(enrollee.Certifications, updateModel.Certifications, enrolleeId);
            ReplaceExistingItems(enrollee.EnrolleeCareSettings, updateModel.EnrolleeCareSettings, enrolleeId);
            ReplaceExistingItems(enrollee.SelfDeclarations, updateModel.SelfDeclarations, enrolleeId);
            ReplaceExistingItems(enrollee.EnrolleeHealthAuthorities, updateModel.EnrolleeHealthAuthorities, enrolleeId);

            UpdateEnrolleeRemoteUsers(enrollee, updateModel);
            UpdateRemoteAccessSites(enrollee, updateModel);
            UpdateRemoteAccessLocations(enrollee, updateModel);

            UpdateOboSites(enrollee, updateModel);

            // If profileCompleted is true, this is the first time the enrollee
            // has completed their profile by traversing the wizard, and indicates
            // a change in routing for the enrollee
            if (profileCompleted)
            {
                enrollee.ProfileCompleted = true;
            }

            // This is the temporary way we are adding self declaration documents until this gets refactored.
            await CreateSelfDeclarationDocuments(enrolleeId, updateModel.SelfDeclarations, enrollee.SelfDeclarationDocuments);

            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }
        }

        private void UpdateAddress<T>(Enrollee dbEnrollee, T newAddress) where T : Address
        {
            var existingEnrolleeAddress = dbEnrollee.Addresses
                .SingleOrDefault(ea => ea.Address is T);

            if (existingEnrolleeAddress == null)
            {
                if (newAddress == null)
                {
                    // Noop
                    return;
                }
                else
                {
                    // New
                    newAddress.Id = 0;
                    dbEnrollee.Addresses.Add(new EnrolleeAddress
                    {
                        Enrollee = dbEnrollee,
                        Address = newAddress
                    });
                }
            }
            else
            {
                if (newAddress == null)
                {
                    // Remove
                    _context.Remove(existingEnrolleeAddress.Address);
                    _context.Remove(existingEnrolleeAddress);
                    return;
                }
                else
                {
                    // Update
                    newAddress.Id = existingEnrolleeAddress.AddressId;
                    _context.Entry(existingEnrolleeAddress.Address).CurrentValues.SetValues(newAddress);
                }
            }
        }

        private void ReplaceExistingItems<T>(ICollection<T> dbCollection, ICollection<T> newCollection, int enrolleeId) where T : class, IEnrolleeNavigationProperty
        {
            // Remove existing items
            foreach (var item in dbCollection)
            {
                _context.Remove(item);
            }

            if (newCollection == null)
            {
                return;
            }

            // Create new items
            foreach (var item in newCollection)
            {
                // Prevent the ID from being changed by the incoming changes
                item.EnrolleeId = enrolleeId;
                _context.Entry(item).State = EntityState.Added;
            }
        }

        private void UpdateRemoteAccessLocations(Enrollee dbEnrollee, EnrolleeUpdateModel updateEnrollee)
        {
            // Wholesale replace the remote access locations
            foreach (var location in dbEnrollee.RemoteAccessLocations)
            {
                _context.Remove(location.PhysicalAddress);
                _context.Remove(location);
            }

            if (updateEnrollee.RemoteAccessLocations == null || !updateEnrollee.RemoteAccessLocations.Any())
            {
                return;
            }

            var remoteAccessLocations = new List<RemoteAccessLocation>();

            foreach (var location in updateEnrollee.RemoteAccessLocations)
            {
                var newAddress = new PhysicalAddress
                {
                    CountryCode = location.PhysicalAddress.CountryCode,
                    ProvinceCode = location.PhysicalAddress.ProvinceCode,
                    Street = location.PhysicalAddress.Street,
                    Street2 = location.PhysicalAddress.Street2,
                    City = location.PhysicalAddress.City,
                    Postal = location.PhysicalAddress.Postal
                };
                var newLocation = new RemoteAccessLocation
                {
                    Enrollee = dbEnrollee,
                    InternetProvider = location.InternetProvider,
                    PhysicalAddress = newAddress
                };
                _context.Entry(newAddress).State = EntityState.Added;
                _context.Entry(newLocation).State = EntityState.Added;
                remoteAccessLocations.Add(newLocation);
            }

            updateEnrollee.RemoteAccessLocations = remoteAccessLocations;
        }

        private void UpdateEnrolleeRemoteUsers(Enrollee dbEnrollee, EnrolleeUpdateModel updateEnrollee)
        {
            if (dbEnrollee.EnrolleeRemoteUsers != null)
            {
                foreach (var eru in dbEnrollee.EnrolleeRemoteUsers)
                {
                    _context.EnrolleeRemoteUsers.Remove(eru);
                }
            }

            if (updateEnrollee.EnrolleeRemoteUsers != null)
            {
                foreach (var eru in updateEnrollee.EnrolleeRemoteUsers)
                {
                    eru.EnrolleeId = dbEnrollee.Id;
                    _context.Entry(eru).State = EntityState.Added;
                }
            }
        }

        private void UpdateRemoteAccessSites(Enrollee dbEnrollee, EnrolleeUpdateModel updateEnrollee)
        {
            if (dbEnrollee.RemoteAccessSites != null)
            {
                foreach (var ras in dbEnrollee.RemoteAccessSites)
                {
                    _context.RemoteAccessSites.Remove(ras);
                }
            }

            if (updateEnrollee.RemoteAccessSites != null)
            {
                foreach (var ras in updateEnrollee.RemoteAccessSites)
                {
                    var remoteAccessSite = new RemoteAccessSite
                    {
                        EnrolleeId = dbEnrollee.Id,
                        SiteId = ras.SiteId
                    };

                    _context.Entry(remoteAccessSite).State = EntityState.Added;
                }
            }
        }

        private void UpdateOboSites(Enrollee dbEnrollee, EnrolleeUpdateModel updateEnrollee)
        {
            // Wholesale replace the obo sites
            foreach (var site in dbEnrollee.OboSites)
            {
                _context.Remove(site.PhysicalAddress);
                _context.Remove(site);
            }

            if (updateEnrollee?.OboSites != null && updateEnrollee?.OboSites.Count() != 0)
            {
                var oboSites = new List<OboSite>();

                foreach (var site in updateEnrollee.OboSites)
                {
                    var newAddress = new PhysicalAddress
                    {
                        CountryCode = site.PhysicalAddress.CountryCode,
                        ProvinceCode = site.PhysicalAddress.ProvinceCode,
                        Street = site.PhysicalAddress.Street,
                        Street2 = site.PhysicalAddress.Street2,
                        City = site.PhysicalAddress.City,
                        Postal = site.PhysicalAddress.Postal
                    };
                    var newSite = new OboSite
                    {
                        Enrollee = dbEnrollee,
                        CareSettingCode = site.CareSettingCode,
                        HealthAuthorityCode = site.HealthAuthorityCode,
                        PhysicalAddress = newAddress,
                        SiteName = site.SiteName,
                        FacilityName = site.FacilityName,
                        JobTitle = site.JobTitle
                    };
                    _context.Entry(newAddress).State = EntityState.Added;
                    _context.Entry(newSite).State = EntityState.Added;
                    oboSites.Add(newSite);
                }
                updateEnrollee.OboSites = oboSites;
            }
        }

        private async Task CreateSelfDeclarationDocuments(int enrolleeId, ICollection<SelfDeclaration> newDeclarations, IEnumerable<SelfDeclarationDocument> currentSelfDeclarationDocuments)
        {
            if (newDeclarations == null)
            {
                return;
            }

            foreach (var declaration in newDeclarations.Where(d => d.DocumentGuids != null))
            {
                foreach (var documentGuid in declaration.DocumentGuids)
                {
                    var filename = await _documentClient.FinalizeUploadAsync(documentGuid, DestinationFolders.SelfDeclarations);
                    if (string.IsNullOrWhiteSpace(filename))
                    {
                        throw new InvalidOperationException($"Could not find a document upload with GUID {documentGuid}");
                    }

                    _context.SelfDeclarationDocuments.Add(new SelfDeclarationDocument
                    {
                        EnrolleeId = enrolleeId,
                        SelfDeclarationTypeCode = declaration.SelfDeclarationTypeCode,
                        DocumentGuid = documentGuid,
                        Filename = filename,
                        UploadedDate = DateTimeOffset.Now
                    });
                }
            }

            foreach (var currentDocument in currentSelfDeclarationDocuments)
            {
                if (!newDeclarations.Any(newDocument => newDocument.SelfDeclarationTypeCode == currentDocument.SelfDeclarationTypeCode))
                {
                    currentDocument.Hidden = true;
                }
            }
        }

        public async Task DeleteEnrolleeAsync(int enrolleeId)
        {
            var enrollee = await _context.Enrollees
                .Include(e => e.EnrolmentStatuses)
                    .ThenInclude(es => es.EnrolmentStatusReference)
                .SingleOrDefaultAsync(e => e.Id == enrolleeId);

            if (enrollee == null)
            {
                return;
            }

            _context.Enrollees.Remove(enrollee);
            await _context.SaveChangesAsync();
        }

        public async Task<AccessAgreementNoteViewModel> GetAccessAgreementNoteAsync(int enrolleeId)
        {
            return await _context.Set<AccessAgreementNote>()
                .ProjectTo<AccessAgreementNoteViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(a => a.EnrolleeId == enrolleeId);
        }

        public async Task<CareSettingViewModel> GetCareSettingsAsync(int enrolleeId)
        {
            return await _context.Enrollees
                .Where(e => e.Id == enrolleeId)
                .Select(e => new CareSettingViewModel
                {
                    EnrolleeCareSettings = e.EnrolleeCareSettings,
                    EnrolleeHealthAuthorities = e.EnrolleeHealthAuthorities
                })
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CertificationViewModel>> GetCertificationsAsync(int enrolleeId)
        {
            return await _context.Set<Certification>()
                .Where(c => c.EnrolleeId == enrolleeId)
                .ProjectTo<CertificationViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<EnrolleeRemoteUserViewModel>> GetEnrolleeRemoteUsersAsync(int enrolleeId)
        {
            return await _context.EnrolleeRemoteUsers
                .Where(eru => eru.EnrolleeId == enrolleeId)
                .ProjectTo<EnrolleeRemoteUserViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<OboSiteViewModel>> GetOboSitesAsync(int enrolleeId)
        {
            return await _context.Set<OboSite>()
                .AsNoTracking()
                .Where(os => os.EnrolleeId == enrolleeId)
                .ProjectTo<OboSiteViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<RemoteAccessLocationViewModel>> GetRemoteAccessLocationsAsync(int enrolleeId)
        {
            return await _context.Set<RemoteAccessLocation>()
                .AsNoTracking()
                .Where(ral => ral.EnrolleeId == enrolleeId)
                .ProjectTo<RemoteAccessLocationViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<RemoteAccessSiteViewModel>> GetRemoteAccessSitesAsync(int enrolleeId)
        {
            // Currently, only maps from Community Sites as Remote Users are disabled on Health Authorities
            return await _context.RemoteAccessSites
                .AsNoTracking()
                .Where(ras => ras.EnrolleeId == enrolleeId)
                .ProjectTo<RemoteAccessSiteViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a View Model for each Self Declaration Question, including ones answered "No"
        /// </summary>
        /// <param name="enrolleeId"></param>
        public async Task<IEnumerable<SelfDeclarationViewModel>> GetSelfDeclarationsAsync(int enrolleeId)
        {
            var answered = await _context.Set<SelfDeclaration>()
                .Where(sd => sd.EnrolleeId == enrolleeId)
                .ProjectTo<SelfDeclarationViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var unAnswered = Enum.GetValues(typeof(SelfDeclarationTypeCode))
                .Cast<int>()
                .Except(answered.Select(a => a.SelfDeclarationTypeCode))
                .Select(code => new SelfDeclarationViewModel
                {
                    EnrolleeId = enrolleeId,
                    SelfDeclarationTypeCode = code
                });

            return answered.Concat(unAnswered);
        }

        public async Task<IEnumerable<SelfDeclarationDocumentViewModel>> GetSelfDeclarationDocumentsAsync(int enrolleeId, bool includeHidden = true)
        {
            return await _context.SelfDeclarationDocuments
                .Where(sdd => sdd.EnrolleeId == enrolleeId)
                .If(!includeHidden, q => q.Where(sdd => !sdd.Hidden))
                .ProjectTo<SelfDeclarationDocumentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task AssignToaAgreementType(int enrolleeId, AgreementType? agreementType)
        {
            var submission = await _context.Submissions
                .OrderByDescending(s => s.CreatedDate)
                .FirstOrDefaultAsync(e => e.EnrolleeId == enrolleeId);

            submission.AgreementType = agreementType;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EnrolmentStatusAdminViewModel>> GetEnrolmentStatusesAsync(int enrolleeId)
        {
            return await _context.EnrolmentStatuses
                .Where(es => es.EnrolleeId == enrolleeId)
                .OrderByDescending(es => es.StatusDate)
                    .ThenByDescending(s => s.Id)
                .ProjectTo<EnrolmentStatusAdminViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> IsEnrolleeInStatusAsync(int enrolleeId, params StatusType[] statusCodesToCheck)
        {
            var enrollee = await _context.Enrollees
                .AsNoTracking()
                .Include(e => e.EnrolmentStatuses)
                .SingleOrDefaultAsync(e => e.Id == enrolleeId);

            if (enrollee == null || enrollee.CurrentStatus == null)
            {
                return false;
            }

            return statusCodesToCheck.Contains(enrollee.CurrentStatus.GetStatusType());
        }

        public async Task<IEnumerable<EnrolleeNoteViewModel>> GetEnrolleeAdjudicatorNotesAsync(int enrolleeId)
        {
            return await _context.EnrolleeNotes
                .Where(an => an.EnrolleeId == enrolleeId)
                .Include(an => an.Adjudicator)
                .OrderByDescending(an => an.NoteDate)
                .ProjectTo<EnrolleeNoteViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EnrolleeNoteViewModel> GetEnrolleeAdjudicatorNoteAsync(int enrolleeId, int noteId)
        {
            return await _context.EnrolleeNotes
                .Where(an => an.EnrolleeId == enrolleeId)
                .Include(an => an.Adjudicator)
                .Include(an => an.EnrolleeNotification)
                    .ThenInclude(ee => ee.Admin)
                .ProjectTo<EnrolleeNoteViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(n => n.Id == noteId);
        }

        public async Task<IEnumerable<EnrolleeNoteViewModel>> GetNotificationsAsync(int enrolleeId, int adminId)
        {
            return await _context.EnrolleeNotes
                .Include(an => an.Adjudicator)
                .Include(an => an.EnrolleeNotification)
                    .ThenInclude(ee => ee.Admin)
                .Where(an => an.EnrolleeId == enrolleeId)
                .Where(an => an.EnrolleeNotification != null && an.EnrolleeNotification.AssigneeId == adminId)
                .ProjectTo<EnrolleeNoteViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EnrolleeNote> CreateEnrolleeAdjudicatorNoteAsync(int enrolleeId, string note, int adminId)
        {
            var adjudicatorNote = new EnrolleeNote
            {
                EnrolleeId = enrolleeId,
                AdjudicatorId = adminId,
                Note = note,
                NoteDate = DateTimeOffset.Now
            };

            _context.EnrolleeNotes.Add(adjudicatorNote);

            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Could not create adjudicator note.");
            }
            else
            {
                await _businessEventService.CreateNoteEventAsync(enrolleeId, "Added Adjudicator Note: " + note);
            }

            return adjudicatorNote;
        }

        public async Task<EnrolmentStatusReference> CreateEnrolmentStatusReferenceAsync(int statusId, int adminId)
        {
            var reference = new EnrolmentStatusReference
            {
                EnrolmentStatusId = statusId,
                AdminId = adminId,
            };

            _context.EnrolmentStatusReference.Add(reference);

            await _context.SaveChangesAsync();

            return reference;
        }

        public async Task<EnrolleeNotification> CreateEnrolleeNotificationAsync(int EnrolleeNoteId, int adminId, int assigneeId)
        {
            var notification = new EnrolleeNotification
            {
                EnrolleeNoteId = EnrolleeNoteId,
                AdminId = adminId,
                AssigneeId = assigneeId,
            };

            _context.EnrolleeNotifications.Add(notification);

            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task RemoveEnrolleeNotificationAsync(int enrolleeNotificationId)
        {
            var notification = await _context.EnrolleeNotifications
                .SingleOrDefaultAsync(ee => ee.Id == enrolleeNotificationId);
            if (notification == null)
            {
                return;
            }
            _context.EnrolleeNotifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveNotificationsAsync(int enrolleeId)
        {
            var notifications = await _context.EnrolleeNotifications
                .Where(en => en.EnrolleeNote.EnrolleeId == enrolleeId)
                .ToListAsync();

            _context.EnrolleeNotifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }

        public async Task<EnrolleeNotification> GetEnrolleeNotificationAsync(int enrolleeNotificationId)
        {
            return await _context.EnrolleeNotifications
                .SingleOrDefaultAsync(ee => ee.Id == enrolleeNotificationId);
        }

        public async Task<EnrolmentStatusReference> AddAdjudicatorNoteToReferenceIdAsync(int statusId, int noteId)
        {
            var reference = await _context.EnrolmentStatusReference.Where(esr => esr.EnrolmentStatusId == statusId).SingleAsync();

            reference.AdjudicatorNoteId = noteId;

            _context.EnrolmentStatusReference.Update(reference);

            await _context.SaveChangesAsync();

            return reference;
        }

        public async Task<IBaseEnrolleeNote> UpdateEnrolleeNoteAsync(int enrolleeId, int adminId, IBaseEnrolleeNote newNote)
        {
            var enrollee = await _context.Enrollees
                .Include(e => e.AccessAgreementNote)
                .Where(e => e.Id == enrolleeId)
                .SingleOrDefaultAsync();

            IBaseEnrolleeNote dbNote = null;

            if (newNote.GetType() == typeof(AccessAgreementNote))
            {
                dbNote = enrollee.AccessAgreementNote;
            }
            else
            {
                throw new ArgumentException("Enrollee note type is not recognized, or not allowed.");
            }

            if (dbNote != null)
            {
                if (newNote.Note == null)
                {
                    _context.Remove(dbNote);
                }
                else
                {
                    dbNote.Note = newNote.Note;
                    dbNote.NoteDate = DateTimeOffset.Now;
                    _context.Update(dbNote);
                }
            }
            else if (newNote != null)
            {
                newNote.EnrolleeId = enrolleeId;
                // Know instance of AccessAgreementNote
                ((AccessAgreementNote)newNote).AdjudicatorId = adminId;
                newNote.NoteDate = DateTimeOffset.Now;
                _context.Add(newNote);
            }

            var updated = await _context.SaveChangesAsync();
            if (updated < 1)
            {
                throw new InvalidOperationException($"Could not update the enrollee note.");
            }
            else
            {
                await _businessEventService.CreateNoteEventAsync(enrolleeId, "Updated Limits and Conditions Note: " + newNote.Note);
            }

            return newNote;
        }

        public async Task UpdateEnrolleeAdjudicator(int enrolleeId, int? adminId = null)
        {
            var enrollee = await _context.Enrollees
                .Where(e => e.Id == enrolleeId)
                .SingleAsync();

            enrollee.AdjudicatorId = adminId;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BusinessEvent>> GetEnrolleeBusinessEventsAsync(int enrolleeId, IEnumerable<int> businessEventTypeCodes)
        {
            var linkedPaperEnrolleeId = await GetLinkedPaperEnrolleeId(enrolleeId);

            return await _context.BusinessEvents
                .Include(e => e.Admin)
                .Where(e => e.EnrolleeId == enrolleeId
                    || linkedPaperEnrolleeId.HasValue && e.EnrolleeId == linkedPaperEnrolleeId)
                .Where(e => businessEventTypeCodes.Any(c => c == e.BusinessEventTypeCode))
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();
        }

        private async Task<int?> GetLinkedPaperEnrolleeId(int enrolleeId)
        {
            return await _context.EnrolleeLinkedEnrolments
                .Where(ele => ele.EnrolleeId == enrolleeId)
                .Select(ele => ele.PaperEnrolleeId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<HpdidLookup>> HpdidLookupAsync(IEnumerable<string> hpdids)
        {
            hpdids.ThrowIfNull(nameof(hpdids));

            hpdids = hpdids.Where(h => !string.IsNullOrWhiteSpace(h));

            return await _context.Enrollees
                .Where(e => hpdids.Contains(e.HPDID))
                .Where(e => e.CurrentStatus.StatusCode != (int)StatusType.Declined)
                .Select(e => new HpdidLookup
                {
                    Gpid = e.GPID,
                    Hpdid = e.HPDID,
                    RenewalDate = e.ExpiryDate
                })
                .DecompileAsync()
                .ToListAsync();
        }

        public async Task<GpidValidationResponse> ValidateProvisionerDataAsync(string gpid, GpidValidationParameters parameters)
        {
            var enrollee = await _context.Enrollees
                .Include(e => e.Certifications)
                    .ThenInclude(c => c.License)
                .SingleOrDefaultAsync(e => e.GPID == gpid);

            if (enrollee == null)
            {
                return null;
            }

            return parameters.ValidateAgainst(enrollee);
        }

        public async Task<SelfDeclarationDocument> AddSelfDeclarationDocumentAsync(int enrolleeId, SelfDeclarationDocument selfDeclarationDocument)
        {
            selfDeclarationDocument.EnrolleeId = enrolleeId;
            selfDeclarationDocument.UploadedDate = DateTimeOffset.Now;

            _context.SelfDeclarationDocuments.Add(selfDeclarationDocument);

            var updated = await _context.SaveChangesAsync();
            if (updated < 1)
            {
                throw new InvalidOperationException($"Could not add Self Declaration Documents.");
            }

            return selfDeclarationDocument;
        }

        public async Task<IdentificationDocument> CreateIdentificationDocument(int enrolleeId, Guid documentGuid, string filename)
        {
            var identificationDocument = new IdentificationDocument
            {
                DocumentGuid = documentGuid,
                EnrolleeId = enrolleeId,
                Filename = filename,
                UploadedDate = DateTimeOffset.Now
            };
            _context.IdentificationDocuments.Add(identificationDocument);

            await _context.SaveChangesAsync();

            return identificationDocument;
        }

        public async Task<EnrolleeAdjudicationDocument> AddEnrolleeAdjudicationDocumentAsync(int enrolleeId, Guid documentGuid, int adminId)
        {
            var filename = await _documentClient.FinalizeUploadAsync(documentGuid, DestinationFolders.EnrolleeAdjudicationDocuments);
            if (string.IsNullOrWhiteSpace(filename))
            {
                return null;
            }

            var document = new EnrolleeAdjudicationDocument
            {
                DocumentGuid = documentGuid,
                EnrolleeId = enrolleeId,
                Filename = filename,
                UploadedDate = DateTimeOffset.Now,
                AdjudicatorId = adminId
            };
            _context.EnrolleeAdjudicationDocuments.Add(document);

            await _context.SaveChangesAsync();

            return document;
        }

        public async Task<IEnumerable<EnrolleeAdjudicationDocument>> GetEnrolleeAdjudicationDocumentsAsync(int enrolleeId)
        {
            return await _context.EnrolleeAdjudicationDocuments
               .Where(bl => bl.EnrolleeId == enrolleeId)
               .Include(bl => bl.Adjudicator)
                .OrderByDescending(bl => bl.UploadedDate)
               .ToListAsync();
        }

        public async Task<EnrolleeAdjudicationDocument> GetEnrolleeAdjudicationDocumentAsync(int documentId)
        {
            return await _context.EnrolleeAdjudicationDocuments
               .SingleOrDefaultAsync(d => d.Id == documentId);
        }

        public async Task DeleteEnrolleeAdjudicationDocumentAsync(int documentId)
        {
            var document = await _context.EnrolleeAdjudicationDocuments
                .SingleOrDefaultAsync(d => d.Id == documentId);
            if (document == null)
            {
                return;
            }
            _context.EnrolleeAdjudicationDocuments.Remove(document);
            await _context.SaveChangesAsync();
        }

        public async Task<EnrolmentStatusAdminViewModel> GetEnrolleeCurrentStatusAsync(int enrolleeId)
        {
            return await _context.Enrollees
                .AsNoTracking()
                .Where(enrollee => enrollee.Id == enrolleeId)
                .Select(enrollee => enrollee.CurrentStatus)
                .ProjectTo<EnrolmentStatusAdminViewModel>(_mapper.ConfigurationProvider)
                .DecompileAsync()
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<int>> GetNotifiedEnrolleeIdsForAdminAsync(ClaimsPrincipal user)
        {
            return await _context.EnrolleeNotes
                .Where(en => en.EnrolleeNotification != null && en.EnrolleeNotification.Assignee.UserId == user.GetPrimeUserId())
                .Select(en => en.EnrolleeId)
                .ToListAsync();
        }

        public async Task<Credential> GetCredentialAsync(int enrolleeId)
        {
            return await _context.Credentials
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync(c => c.EnrolleeId == enrolleeId);
        }

        public async Task<IEnumerable<string>> GetEnrolleeEmails(BulkEmailType bulkEmailType)
        {
            Expression<Func<Enrollee, bool>> predicate = bulkEmailType switch
            {
                BulkEmailType.CommunityPractice => e => e.EnrolleeCareSettings.Any(cs => cs.CareSettingCode == (int)CareSettingType.CommunityPractice),
                BulkEmailType.CommunityPharmacy => e => e.EnrolleeCareSettings.Any(cs => cs.CareSettingCode == (int)CareSettingType.CommunityPharmacy),
                BulkEmailType.HealthAuthority => e => e.EnrolleeCareSettings.Any(cs => cs.CareSettingCode == (int)CareSettingType.HealthAuthority),
                BulkEmailType.RequiresTOA => e => e.CurrentStatus.StatusCode == (int)StatusType.RequiresToa,
                BulkEmailType.RuTOA => e => e.Agreements.OrderByDescending(a => a.AcceptedDate).FirstOrDefault().AgreementVersion.AgreementType == AgreementType.RegulatedUserTOA,
                BulkEmailType.OboTOA => e => e.Agreements.OrderByDescending(a => a.AcceptedDate).FirstOrDefault().AgreementVersion.AgreementType == AgreementType.OboTOA,
                BulkEmailType.PharmRuTOA => e => e.Agreements.OrderByDescending(a => a.AcceptedDate).FirstOrDefault().AgreementVersion.AgreementType == AgreementType.CommunityPharmacistTOA,
                BulkEmailType.PharmOboTOA => e => e.Agreements.OrderByDescending(a => a.AcceptedDate).FirstOrDefault().AgreementVersion.AgreementType == AgreementType.PharmacyOboTOA,
                _ => null,
            };

            return await _context.Enrollees
                .AsNoTracking()
                .Where(predicate)
                .Select(e => e.Email)
                .DecompileAsync()
                .ToListAsync();
        }

        public async Task<EnrolleeAbsence> CreateEnrolleeAbsenceAsync(int enrolleeId, EnrolleeAbsenceViewModel createModel)
        {
            var absence = new EnrolleeAbsence
            {
                EnrolleeId = enrolleeId,
                StartTimestamp = createModel.StartTimestamp.ToUniversalTime(),
                EndTimestamp = createModel.EndTimestamp?.ToUniversalTime()
            };

            _context.EnrolleeAbsences.Add(absence);
            await _context.SaveChangesAsync();

            return absence;
        }

        public async Task<IEnumerable<EnrolleeAbsenceViewModel>> GetEnrolleeAbsencesAsync(int enrolleeId, bool includesPast)
        {
            var rightNow = DateTime.UtcNow;
            return await _context.EnrolleeAbsences
                .Where(ea => ea.EnrolleeId == enrolleeId)
                .If(!includesPast,
                    absences => absences.Where(ea => rightNow <= ea.EndTimestamp || ea.EndTimestamp == null)
                )
                .ProjectTo<EnrolleeAbsenceViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EnrolleeAbsenceViewModel> GetCurrentEnrolleeAbsenceAsync(int enrolleeId)
        {
            var rightNow = DateTime.UtcNow;
            return await _context.EnrolleeAbsences
                .Where(ea => ea.EnrolleeId == enrolleeId
                    && ea.StartTimestamp <= rightNow
                    && (rightNow <= ea.EndTimestamp || ea.EndTimestamp == null))
                .ProjectTo<EnrolleeAbsenceViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task EndCurrentEnrolleeAbsenceAsync(int enrolleeId)
        {
            var rightNow = DateTime.UtcNow;
            var absence = await _context.EnrolleeAbsences
                .SingleOrDefaultAsync(ea => ea.EnrolleeId == enrolleeId
                    && ea.StartTimestamp <= rightNow
                    && (rightNow <= ea.EndTimestamp || ea.EndTimestamp == null));

            if (absence != null)
            {
                absence.EndTimestamp = rightNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFutureEnrolleeAbsenceAsync(int enrolleeId, int absenceId)
        {
            var rightNow = DateTime.UtcNow;
            var absence = await _context.EnrolleeAbsences
                .SingleOrDefaultAsync(ea => ea.Id == absenceId
                    && ea.EnrolleeId == enrolleeId
                    && ea.StartTimestamp > rightNow);

            if (absence != null)
            {
                _context.EnrolleeAbsences.Remove(absence);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetAdjudicatorIdirForEnrolleeAsync(int enrolleeId)
        {
            return await _context.Enrollees
                .Where(e => e.Id == enrolleeId)
                .Select(e => e.Adjudicator.IDIR)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateDateOfBirthAsync(int enrolleeId, DateTime dateOfBirth)
        {
            var enrollee = await _context.Enrollees.SingleAsync(e => e.Id == enrolleeId);
            enrollee.DateOfBirth = dateOfBirth;
            await _context.SaveChangesAsync();
        }
    }
}
