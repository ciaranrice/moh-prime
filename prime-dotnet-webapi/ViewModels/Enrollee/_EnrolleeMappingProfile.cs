using System.Linq;
using AutoMapper;

using Prime.DTOs.AgreementEngine;
using Prime.Models;

namespace Prime.ViewModels.Profiles
{
    public class EnrolleeMappingProfile : Profile
    {
        public EnrolleeMappingProfile()
        {
            CreateMap<EnrolleeCreateModel, Enrollee>();

            IQueryable<int> newestAgreementIds = null;
            CreateMap<Enrollee, EnrolleeListViewModel>()
                .ForMember(dest => dest.CurrentStatusCode, opt => opt.MapFrom(src => src.CurrentStatus.StatusCode))
                .ForMember(dest => dest.AdjudicatorIdir, opt => opt.MapFrom(src => src.Adjudicator.IDIR))
                .ForMember(dest => dest.HasNewestAgreement, opt => opt.MapFrom(src => newestAgreementIds.Any(n => n == src.CurrentAgreementId)))
                .ForMember(dest => dest.RemoteAccess, opt => opt.MapFrom(src => src.EnrolleeRemoteUsers.Any()))
                .ForMember(dest => dest.CareSettingCodes, opt => opt.MapFrom(src => src.EnrolleeCareSettings.Select(ecs => ecs.CareSettingCode)))
                .ForMember(dest => dest.RequiresConfirmation, opt => opt.MapFrom(src =>
                    !src.Submissions.OrderByDescending(s => s.CreatedDate).FirstOrDefault().Confirmed
                    && src.PreviousStatus.StatusCode == (int)StatusType.RequiresToa
                ))
                .ForMember(dest => dest.Confirmed, opt => opt.MapFrom(src => src.Submissions.OrderByDescending(s => s.CreatedDate).FirstOrDefault().Confirmed));

            CreateMap<Enrollee, EnrolleeViewModel>()
                .ForMember(dest => dest.CurrentStatusCode, opt => opt.MapFrom(src => src.CurrentStatus.StatusCode))
                .ForMember(dest => dest.AdjudicatorIdir, opt => opt.MapFrom(src => src.Adjudicator.IDIR))
                .ForMember(dest => dest.RequiresConfirmation, opt => opt.MapFrom(src =>
                    !src.Submissions.OrderByDescending(s => s.CreatedDate).FirstOrDefault().Confirmed
                    && src.PreviousStatus.StatusCode == (int)StatusType.RequiresToa
                ))
                .ForMember(dest => dest.Confirmed, opt => opt.MapFrom(src => src.Submissions.OrderByDescending(s => s.CreatedDate).FirstOrDefault().Confirmed))
                .AfterMap((src, dest) => dest.IsRegulatedUser = src.IsRegulatedUser());

            CreateMap<EnrolleeNote, EnrolleeNoteViewModel>();

            CreateMap<Enrollee, AgreementEngineDto>()
                .ForMember(dest => dest.CareSettingCodes, opt => opt.MapFrom(src => src.EnrolleeCareSettings.Select(ecs => ecs.CareSettingCode)));
            CreateMap<Certification, CertificationDto>();
        }
    }
}