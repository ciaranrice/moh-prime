import { Component, OnInit, Inject, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { APP_CONFIG, AppConfig } from 'app/app-config.module';
import { ToastService } from '@core/services/toast.service';
import { LoggerService } from '@core/services/logger.service';
import { Enrolment } from '@shared/models/enrolment.model';
import { EnrolmentCertificateAccessToken } from '@shared/models/enrolment-certificate-access-token.model';
import { EnrolmentResource } from '@enrolment/shared/services/enrolment-resource.service';
import { EnrolmentService } from '@enrolment/shared/services/enrolment.service';
import { BaseEnrolmentPage } from '@enrolment/shared/classes/BaseEnrolmentPage';
import { WindowRefService } from '@core/services/window-ref.service';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SummaryComponent extends BaseEnrolmentPage implements OnInit {
  public enrolment: Enrolment;
  public tokens: EnrolmentCertificateAccessToken[];
  public showProgressBar: boolean;

  constructor(
    protected route: ActivatedRoute,
    protected router: Router,
    @Inject(APP_CONFIG) private config: AppConfig,
    private enrolmentResource: EnrolmentResource,
    private enrolmentService: EnrolmentService,
    private toastService: ToastService,
    private logger: LoggerService,
    private windowRef: WindowRefService
  ) {
    super(route, router);

    this.showProgressBar = false;
  }

  public get enrollee() {
    return (this.enrolment) ? this.enrolment.enrollee : null;
  }

  public get mailingAddress() {
    return (this.enrollee) ? this.enrollee.mailingAddress : null;
  }

  public getTokenUrl(tokenId: string): string {
    return `${this.config.loginRedirectUrl}/enrolment-certificate/${tokenId}`;
  }

  public generateProvisionerLink() {
    this.enrolmentResource.createEnrolmentCertificateAccessToken()
      .subscribe((token) => this.tokens.push(token));
  }

  public ngOnInit() {
    // Only shown the first time the enrollee reaches the summary
    const routeState = this.windowRef.nativeWindow.history.state;
    this.showProgressBar = (routeState && routeState.showProgressBar) ? routeState.showProgressBar : false;

    this.enrolment = this.enrolmentService.enrolment;
    this.hasInitialStatus = this.enrolment.initialStatus;

    this.busy = this.enrolmentResource.enrolmentCertificateAccessTokens()
      .subscribe(
        (tokens: EnrolmentCertificateAccessToken[]) => this.tokens = tokens,
        (error: any) => {
          this.toastService.openErrorToast('Access tokens could be found.');
          this.logger.error('[EnrolmentCertificate] Summary::ngOnInit error has occurred: ', error);
        });
  }
}
