import { AdjudicationRoutes } from '@adjudication/adjudication.routes';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

import { ArrayUtils } from '@lib/utils/array-utils.class';
import { HealthAuthorityResource } from '@core/resources/health-authority-resource.service';
import { HealthAuthorityEnum } from '@shared/enums/health-authority.enum';
import { HealthAuthorityRow } from '@shared/models/health-authority-row.model';
import { Role } from '@auth/shared/enum/role.enum';
import { HealthAuthoritySite } from '@health-auth/shared/models/health-authority-site.model';
import { SiteStatusType } from '@registration/shared/enum/site-status.enum';

@Component({
  selector: 'app-health-authority-table',
  templateUrl: './health-authority-table.component.html',
  styleUrls: ['./health-authority-table.component.scss']
})
export class HealthAuthorityTableComponent implements OnInit {
  @Output() public route: EventEmitter<string | (string | number)[]>;

  public siteColumns: string[];
  public dataSource: MatTableDataSource<HealthAuthorityRow | HealthAuthoritySite>;
  public flaggedHealthAuthorities: HealthAuthorityEnum[];
  public Role = Role;
  public AdjudicationRoutes = AdjudicationRoutes;
  public SiteStatusType = SiteStatusType;
  public expandedHealthAuthId: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private healthAuthorityResource: HealthAuthorityResource
  ) {
    this.siteColumns = [
      'prefixes',
      'orgName',
      'siteName',
      'submissionDate',
      'assignedTo',
      'state',
      'siteId',
      'remoteUsers',
      'siteActions'
    ];
    this.route = new EventEmitter<string | (string | number)[]>();
    this.dataSource = new MatTableDataSource<HealthAuthorityRow | HealthAuthoritySite>([]);
    this.expandedHealthAuthId = 0;
  }

  public onRoute(routePath: string | (string | number)[]) {
    this.route.emit(routePath);
  }

  public isHealthAuthority(row: HealthAuthorityRow | HealthAuthoritySite): boolean {
    return row.hasOwnProperty('hasUnderReviewUsers');
  }

  public isGroup(): (index: number, row: HealthAuthorityRow | HealthAuthoritySite) => boolean {
    return (index: number, row: HealthAuthorityRow | HealthAuthoritySite): boolean =>
      this.isHealthAuthority(row);
  }

  public onExpandHeader(item: HealthAuthorityRow): void {
    this.expandedHealthAuthId = (this.expandedHealthAuthId !== item.id)
      ? item.id
      : 0;
  }

  public ngOnInit(): void {
    forkJoin({
      healthAuthorities: this.healthAuthorityResource.getHealthAuthorities(),
      healthAuthoritySites: this.healthAuthorityResource.getAllHealthAuthoritySites()
    }).pipe(
      map(({ healthAuthorities, healthAuthoritySites }) => {
        this.flaggedHealthAuthorities = healthAuthorities.reduce((fhas: number[], ha: HealthAuthorityRow) =>
          [...fhas, ...ArrayUtils.insertIf(ha.hasUnderReviewUsers, ha.id)], []
        );
        // Group sites under their associated health authorities
        this.dataSource.data = [...healthAuthorities, ...healthAuthoritySites].sort(this.sortData());
      })
    ).subscribe();
  }

  /**
   * @description
   * Sort health authorities and their grouped sites in ascending order by ID.
   */
  private sortData(): (a: HealthAuthorityRow | HealthAuthoritySite, b: HealthAuthorityRow | HealthAuthoritySite) => number {
    return (a: HealthAuthorityRow | HealthAuthoritySite, b: HealthAuthorityRow | HealthAuthoritySite): number => {
      if (this.isHealthAuthority(a) && this.isHealthAuthority(b)) {
        return a.id - b.id;
      } else if (this.isHealthAuthority(a)) {
        return (a.id !== (b as HealthAuthoritySite).healthAuthorityOrganizationId)
          ? a.id - (b as HealthAuthoritySite).healthAuthorityOrganizationId
          : -1;
      } else if (this.isHealthAuthority(b)) {
        return (b.id !== (a as HealthAuthoritySite).healthAuthorityOrganizationId)
          ? (a as HealthAuthoritySite).healthAuthorityOrganizationId - b.id
          : 1;
      } else {
        return (a as HealthAuthoritySite).healthAuthorityOrganizationId - (b as HealthAuthoritySite).healthAuthorityOrganizationId;
      }
    };
  }
}
