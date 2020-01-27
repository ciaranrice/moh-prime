// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  version: '1.0.0',
  apiEndpoint: 'http://localhost:5000/api',
  loginRedirectUrl: 'http://localhost:4200',
  prime: {
    displayPhone: '1-844-39PRIME',
    phone: '18443977463',
    email: 'prime@gov.bc.ca',
  },
  keycloakConfig: {
    config: {
      url: 'https://sso-dev.pathfinder.gov.bc.ca/auth',
      realm: 'v4mbqqas',
      clientId: 'prime-application-local'
    },
    initOptions: {
      onLoad: 'check-sso'
    },
    bearerExcludedUrls: ['/enrolment-certificates/certificate']
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
