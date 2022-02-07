using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prime.Models;

namespace Prime.Configuration.Database
{
    public class LicenseDetailConfiguration : SeededTable<LicenseDetail>
    {
        public static readonly DateTime ImRegUpdate1Date = new(2022, 2, 1, 8, 0, 0, DateTimeKind.Utc);


        public override IEnumerable<LicenseDetail> SeedData
        {
            get
            {
                return new[] {
                    // CPSBC
                    new LicenseDetail { Id = 1,  LicenseCode = 1,  Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 2,  LicenseCode = 2,  Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 5,  LicenseCode = 5,  Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 6,  LicenseCode = 6,  Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 9,  LicenseCode = 9,  Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 8,  LicenseCode = 8,  Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 10, LicenseCode = 10, Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 22, LicenseCode = 22, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 16, LicenseCode = 16, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 7,  LicenseCode = 7,  Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 4,  LicenseCode = 4,  Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 3,  LicenseCode = 3,  Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 17, LicenseCode = 17, Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 12, LicenseCode = 12, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 13, LicenseCode = 13, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 14, LicenseCode = 14, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 15, LicenseCode = 15, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 11, LicenseCode = 11, Prefix = "91", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 23, LicenseCode = 23, Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = false, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 20, LicenseCode = 20, Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = false, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 24, LicenseCode = 24, Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 18, LicenseCode = 18, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 19, LicenseCode = 19, Prefix = "91", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 21, LicenseCode = 21, Prefix = "91", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = false, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 59, LicenseCode = 59, Prefix = "93", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 65, LicenseCode = 65, Prefix = "93", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 66, LicenseCode = 66, Prefix = "93", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 67, LicenseCode = 67, Prefix = "93", Manual = true,  Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },

                    // Pharmacy
                    new LicenseDetail { Id = 25, LicenseCode = 25, Prefix = "P1", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 26, LicenseCode = 26, Prefix = "P1", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 28, LicenseCode = 28, Prefix = "P1", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 27, LicenseCode = 27, Prefix = "P1", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 30, LicenseCode = 30, Prefix = "P1", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 29, LicenseCode = 29, Prefix = "T9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 31, LicenseCode = 31, Prefix = "T9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = false, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 68, LicenseCode = 68, Prefix = "T9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = true,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 70, LicenseCode = 29, Prefix = "T9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 71, LicenseCode = 31, Prefix = "T9", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 72, LicenseCode = 68, Prefix = "T9", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },

                    // Nursing
                    new LicenseDetail { Id = 47, LicenseCode = 47, Prefix = "96", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 48, LicenseCode = 48, Prefix = "96", Manual = true,  Validate = false, NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 51, LicenseCode = 51, Prefix = "96", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 49, LicenseCode = 49, Prefix = "96", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false,                                                EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 32, LicenseCode = 32, Prefix = "R9", Manual = false, Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Optional,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 33, LicenseCode = 33, Prefix = "R9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 39, LicenseCode = 39, Prefix = "R9", Manual = false, Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Optional,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 34, LicenseCode = 34, Prefix = "R9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = false,                                                EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 40, LicenseCode = 40, Prefix = "R9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 35, LicenseCode = 35, Prefix = "R9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Optional,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 36, LicenseCode = 36, Prefix = "R9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 37, LicenseCode = 37, Prefix = "R9", Manual = false, Validate = true,  NamedInImReg = false, LicensedToProvideCare = false,                                                EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 41, LicenseCode = 41, Prefix = "Y9", Manual = false, Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Optional,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 42, LicenseCode = 42, Prefix = "Y9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 45, LicenseCode = 45, Prefix = "Y9", Manual = false, Validate = true,  NamedInImReg = false, LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Optional,  EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 43, LicenseCode = 43, Prefix = "Y9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = false,                                                EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 46, LicenseCode = 46, Prefix = "Y9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 52, LicenseCode = 52, Prefix = "L9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 53, LicenseCode = 53, Prefix = "L9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 55, LicenseCode = 55, Prefix = "L9", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 54, LicenseCode = 54, Prefix = "L9", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = false,                                                EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 60, LicenseCode = 60, Prefix = "98", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 61, LicenseCode = 61, Prefix = "98", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 62, LicenseCode = 62, Prefix = "98", Manual = false, Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 63, LicenseCode = 63, Prefix = "98", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 69, LicenseCode = 69, Prefix = "98", Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = true,                                                 EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    // Nursing IMReg February 1st Updates
                    new LicenseDetail { Id = 73, LicenseCode = 32, Prefix = "R9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 74, LicenseCode = 33, Prefix = "R9", Manual = true,  Validate = false, NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 75, LicenseCode = 34, Prefix = "R9", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false,                                                EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 76, LicenseCode = 39, Prefix = "R9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 77, LicenseCode = 41, Prefix = "Y9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 78, LicenseCode = 42, Prefix = "Y9", Manual = true,  Validate = false, NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 79, LicenseCode = 43, Prefix = "Y9", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false,                                                EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 80, LicenseCode = 45, Prefix = "Y9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 81, LicenseCode = 52, Prefix = "L9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 82, LicenseCode = 53, Prefix = "L9", Manual = true,  Validate = false, NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 83, LicenseCode = 54, Prefix = "L9", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false,                                                EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 84, LicenseCode = 55, Prefix = "L9", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 85, LicenseCode = 60, Prefix = "98", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 86, LicenseCode = 61, Prefix = "98", Manual = true,  Validate = false, NamedInImReg = true,  LicensedToProvideCare = true,                                                 EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 87, LicenseCode = 62, Prefix = "98", Manual = false, Validate = true,  NamedInImReg = true,  LicensedToProvideCare = true,  PrescriberIdType = PrescriberIdType.Mandatory, EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    new LicenseDetail { Id = 88, LicenseCode = 63, Prefix = "98", Manual = true,  Validate = true,  NamedInImReg = true,  LicensedToProvideCare = false,                                                EffectiveDate = ImRegUpdate1Date, CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                    // All other colleges are assigned the "Not Displayed"Type
                    new LicenseDetail { Id = 64, LicenseCode = 64, Prefix = "",   Manual = true,  Validate = false, NamedInImReg = false, LicensedToProvideCare = false, EffectiveDate = SEEDING_DATE.DateTime.ToUniversalTime(), CreatedTimeStamp = SEEDING_DATE, UpdatedTimeStamp = SEEDING_DATE },
                };
            }
        }

        public override void Configure(EntityTypeBuilder<LicenseDetail> builder)
        {
            builder.HasOne(ld => ld.License)
                .WithMany(l => l.LicenseDetails)
                .HasForeignKey(cl => cl.LicenseCode);
            builder.HasData(SeedData);
        }
    }
}
