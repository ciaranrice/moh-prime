import { NgModule } from '@angular/core';

import { NgxProgressModule } from '@lib/modules/ngx-progress/ngx-progress.module';

import { SharedModule } from '@shared/shared.module';

import { EnrolmentRoutingModule } from './enrolment-routing.module';

import { CollectionNoticeComponent } from './pages/collection-notice/collection-notice.component';
import { DemographicComponent } from './pages/demographic/demographic.component';
import { RegulatoryComponent } from './pages/regulatory/regulatory.component';
import { DeviceProviderComponent } from './pages/device-provider/device-provider.component';
import { JobComponent } from './pages/job/job.component';
import { SelfDeclarationComponent } from './pages/self-declaration/self-declaration.component';
import { OrganizationComponent } from './pages/organization/organization.component';
import { OverviewComponent } from './pages/overview/overview.component';
import { MinorUpdateConfirmationComponent } from './pages/minor-update-confirmation/minor-update-confirmation.component';
import { SubmissionConfirmationComponent } from './pages/submission-confirmation/submission-confirmation.component';
import { AccessAgreementComponent } from './pages/access-agreement/access-agreement.component';
import { PharmanetEnrolmentSummaryComponent } from './pages/pharmanet-enrolment-summary/pharmanet-enrolment-summary.component';
import { AccessLockedComponent } from './pages/access-locked/access-locked.component';
import { AccessAgreementHistoryComponent } from './pages/access-agreement-history/access-agreement-history.component';
import { AccessTermsComponent } from './pages/access-terms/access-terms.component';
import { AccessAgreementCurrentComponent } from './pages/access-agreement-current/access-agreement-current.component';
import {
  AccessAgreementHistoryEnrolmentComponent
} from './pages/access-agreement-history-enrolment/access-agreement-history-enrolment.component';

import { JobFormComponent } from './shared/components/job-form/job-form.component';
import { EnrolleePageComponent } from './shared/components/enrollee-page/enrollee-page.component';
import { NextStepsInfographicComponent } from './shared/components/next-steps-infographic/next-steps-infographic.component';
import { EnrolmentCollectionNoticeComponent } from './shared/components/enrolment-collection-notice/enrolment-collection-notice.component';
import { DashboardV1Component } from './shared/components/dashboard/dashboardv1.component';
import { HeaderComponent } from './shared/components/header/header.component';
import {
  EnrolmentProgressIndicatorComponent
} from './shared/components/enrolment-progress-indicator/enrolment-progress-indicator.component';
import { AccessDeclinedComponent } from './pages/access-declined/access-declined.component';
import { NotificationConfirmationComponent } from './pages/notification-confirmation/notification-confirmation.component';
import { ProgressIndicatorComponent } from './shared/components/progress-indicator/progress-indicator.component';

@NgModule({
  declarations: [
    DashboardV1Component,
    HeaderComponent,
    CollectionNoticeComponent,
    DemographicComponent,
    RegulatoryComponent,
    DeviceProviderComponent,
    JobComponent,
    SelfDeclarationComponent,
    OrganizationComponent,
    OverviewComponent,
    MinorUpdateConfirmationComponent,
    SubmissionConfirmationComponent,
    AccessAgreementComponent,
    AccessLockedComponent,
    AccessAgreementHistoryComponent,
    PharmanetEnrolmentSummaryComponent,
    AccessTermsComponent,
    JobFormComponent,
    AccessAgreementCurrentComponent,
    AccessAgreementHistoryEnrolmentComponent,
    EnrolleePageComponent,
    NextStepsInfographicComponent,
    EnrolmentCollectionNoticeComponent,
    EnrolmentProgressIndicatorComponent,
    // TODO drop this component and reimplement using ProgressIndicator2Component
    ProgressIndicatorComponent,
    AccessDeclinedComponent,
    NotificationConfirmationComponent
  ],
  imports: [
    SharedModule,
    EnrolmentRoutingModule,
    NgxProgressModule
  ]
})
export class EnrolmentModule { }
