import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { exhaustMap } from 'rxjs/operators';
import { EMPTY } from 'rxjs';

import { APP_CONFIG, AppConfig } from 'app/app-config.module';
import { FormControlValidators } from '@lib/validators/form-control.validators';
import { ToastService } from '@core/services/toast.service';
import { ConfirmDialogComponent } from '@shared/components/dialogs/confirm-dialog/confirm-dialog.component';
import { DialogOptions } from '@shared/components/dialogs/dialog-options.model';
import { AgreementTypeGroup } from '@shared/enums/agreement-type-group.enum';
import { Enrolment } from '@shared/models/enrolment.model';
import { EnrolmentResource } from '@enrolment/shared/services/enrolment-resource.service';
import { EnrolmentService } from '@enrolment/shared/services/enrolment.service';
import { BaseEnrolmentPage } from '@enrolment/shared/classes/enrolment-page.class';
import { CareSettingEnum } from '@shared/enums/care-setting.enum';
import { EnrolmentStatusEnum } from '@shared/enums/enrolment-status.enum';
import { ImageComponent } from '@shared/components/dialogs/content/image/image.component';
import { Role } from '@auth/shared/enum/role.enum';

@Component({
  selector: 'app-pharmanet-enrolment-summary',
  templateUrl: './pharmanet-enrolment-summary.component.html',
  styleUrls: ['./pharmanet-enrolment-summary.component.scss']
})
export class PharmanetEnrolmentSummaryComponent extends BaseEnrolmentPage implements OnInit {
  public form: FormGroup;
  public enrolment: Enrolment;

  public CareSettingEnum = CareSettingEnum;
  public EnrolmentStatus = EnrolmentStatusEnum;
  public Role = Role;

  public showCommunityHealth: boolean;
  public showPharmacist: boolean;
  public showHealthAuthority: boolean;
  public showDeviceProvider: boolean;
  public currentAgreementGroup: AgreementTypeGroup;

  public initialEnrolment: boolean;
  public complete: boolean;

  public careSettingConfigs: {
    setting: string,
    settingPlural: string,
    settingCode: number,
    formControl: FormControl,
    subheaderContent: string;
  }[];

  constructor(
    protected route: ActivatedRoute,
    protected router: Router,
    @Inject(APP_CONFIG) private config: AppConfig,
    private fb: FormBuilder,
    private enrolmentResource: EnrolmentResource,
    private enrolmentService: EnrolmentService,
    private dialog: MatDialog,
    private toastService: ToastService
  ) {
    super(route, router);
    this.showCommunityHealth = true;
    this.showPharmacist = true;
    this.showHealthAuthority = true;
    this.showDeviceProvider = true;

    this.form = this.buildVendorEmailGroup();
    this.careSettingConfigs = [];
  }

  public get enrollee() {
    return (this.enrolment) ? this.enrolment.enrollee : null;
  }

  public get mailingAddress() {
    return (this.enrollee) ? this.enrollee.mailingAddress : null;
  }

  public get careSettings() {
    return (this.enrolment) ? this.enrolment.careSettings : null;
  }

  public get enrolmentCertificateNote() {
    return (this.enrolment.enrolmentCertificateNote)
      ? this.enrolment.enrolmentCertificateNote.note
      : null;
  }

  public get communityHealthEmails(): FormControl {
    return this.form.get('communityHealthEmails') as FormControl;
  }

  public get pharmacistEmails(): FormControl {
    return this.form.get('pharmacistEmails') as FormControl;
  }

  public get healthAuthorityEmails(): FormControl {
    return this.form.get('healthAuthorityEmails') as FormControl;
  }

  public get deviceProviderEmails(): FormControl {
    return this.form.get('deviceProviderEmails') as FormControl;
  }

  public get GPID(): string {
    return this.enrollee.gpid;
  }

  public onCopy() {
    this.toastService.openSuccessToast('Your GPID has been copied to clipboard');
  }

  public setShowEmail(careSettingCode: number, show: boolean, formControl: FormControl = null) {
    if (formControl) {
      formControl.reset();
    }
    switch (careSettingCode) {
      case this.CareSettingEnum.PRIVATE_COMMUNITY_HEALTH_PRACTICE: {
        this.showCommunityHealth = show;
        break;
      }
      case this.CareSettingEnum.COMMUNITY_PHARMACIST: {
        this.showPharmacist = show;
        break;
      }
      case this.CareSettingEnum.HEALTH_AUTHORITY: {
        this.showHealthAuthority = show;
        break;
      }
      case this.CareSettingEnum.DEVICE_PROVIDER: {
        this.showDeviceProvider = show;
        break;
      }
    }
  }

  public isEmailVisible(careSettingCode: number): boolean {
    switch (careSettingCode) {
      case this.CareSettingEnum.PRIVATE_COMMUNITY_HEALTH_PRACTICE: {
        return this.showCommunityHealth;
      }
      case this.CareSettingEnum.COMMUNITY_PHARMACIST: {
        return this.showPharmacist;
      }
      case this.CareSettingEnum.HEALTH_AUTHORITY: {
        return this.showHealthAuthority;
      }
      case this.CareSettingEnum.DEVICE_PROVIDER: {
        return this.showDeviceProvider;
      }
      default: {
        return false;
      }
    }
  }

  public getTokenUrl(tokenId: string): string {
    return `${this.config.loginRedirectUrl}/provisioner-access/${tokenId}`;
  }

