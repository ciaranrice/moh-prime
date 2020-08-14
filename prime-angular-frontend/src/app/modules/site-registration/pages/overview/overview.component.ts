import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';

import { Subscription, EMPTY } from 'rxjs';
import { exhaustMap } from 'rxjs/operators';

import { SiteResource } from '@core/resources/site-resource.service';
import { DialogOptions } from '@shared/components/dialogs/dialog-options.model';
import { ConfirmDialogComponent } from '@shared/components/dialogs/confirm-dialog/confirm-dialog.component';

import { SiteService } from '@registration/shared/services/site.service';
import { RouteUtils } from '@registration/shared/classes/route-utils.class';
import { SiteRoutes } from '@registration/site-registration.routes';
import { Site } from '@registration/shared/models/site.model';
import { Organization } from '@registration/shared/models/organization.model';
import { OrganizationService } from '@registration/shared/services/organization.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {
  public busy: Subscription;
  public site: Site;
  public organization: Organization;

  public routeUtils: RouteUtils;
  public SiteRoutes = SiteRoutes;

  public submitted: boolean;

  constructor(
    protected route: ActivatedRoute,
    protected router: Router,
    private siteResource: SiteResource,
    private dialog: MatDialog,
    private siteService: SiteService,
    private organizationService: OrganizationService,
  ) {
    this.routeUtils = (this.isOrganizationReview)
      ? new RouteUtils(route, router, SiteRoutes.routePath(SiteRoutes.ORGANIZATION_REVIEW))
      : new RouteUtils(route, router, SiteRoutes.routePath(SiteRoutes.SITE_REVIEW));
  }

  public onSubmit(): void {
    const payload = this.siteService.site;
    const data: DialogOptions = {
      title: 'Save Site',
      message: 'When your site is saved it will be submitted for review. Are you ready to save your site?',
      actionText: 'Save Site'
    };
    this.busy = this.dialog.open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result: boolean) =>
          (result)
            ? this.siteResource.submitSite(payload)
            : EMPTY
        )
      )
      .subscribe(() => this.nextRoute());
  }

  public get isSiteReview(): boolean {
    return this.route.snapshot.routeConfig.path === SiteRoutes.SITE_REVIEW;
  }

  public get isOrganizationReview(): boolean {
    return this.route.snapshot.routeConfig.path === SiteRoutes.ORGANIZATION_REVIEW;
  }

  public onRoute(routePath: string): void {
    this.routeUtils.routeRelativeTo(routePath);
  }

  public onBack(): void {
    this.routeUtils.routeTo([SiteRoutes.MODULE_PATH, SiteRoutes.SITE_MANAGEMENT]);
  }

  public nextRoute(): void {
    this.routeUtils.routeTo([SiteRoutes.MODULE_PATH, SiteRoutes.SITE_MANAGEMENT], {
      queryParams: { submitted: true }
    });
  }

  public ngOnInit(): void {
    if (this.isOrganizationReview) {
      this.organization = this.organizationService.organization;
      // Never show the submission section for an organization
      this.submitted = true;
    } else {
      this.site = this.siteService.site;
      this.submitted = !!this.site.submittedDate;
      if (!this.submitted && this.organizationService.organization.siteCount === 1) {
        this.organization = this.organizationService.organization;
      }
    }
  }
}
