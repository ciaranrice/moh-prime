import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

import { EmailUtils } from '@lib/utils/email-utils.class';
import { UtilsService } from '@core/services/utils.service';
import { Role } from '@auth/shared/enum/role.enum';
import { PermissionService } from '@auth/shared/services/permission.service';
import { SiteActionViewModel } from '@registration/shared/models/site-registration.model';
import { SiteStatusType } from '@registration/shared/enum/site-status.enum';
import { SiteAdjudicationAction } from '@registration/shared/enum/site-adjudication-action.enum';

@Component({
  selector: 'app-site-registration-actions',
  templateUrl: './site-registration-actions.component.html',
  styleUrls: ['./site-registration-actions.component.scss']
})
export class SiteRegistrationActionsComponent implements OnInit {
  @Input() siteRegistration: SiteActionViewModel;
  @Output() public approve: EventEmitter<{ siteId: number, organizationId: number }>;
  @Output() public decline: EventEmitter<{ siteId: number, organizationId: number }>;
  @Output() public unreject: EventEmitter<{ siteId: number, organizationId: number }>;
  @Output() public escalate: EventEmitter<{ siteId: number, organizationId: number }>;
  @Output() public delete: EventEmitter<{ [key: string]: number }>;
  @Output() public enableEditing: EventEmitter<{ siteId: number, organizationId: number }>;
  @Output() public flag: EventEmitter<{ siteId: number, flagged: boolean }>;

  public Role = Role;
  public SiteStatusType = SiteStatusType;
  public SiteAdjudicationAction = SiteAdjudicationAction;

  constructor(
    private permissionService: PermissionService,
    private utilsService: UtilsService
  ) {
    this.delete = new EventEmitter<{ [key: string]: number }>();
    this.approve = new EventEmitter<{ siteId: number, organizationId: number }>();
    this.decline = new EventEmitter<{ siteId: number, organizationId: number }>();
    this.unreject = new EventEmitter<{ siteId: number, organizationId: number }>();
    this.escalate = new EventEmitter<{ siteId: number, organizationId: number }>();
    this.enableEditing = new EventEmitter<{ siteId: number, organizationId: number }>();
    this.flag = new EventEmitter<{ siteId: number, flagged: boolean }>();
  }

  public onApprove(): void {
    if (this.permissionService.hasRoles(Role.EDIT_SITE)) {
      this.approve.emit({ siteId: this.siteRegistration.siteId, organizationId: this.siteRegistration.organizationId });
    }
  }

  public onReject(): void {
    if (this.permissionService.hasRoles(Role.EDIT_SITE)) {
      this.decline.emit({ siteId: this.siteRegistration.siteId, organizationId: this.siteRegistration.organizationId });
    }
  }

  public onUnreject(): void {
    if (this.permissionService.hasRoles(Role.EDIT_SITE)) {
      this.unreject.emit({ siteId: this.siteRegistration.siteId, organizationId: this.siteRegistration.organizationId });
    }
  }

  public onEscalate(): void {
    if (this.permissionService.hasRoles(Role.EDIT_SITE)) {
      this.escalate.emit({ siteId: this.siteRegistration.siteId, organizationId: this.siteRegistration.organizationId });
    }
  }

  public onContactSigningAuthority() {
    const signingAuthority = this.siteRegistration?.signingAuthority;
    if (signingAuthority) {
      EmailUtils.openEmailClient(
        signingAuthority.email,
        `PRIME Site Registration - ${this.siteRegistration.name}`,
        `Dear ${signingAuthority.firstName} ${signingAuthority.lastName},`
      );
    }
  }

  public onToggleFlagSite() {
    if (this.permissionService.hasRoles(Role.VIEW_SITE)) {
      this.flag.emit({
        siteId: this.siteRegistration.siteId,
        flagged: !this.siteRegistration.flagged
      });
    }
  }

  public onDelete(record: { [key: string]: number }) {
    this.delete.emit(record);
  }

  public onRequestChanges(): void {
    if (this.permissionService.hasRoles(Role.EDIT_SITE)) {
      this.enableEditing.emit({ siteId: this.siteRegistration.siteId, organizationId: this.siteRegistration.organizationId });
    }
  }

  /**
   * @description
   * Check whether the given action is valid according to the status of the
   * site registration.
   */
  public isActionAllowed(action: SiteAdjudicationAction): boolean {
    switch (this.siteRegistration.status) {
      case SiteStatusType.EDITABLE:
        return (action === SiteAdjudicationAction.REJECT);
      case SiteStatusType.IN_REVIEW:
        return (action === SiteAdjudicationAction.REQUEST_CHANGES
          || action === SiteAdjudicationAction.APPROVE
          || action === SiteAdjudicationAction.REJECT);
      case SiteStatusType.LOCKED:
        return (action === SiteAdjudicationAction.UNREJECT);
      default:
        return false;
    }
  }

  public ngOnInit(): void {}
}
