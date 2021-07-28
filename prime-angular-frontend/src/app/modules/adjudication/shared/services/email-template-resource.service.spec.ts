import { TestBed } from '@angular/core/testing';
import { APP_CONFIG, APP_DI_CONFIG } from 'app/app-config.module';

import { EmailTemplateResourceService } from './email-template-resource.service';

describe('EmailTemplateResourceService', () => {
  let service: EmailTemplateResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG
        },
      ]
    });
    service = TestBed.inject(EmailTemplateResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
