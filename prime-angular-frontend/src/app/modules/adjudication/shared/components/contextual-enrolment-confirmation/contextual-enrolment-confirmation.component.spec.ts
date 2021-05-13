import { AdjudicationModule } from '@adjudication/adjudication.module';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthService } from '@auth/shared/services/auth.service';
import { PermissionService } from '@auth/shared/services/permission.service';
import { ConfigService } from '@config/config.service';
import { NgxMaterialModule } from '@lib/modules/ngx-material/ngx-material.module';
import { SharedModule } from '@shared/shared.module';
import { APP_CONFIG, APP_DI_CONFIG } from 'app/app-config.module';
import { KeycloakService } from 'keycloak-angular';
import { MockAuthService } from 'test/mocks/mock-auth.service';
import { MockConfigService } from 'test/mocks/mock-config.service';
import { MockPermissionService } from 'test/mocks/mock-permission.service';

import { ContextualEnrolmentConfirmationComponent } from './contextual-enrolment-confirmation.component';

describe('ContextualEnrolmentConfirmationComponent', () => {
  let component: ContextualEnrolmentConfirmationComponent;
  let fixture: ComponentFixture<ContextualEnrolmentConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        NgxMaterialModule,
        SharedModule,
        AdjudicationModule
      ],
      declarations: [ContextualEnrolmentConfirmationComponent],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG
        },
        {
          provide: KeycloakService
        },
        {
          provide: ConfigService,
          useValue: MockConfigService
        },
        {
          provide: AuthService,
          useClass: MockAuthService
        },
        {
          provide: PermissionService,
          useClass: MockPermissionService
        }
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContextualEnrolmentConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});