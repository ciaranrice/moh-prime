using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Prime.Models;
using Prime.ViewModels.SpecialAuthorityTransformation;

namespace Prime.Services
{
    public interface ISatEnrolmentService
    {
        Task<int> CreateOrUpdateEnrolleeAsync(SatEnrolleeDemographicChangeModel enrollee, ClaimsPrincipal user);

        Task<Party> GetEnrolleeAsync(int satId);

        Task UpdateDemographicsAsync(int satId, SatEnrolleeDemographicChangeModel viewModel, ClaimsPrincipal user);

        Task UpdateCertificationsAsync(int satId, IEnumerable<SatEnrolleeCertificationViewModel> viewModels);
        Task FinalizeEnrolleeAsync(int satId);
    }
}
