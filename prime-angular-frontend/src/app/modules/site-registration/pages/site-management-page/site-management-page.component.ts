import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { KeyValue } from '@angular/common';

import { of, Subscription } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';

import { ArrayUtils } from '@lib/utils/array-utils.class';
import { RouteUtils } from '@lib/utils/route-utils.class';
import { DateUtils } from '@lib/utils/date-utils.class';
import { optionalAddressLineItems } from '@lib/models/address.model';
import { SiteStatusType } from '@lib/enums/site-status.enum';
import { ConfigCodePipe } from '@config/config-code.pipe';
import { OrganizationResource } from '@core/resources/organization-resource.service';
import { ConsoleLoggerService } from '@core/services/console-logger.service';
import { SiteResource } from '@core/resources/site-resource.service';
import { UtilsService } from '@core/services/utils.service';
import { OrganizationAgreement, OrganizationAgreementViewModel } from '@shared/models/agreement.model';
import { AgreementType } from '@shared/enums/agreement-type.enum';
import { CareSettingEnum } from '@shared/enums/care-setting.enum';
import { AddressPipe } from '@shared/pipes/address.pipe';
import { FullnamePipe } from '@shared/pipes/fullname.pipe';

import { SiteRoutes } from '@registration/site-registration.routes';
import { Organization } from '@registration/shared/models/organization.model';
import { Site, SiteListViewModel } from '@registration/shared/models/site.model';
import { AuthService } from '@auth/shared/services/auth.service';
import { BcscUser } from '@auth/shared/models/bcsc-user.model';

@Component({
  selector: 'app-site-management-page',
  templateUrl: './site-management-page.component.html',
  styleUrls: ['./site-management-page.component.scss']
})
export class SiteManagementPageComponent implements OnInit {
  public busy: Subscription;
  public organization: Organization;
  public organizationSitesExpiryDates: string[];
  public organizationAgreements: OrganizationAgreementViewModel[];
  public routeUtils: RouteUtils;
  public careSettingCodesPendingTransfer: CareSettingEnum[];