  public sendProvisionerAccessLinkTo(careSettingCode: number) {
    let formControl: FormControl;

    switch (careSettingCode) {
      case this.CareSettingEnum.PRIVATE_COMMUNITY_HEALTH_PRACTICE: {
        formControl = this.communityHealthEmails;
        break;
      }
      case this.CareSettingEnum.COMMUNITY_PHARMACIST: {
        formControl = this.pharmacistEmails;
        break;
      }
      case this.CareSettingEnum.HEALTH_AUTHORITY: {
        formControl = this.healthAuthorityEmails;
        break;
      }
      case this.CareSettingEnum.DEVICE_PROVIDER: {
        formControl = this.deviceProviderEmails;
        break;
      }
    }

    if (!formControl) { return; }

    const emails = formControl.value?.split(',').map((email: string) => email.trim()).join(',') || null;

    (formControl.valid)
      ? this.sendProvisionerAccessLink(emails, formControl, careSettingCode)
      : formControl.markAllAsTouched();
  }

  public sendProvisionerAccessLink(emails: string = null, formControl: FormControl = null, careSettingCode) {
    const data: DialogOptions = {
      title: 'Confirm Email',
      message: `Are you sure you want to send your Approval Notification?`,
      actionText: 'Send',
    };
    this.busy = this.dialog.open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result: boolean) => {
          if (result) {
            this.complete = true;
            return this.enrolmentResource.sendProvisionerAccessLink(emails, this.enrolment.id, careSettingCode);
          } else {
            return EMPTY;
          }
        })
      )
      .subscribe(() => {
        this.toastService.openSuccessToast('Email was successfully sent');
        this.setShowEmail(careSettingCode, false);
      });
  }

  public openQR(event: Event): void {
    event.preventDefault();

    this.enrolmentResource.getQrCode(this.enrolment.id)
      .subscribe((qrCode: string) => {
        var data = qrCode
          ? { base64Image: qrCode }
          : null;
        var message = qrCode
          ? 'Scan this QR code to receive an invitation to your verifiable credential that can be stored in your digital wallet.'
          : 'No credential invitation found.';

        const options: DialogOptions = {
          title: 'Verified Credential',
          message,
          actionHide: true,
          cancelText: 'Close',
          data,
          component: ImageComponent
        };

        this.busy = this.dialog.open(ConfirmDialogComponent, { data: options })
          .afterClosed()
          .subscribe();
      });
  }

  public getTitle() {
    if (!this.initialEnrolment) {
      return 'Share my Global PharmaNet ID (GPID)';
    } else if (this.complete) {
      return 'PRIME Enrolment Complete';
    }
    return 'Next Steps to Get PharmaNet';
  }

  public getAgreementDescription() {
    switch (this.currentAgreementGroup) {
      case AgreementTypeGroup.ON_BEHALF_OF:
        return 'You are an on behalf of user';
      case AgreementTypeGroup.REGULATED_USER:
        return 'You are an independant user';
      default:
        return '';
    }
  }

  public ngOnInit(): void {
    this.enrolment = this.enrolmentService.enrolment;
    this.isInitialEnrolment = this.enrolmentService.isInitialEnrolment;
    this.initialEnrolment = this.route.snapshot.queryParams?.initialEnrolment === 'true';

    this.enrolmentResource.getCurrentAgreementGroupForAnEnrollee(this.enrolment.id)
      .subscribe((group: AgreementTypeGroup) => this.currentAgreementGroup = group)

    this.careSettingConfigs = this.careSettings.map(careSetting => {
      switch (careSetting.careSettingCode) {
        case CareSettingEnum.PRIVATE_COMMUNITY_HEALTH_PRACTICE: {
          return {
            setting: 'Private Community Health Practice',
            settingPlural: 'Private Community Health Practices',
            settingCode: careSetting.careSettingCode,
            formControl: this.communityHealthEmails,
            subheaderContent: `Enter the email of the PharmaNet administrator in your workplace. If you work at more than one workplace, include the email addresses for each PharmaNet administrator, separated by a comma.`
          };
        }
        case CareSettingEnum.COMMUNITY_PHARMACIST: {
          return {
            setting: 'Community Pharmacy',
            settingPlural: 'Community Pharmacies',
            settingCode: careSetting.careSettingCode,
            formControl: this.pharmacistEmails,
            subheaderContent: `Enter the email of the PharmaNet administrator in your workplace. If you work at more than one workplace, include the email addresses for each PharmaNet administrator, separated by a comma.`
          };
        }
        case CareSettingEnum.HEALTH_AUTHORITY: {
          return {
            setting: 'Health Authority',
            settingPlural: 'Health Authorities',
            settingCode: careSetting.careSettingCode,
            formControl: this.healthAuthorityEmails,
            subheaderContent: `Enter the email of the PharmaNet administrator in your workplace. If you work at more than one workplace, include the email addresses for each PharmaNet administrator, separated by a comma.`
          };
        }
        case CareSettingEnum.DEVICE_PROVIDER: {
          return {
            setting: 'Device Provider',
            settingPlural: 'Device Providers',
            settingCode: careSetting.careSettingCode,
            formControl: this.deviceProviderEmails,
            subheaderContent: `Enter the email of the PharmaNet administrator in your workplace. If you work at more than one workplace, include the email addresses for each PharmaNet administrator, separated by a comma.`
          };
        }
      }
    });
  }

  private buildVendorEmailGroup(): FormGroup {
    return this.fb.group({
      communityHealthEmails: [null, [Validators.required, FormControlValidators.multipleEmails]],
      pharmacistEmails: [null, [Validators.required, FormControlValidators.multipleEmails]],
      healthAuthorityEmails: [null, [Validators.required, FormControlValidators.multipleEmails]],
      deviceProviderEmails: [null, [Validators.required, FormControlValidators.multipleEmails]],
    });
  }
}
