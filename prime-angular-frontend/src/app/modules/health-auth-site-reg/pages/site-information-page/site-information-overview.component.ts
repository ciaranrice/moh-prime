import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AbstractOverview } from '@lib/classes/abstract-overview.class';
import { HealthAuthSiteRegRoutes } from '@health-auth/health-auth-site-reg.routes';

import { SiteInformationForm } from './site-information-form.model';

@Component({
  selector: 'app-site-information-overview',
  template: `
    <app-overview-section title="Site Details"
                          [showEditRedirect]="showEditRedirect"
                          [editRoute]="HealthAuthSiteRegRoutes.SITE_INFORMATION"
                          (route)="onRoute($event)">
      <app-enrollee-property title="Site Name">
        {{ siteInformation?.siteName | default }}
      </app-enrollee-property>
      <app-enrollee-property title="Site ID/PEC Code">
        {{ siteInformation?.pec | default }}
      </app-enrollee-property>
      <app-enrollee-property title="Security Group">
        {{ siteInformation?.securityGroupCode | configCode: 'securityGroups' | default }}
      </app-enrollee-property>
    </app-overview-section>
  `,
  styles: ['mat-icon { font-size: 1.2em; }'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SiteInformationOverviewComponent extends AbstractOverview {
  @Input() public siteInformation: SiteInformationForm;
  public HealthAuthSiteRegRoutes = HealthAuthSiteRegRoutes;

  constructor(
    route: ActivatedRoute,
    router: Router
  ) {
    super(route, router, HealthAuthSiteRegRoutes.MODULE_PATH);
  }
}
