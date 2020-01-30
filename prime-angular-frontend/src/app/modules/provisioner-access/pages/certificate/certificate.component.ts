import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { EnrolmentCertificate } from '../../shared/models/enrolment-certificate.model';
import moment from 'moment';
import { ProvisionerAccessResource } from '../../shared/services/provisioner-access-resource.service';

@Component({
  selector: 'app-certificate',
  templateUrl: './certificate.component.html',
  styleUrls: ['./certificate.component.scss']
})
export class CertificateComponent implements OnInit {
  public busy: Subscription;
  public certificate: EnrolmentCertificate;
  public expiryDate: string;

  constructor(
    private route: ActivatedRoute,
    private enrolmentCertificateResource: ProvisionerAccessResource,
  ) { }

  public ngOnInit() {
    this.busy = this.enrolmentCertificateResource
      .getCertificate(this.route.snapshot.params.tokenId)
      .subscribe((certificate: EnrolmentCertificate) => {
        this.certificate = certificate;
        this.expiryDate = moment(this.certificate.expiryDate).isAfter(moment.now())
          ? moment(this.certificate.expiryDate).format('MMMM Do, YYYY') : null;
      });
  }
}