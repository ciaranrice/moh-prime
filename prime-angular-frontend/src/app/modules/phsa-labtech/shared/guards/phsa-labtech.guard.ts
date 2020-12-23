import { Inject } from '@angular/core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { Observable, of } from 'rxjs';

import { RouteUtils } from '@lib/utils/route-utils.class';
import { BaseGuard } from '@core/guards/base.guard';
import { LoggerService } from '@core/services/logger.service';
import { AuthService } from '@auth/shared/services/auth.service';
import { AppConfig, APP_CONFIG } from 'app/app-config.module';
import { PhsaLabtechRoutes } from '@phsa/phsa-labtech.routes';

@Injectable({
  providedIn: 'root'
})
export class PhsaLabtechGuard extends BaseGuard {

  public constructor(
    protected authService: AuthService,
    protected logger: LoggerService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router
  ) {
    super(authService, logger);
  }

  protected checkAccess(routePath: string = null): Observable<boolean> | Promise<boolean> {
    // currentRoutePath is "" if user types URL into address bar
    const currentRoutePath = RouteUtils.currentRoutePath(this.router.url);
    const nextRoutePath = RouteUtils.currentRoutePath(routePath);

    if (
      currentRoutePath === PhsaLabtechRoutes.DEMOGRAPHIC &&
      nextRoutePath === PhsaLabtechRoutes.AVAILABLE_ACCESS
    ) {
      return of(this.navigate(routePath, PhsaLabtechRoutes.AVAILABLE_ACCESS));
    } else if (
      currentRoutePath === PhsaLabtechRoutes.AVAILABLE_ACCESS &&
      nextRoutePath === PhsaLabtechRoutes.SUBMISSION_CONFIRMATION
    ) {
      return of(this.navigate(routePath, PhsaLabtechRoutes.SUBMISSION_CONFIRMATION));
    } else {
      // Otherwise, start at the beginning of the enrolment process
      return of(this.navigate(routePath, PhsaLabtechRoutes.DEMOGRAPHIC));
    }
  }

  /**
   * @description
   * Similar to code from EnrolmentGuard.navigate
   */
  private navigate(routePath: string, destinationPath: string): boolean {
    const phsaRoutePath = this.config.routes.phsa;

    if (routePath === `/${phsaRoutePath}/${destinationPath}`) {
      return true;
    } else {
      this.router.navigate([phsaRoutePath, destinationPath]);
      return false;
    }
  }
}
