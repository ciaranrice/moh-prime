import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';

import { MockEnrolmentService } from 'test/mocks/mock-enrolment.service';

import { AccessAgreementComponent } from './access-agreement.component';
import { APP_CONFIG, APP_DI_CONFIG } from 'app/app-config.module';
import { KeycloakService } from 'keycloak-angular';
import { NgxBusyModule } from '@shared/modules/ngx-busy/ngx-busy.module';
import { NgxContextualHelpModule } from '@shared/modules/ngx-contextual-help/ngx-contextual-help.module';
import { NgxMaterialModule } from '@shared/modules/ngx-material/ngx-material.module';
import { AlertComponent } from '@shared/components/alert/alert.component';
import { PageComponent } from '@shared/components/page/page.component';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { PageSubheaderComponent } from '@shared/components/page-subheader/page-subheader.component';
import { ProgressIndicatorComponent } from '@shared/components/progress-indicator/progress-indicator.component';
import { PrimeContactComponent } from '@shared/components/prime-contact/prime-contact.component';
import { PrimePhoneComponent } from '@shared/components/prime-phone/prime-phone.component';
import { PrimeEmailComponent } from '@shared/components/prime-email/prime-email.component';
import { EnrolmentService } from '@enrolment/shared/services/enrolment.service';
import { CollectionNoticeAlertComponent } from '@enrolment/shared/components/collection-notice-alert/collection-notice-alert.component';
import { GlobalClauseComponent } from './components/global-clause/global-clause.component';
import { UserClauseComponent } from './components/user-clause/user-clause.component';
import { LicenceClassClauseComponent } from './components/licence-class-clause/licence-class-clause.component';
import { LimitsAndConditionsClauseComponent } from './components/limits-and-conditions-clause/limits-and-conditions-clause.component';
import { AccessTermsPagerComponent } from './components/access-terms-pager/access-terms-pager.component';
import { RuAccessTermsComponent } from './components/ru-access-terms/ru-access-terms.component';
import { OboAccessTermsComponent } from './components/obo-access-terms/obo-access-terms.component';

describe('AccessAgreementComponent', () => {
  let component: AccessAgreementComponent;
  let fixture: ComponentFixture<AccessAgreementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule(
      {
        imports: [
          HttpClientTestingModule,
          NgxBusyModule,
          NgxContextualHelpModule,
          NgxMaterialModule,
          ReactiveFormsModule,
          RouterTestingModule
        ],
        declarations: [
          AlertComponent,
          AccessAgreementComponent,
          PageComponent,
          PageHeaderComponent,
          PageSubheaderComponent,
          ProgressIndicatorComponent,
          PrimeContactComponent,
          PrimePhoneComponent,
          PrimeEmailComponent,
          GlobalClauseComponent,
          UserClauseComponent,
          LicenceClassClauseComponent,
          LimitsAndConditionsClauseComponent,
          AccessTermsPagerComponent,
          CollectionNoticeAlertComponent,
          RuAccessTermsComponent,
          OboAccessTermsComponent
        ],
        providers: [
          {
            provide: APP_CONFIG,
            useValue: APP_DI_CONFIG
          },
          {
            provide: EnrolmentService,
            useClass: MockEnrolmentService
          },
          KeycloakService
        ]
      }
    ).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessAgreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
