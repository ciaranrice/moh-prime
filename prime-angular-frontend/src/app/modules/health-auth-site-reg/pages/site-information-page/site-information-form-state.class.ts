import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';

import { asyncValidator } from '@lib/validators/form-async.validators';
import { AbstractFormState } from '@lib/classes/abstract-form-state.class';
import { SiteResource } from '@core/resources/site-resource.service';
import { SiteInformationForm } from './site-information-form.model';

export class SiteInformationFormState extends AbstractFormState<SiteInformationForm> {
  private siteId: number;

  public constructor(
    private fb: FormBuilder,
    private siteResource: SiteResource
  ) {
    super();

    this.buildForm();
  }

  public get siteName(): FormControl {
    return this.formInstance.get('siteName') as FormControl;
  }

  public get pec(): FormControl {
    return this.formInstance.get('pec') as FormControl;
  }

  public get securityGroupCode(): FormControl {
    return this.formInstance.get('securityGroupCode') as FormControl;
  }

  public get json(): SiteInformationForm {
    if (!this.formInstance) {
      return;
    }

    return this.formInstance.getRawValue();
  }

  public patchValue(model: SiteInformationForm, siteId: number): void {
    if (!this.formInstance) {
      return;
    }

    this.siteId = siteId;
    this.formInstance.patchValue(model);
  }

  public buildForm(): void {
    this.formInstance = this.fb.group({
      siteName: ['', [Validators.required]],
      pec: [null, []],
      securityGroupCode: [null, [Validators.required]]
    });
  }
}