  public AgreementType = AgreementType;
  public CareSettingEnum = CareSettingEnum;
  public SiteRoutes = SiteRoutes;
  public SiteStatusType = SiteStatusType;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private organizationResource: OrganizationResource,
    private siteResource: SiteResource,
    private utilsService: UtilsService,
    private authService: AuthService,
    private logger: ConsoleLoggerService,
    private fullnamePipe: FullnamePipe,
    private addressPipe: AddressPipe,
    private configCodePipe: ConfigCodePipe
  ) {
    this.routeUtils = new RouteUtils(route, router, SiteRoutes.MODULE_PATH);
  }

  public viewOrganization(organization: Organization): void {
    const routePath = (!organization.completed)
      ? [SiteRoutes.ORGANIZATION_SIGNING_AUTHORITY]
      : []; // Defaults to overview
    this.routeUtils.routeRelativeTo([organization.id, ...routePath]);
  }

  public viewAgreement(organization: Organization, organizationAgreement: OrganizationAgreementViewModel): void {
    const request$ = (organizationAgreement?.signedAgreementDocumentGuid)
      ? this.organizationResource.getSignedOrganizationAgreementToken(organization.id, organizationAgreement.id)
        .pipe(
          map((token: string) => this.utilsService.downloadToken(token))
        )
      : this.organizationResource.getOrganizationAgreement(organization.id, organizationAgreement.id, true)
        .pipe(
          map((agreement: OrganizationAgreement) => agreement.agreementContent),
          map((base64: string) => this.utilsService.base64ToBlob(base64)),
          map((blob: Blob) => this.utilsService.downloadDocument(blob, 'Organization-Agreement'))
        );

    this.busy = request$.subscribe();
  }

  public viewSite(organizationId: number, site: SiteListViewModel): void {
    const routePath = (site.completed)
      ? [organizationId, SiteRoutes.SITES, site.id] // Defaults to overview
      : [organizationId, SiteRoutes.SITES, site.id, SiteRoutes.CARE_SETTING];
    this.routeUtils.routeRelativeTo(routePath);
  }

  public viewSiteRemoteUsers(organizationId: number, site: SiteListViewModel): void {
    const routePath = [organizationId, SiteRoutes.SITES, site.id, SiteRoutes.REMOTE_USERS];
    this.routeUtils.routeRelativeTo(routePath);
  }

  public addSite(organizationId: number): void {
    this.createSite(organizationId);
  }

  public getOrganizationProperties(organization: Organization): KeyValue<string, string>[] {
    return [
      { key: 'Signing Authority', value: this.fullnamePipe.transform(organization.signingAuthority) },
      { key: 'Organization Name', value: organization.name },
      ...ArrayUtils.insertIf(organization?.doingBusinessAs, { key: 'Doing Business As', value: organization.doingBusinessAs })
    ];
  }

  public getSiteProperties(site: SiteListViewModel): KeyValue<string, string>[] {
    // TODO update API to only provide a single vendor
    //      initially only as VM change, then API update
    const siteVendorCode = (Array.isArray(site.siteVendors) && site.siteVendors.length)
      ? site.siteVendors[0].vendorCode
      : null;
    return [
      ...ArrayUtils.insertIf(site.doingBusinessAs, { key: 'Doing Business As', value: site.doingBusinessAs }),
      { key: 'Care Setting', value: this.configCodePipe.transform(site.careSettingCode, 'careSettings') },
      {
        key: 'Site Address',
        value: this.addressPipe.transform(site.physicalAddress, [...optionalAddressLineItems, 'provinceCode', 'countryCode'])
      },
      { key: 'Vendor', value: this.configCodePipe.transform(siteVendorCode, 'vendors') }
    ];
  }

  public getNotSubmittedSiteNotificationProperties(organizationId: number, site: SiteListViewModel) {
    return {
      icon: 'notification_important',
      text: 'Submission not completed',
      label: 'Continue Site Submission',
      route: () => this.viewSite(organizationId, site)
    };
  }

  public getRenewalRequiredSiteNotificationProperties(organizationId: number, site: SiteListViewModel) {
    return {
      icon: 'notification_important',
      text: 'This site requires renewal including this year\'s business licence.',
      label: 'Renew Site',
      route: () => this.viewSite(organizationId, site)
    };
  }

  public inComplete(site: SiteListViewModel): boolean {
    return !site.submittedDate || (site.submittedDate && !site.approvedDate && site.status === SiteStatusType.EDITABLE);
  }

  public isInReview(site: SiteListViewModel): boolean {
    return site.submittedDate && site.status === SiteStatusType.IN_REVIEW;
  }

  public getInReviewSiteNotificationProperties(site: SiteListViewModel) {
    const andSiteId = (!site.pec) ? ' and an assigned Site ID' : '';
    return {
      icon: 'notification_important',
      text: `This site is waiting for approval${andSiteId}`,
    };
  }

  public isPendingTransfer(): boolean {
    return this.organization?.pendingTransfer;
  }

  public isLocked(site: SiteListViewModel): boolean {
    return site.status === SiteStatusType.LOCKED;
  }

  public getLockedSiteNotificationProperties() {
    return {
      icon: 'not_interested',
      text: 'Declined',
    };
  }

  public isApproved(site: SiteListViewModel): boolean {
    return (site.status === SiteStatusType.EDITABLE && !!site.approvedDate);
  }

  public requiresRenewal(site: SiteListViewModel): boolean {
    return DateUtils.withinRenewalPeriod(Site.getExpiryDate(site));
  }

  public getApprovedSiteNotificationProperties(site: SiteListViewModel) {
    return {
      icon: 'task_alt',
      text: `Site Approved<br>Site ID: ${site.pec}`
    };
  }

  public routeToOrgAgreementByCareSettingCode(code: CareSettingEnum): void {
    this.routeUtils.routeRelativeTo([this.organization.id, SiteRoutes.CARE_SETTINGS, code, SiteRoutes.ORGANIZATION_AGREEMENT]);
  }

  public ngOnInit(): void {
    this.getOrganizations();
  }

  private getOrganizations(): void {
    this.busy = this.authService.getUser$()
      .pipe(
        exhaustMap((user: BcscUser) =>
          this.organizationResource.getSigningAuthorityOrganizationByUserId(user.userId)
        ),
        map((organization: Organization) => {
          this.organizationSitesExpiryDates = organization.sites
            .reduce((expiryDates: string[], site: Site) => {
              return (site.status === SiteStatusType.EDITABLE && !!site.approvedDate)
                ? [...expiryDates, Site.getExpiryDate(site)]
                : expiryDates;
            }, []);
          return this.organization = organization;
        }),
        exhaustMap((organization: Organization) =>
          this.organizationResource.getOrganizationAgreements(organization.id)
        ),
        exhaustMap((agreements: OrganizationAgreementViewModel[]) => {
          this.organizationAgreements = agreements;
          return (this.organization.pendingTransfer)
            ? this.organizationResource.getCareSettingCodesForPendingTransfer(this.organization.id)
            : of([]);
        }),
      )
      .subscribe((careSettingCodes: CareSettingEnum[]) =>
        this.careSettingCodesPendingTransfer = careSettingCodes
      );
  }

  private createSite(organizationId: number): void {
    this.busy = this.siteResource.createSite(organizationId)
      .subscribe((site: Site) =>
        this.routeUtils.routeRelativeTo([organizationId, SiteRoutes.SITES, site.id, SiteRoutes.CARE_SETTING])
      );
  }
}
