import { AdjudicationRoutes } from '@adjudication/adjudication.routes';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

import { ArrayUtils } from '@lib/utils/array-utils.class';
import { SiteStatusType } from '@lib/enums/site-status.enum';
import { HealthAuthorityEnum } from '@lib/enums/health-authority.enum';
import { HealthAuthorityResource } from '@core/resources/health-authority-resource.service';
import { HealthAuthorityRow } from '@shared/models/health-authority-row.model';
import { Role } from '@auth/shared/enum/role.enum';
import { HealthAuthoritySiteList } from '@health-auth/shared/models/health-authority-site-list.model';

@Component({
  selector: 'app-health-authority-table',
  templateUrl: './health-authority-table.component.html',
  styleUrls: ['./health-authority-table.component.scss']
})
export class HealthAuthorityTableComponent implements OnInit {
  @Output() public assign: EventEmitter<number>;
  @Output() public reassign: EventEmitter<number>;
  @Output() public notify: EventEmitter<{ siteId: number, healthAuthorityOrganizationId: HealthAuthorityEnum }>;
  @Output() public reload: EventEmitter<number>;
  @Output() public route: EventEmitter<string | (string | number)[]>;

  public siteColumns: string[];
  public dataSource: MatTableDataSource<HealthAuthorityRow | HealthAuthoritySiteList>;
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
      'siteActions'
    ];
    this.assign = new EventEmitter<number>();
    this.reassign = new EventEmitter<number>();
    this.notify = new EventEmitter<{ siteId: number, healthAuthorityOrganizationId: HealthAuthorityEnum }>();
    this.reload = new EventEmitter<number>();
    this.route = new EventEmitter<string | (string | number)[]>();
    this.dataSource = new MatTableDataSource<HealthAuthorityRow | HealthAuthoritySiteList>([]);
    this.expandedHealthAuthId = 0;
  }

  public onAssign(siteId: number): void {
    this.assign.emit(siteId);
  }

  public onReassign(siteId: number): void {
    this.reassign.emit(siteId);
  }

  public onNotify(siteId: number, healthAuthorityOrganizationId: HealthAuthorityEnum): void {
    this.notify.emit({ siteId, healthAuthorityOrganizationId });
  }

  public onReload(siteId: number): void {
    this.reload.emit(siteId);
  }

  public onRoute(routePath: string | (string | number)[]) {
    this.route.emit(routePath);
  }

  public isHealthAuthority(row: HealthAuthorityRow | HealthAuthoritySiteList): boolean {
    return row.hasOwnProperty('hasUnderReviewUsers');
  }

  public isGroup(): (index: number, row: HealthAuthorityRow | HealthAuthoritySiteList) => boolean {
    return (index: number, row: HealthAuthorityRow | HealthAuthoritySiteList): boolean =>
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
      healthAuthoritySiteLists: this.healthAuthorityResource.getAllHealthAuthoritySiteLists()
    }).pipe(
      map(({ healthAuthorities, healthAuthoritySiteLists }) => {
        this.flaggedHealthAuthorities = healthAuthorities.reduce((fhas: number[], ha: HealthAuthorityRow) =>
          [...fhas, ...ArrayUtils.insertIf(ha.hasUnderReviewUsers, ha.id)], []
        );
        // Group sites under their associated health authorities
        this.dataSource.data = [...healthAuthorities, ...healthAuthoritySiteLists].sort(this.sortData());
      })
    ).subscribe();
  }

  /**
   * @description
   * Sort health authorities and their grouped sites in ascending order by ID.
   */
  private sortData(): (a: HealthAuthorityRow | HealthAuthoritySiteList, b: HealthAuthorityRow | HealthAuthoritySiteList) => number {
    return (a: HealthAuthorityRow | HealthAuthoritySiteList, b: HealthAuthorityRow | HealthAuthoritySiteList): number => {
      if (this.isHealthAuthority(a) && this.isHealthAuthority(b)) {
        return a.id - b.id;
      } else if (this.isHealthAuthority(a)) {
        return (a.id !== (b as HealthAuthoritySiteList).healthAuthorityOrganizationId)
          ? a.id - (b as HealthAuthoritySiteList).healthAuthorityOrganizationId
          : -1;
      } else if (this.isHealthAuthority(b)) {
        return (b.id !== (a as HealthAuthoritySiteList).healthAuthorityOrganizationId)
          ? (a as HealthAuthoritySiteList).healthAuthorityOrganizationId - b.id
          : 1;
      } else {
        return (a as HealthAuthoritySiteList).healthAuthorityOrganizationId - (b as HealthAuthoritySiteList).healthAuthorityOrganizationId;
      }
    };
  }
}
