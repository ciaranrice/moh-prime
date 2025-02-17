import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { APP_CONFIG, APP_DI_CONFIG } from 'app/app-config.module';
import { MockPermissionService } from 'test/mocks/mock-permission.service';

import { NgxMaterialModule } from '@lib/modules/ngx-material/ngx-material.module';

import { PermissionService } from '@auth/shared/services/permission.service';
import { CapitalizePipe } from '@shared/pipes/capitalize.pipe';

import { SiteOverviewPageComponent } from './site-overview-page.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

describe('SiteOverviewPageComponent', () => {
  let component: SiteOverviewPageComponent;
  let fixture: ComponentFixture<SiteOverviewPageComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        NgxMaterialModule,
        ReactiveFormsModule,
        BrowserAnimationsModule
      ],
      declarations: [SiteOverviewPageComponent],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG
        },
        {
          provide: PermissionService,
          useClass: MockPermissionService
        },
        CapitalizePipe
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SiteOverviewPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
