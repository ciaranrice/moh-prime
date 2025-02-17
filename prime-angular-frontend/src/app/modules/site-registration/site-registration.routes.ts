export class SiteRoutes {
  public static LOGIN_PAGE = 'site';

  public static MODULE_PATH = 'site-registration';

  public static COMMUNITY_SITE_DEFAULT_WORKFLOW = '';
  public static CHANGE_SIGNING_AUTHORITY_WORKFLOW = 'change-signing-authority';

  public static COLLECTION_NOTICE = 'collection-notice';
  public static ORGANIZATIONS = 'organizations';

  public static ORGANIZATION_SIGNING_AUTHORITY = 'organization-signing-authority';
  public static ORGANIZATION_CLAIM = 'organization-claim';
  public static ORGANIZATION_CLAIM_CONFIRMATION = 'organization-claim-confirmation';
  public static ORGANIZATION_CLAIMED = 'organization-claimed';
  public static ORGANIZATION_NAME = 'organization-name';
  public static ORGANIZATION_REVIEW = 'organization-review';
  public static ORGANIZATION_AGREEMENT = 'organization-agreement';

  public static SITES = 'sites';
  public static CARE_SETTINGS = 'care-settings';

  public static CARE_SETTING = 'care-setting';
  public static BUSINESS_LICENCE = 'business-licence';
  public static SITE_ADDRESS = 'site-address';
  public static HOURS_OPERATION = 'hours-operation';
  public static DEVICE_PROVIDER = 'device-provider';
  public static ADMINISTRATOR = 'site-administrator';
  public static PRIVACY_OFFICER = 'privacy-officer';
  public static TECHNICAL_SUPPORT = 'technical-support';
  public static REMOTE_USERS = 'remote-users';
  public static REMOTE_USER = 'remote-user';
  public static SITE_REVIEW = 'site-review';
  public static NEXT_STEPS = 'next-steps';

  public static BUSINESS_LICENCE_RENEWAL = 'business-licence-renewal';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${SiteRoutes.MODULE_PATH}/${route}`;
  }

  // Used to indicate the routes and order of registration for an initial
  // organization, and a subsequent associated site
  public static initialRegistrationRouteOrder(): string[] {
    return [
      ...this.organizationRegistrationRouteOrder(),
      ...this.siteRegistrationRouteOrder(),
      // Only occurs once when creating the organization
      this.ORGANIZATION_AGREEMENT
    ];
  }

  // Used to indicate the routes for updating
  // organization, and sites
  public static editRegistrationRouteAccess(): string[] {
    return [
      ...this.organizationRegistrationRouteOrder(),
      ...this.editSiteRegistrationRouteOrder(),
    ];
  }

  // Used to indicate the routes and order of registration for organizations
  public static organizationRegistrationRouteOrder(): string[] {
    return [
      this.COLLECTION_NOTICE,
      this.ORGANIZATION_SIGNING_AUTHORITY,
      this.ORGANIZATION_NAME
    ];
  }

  // Used to indicate the routes and order of registration for sites
  public static siteRegistrationRouteOrder(): string[] {
    return [
      this.CARE_SETTING,
      this.BUSINESS_LICENCE,
      this.SITE_ADDRESS,
      this.HOURS_OPERATION,
      this.DEVICE_PROVIDER,
      this.REMOTE_USERS,
      this.ADMINISTRATOR,
      this.PRIVACY_OFFICER,
      this.TECHNICAL_SUPPORT,
      this.NEXT_STEPS
    ];
  }

  // Used to indicate the routes and order for sites for editing
  public static editSiteRegistrationRouteOrder(): string[] {
    return [
      this.CARE_SETTING,
      this.BUSINESS_LICENCE,
      this.SITE_ADDRESS,
      this.HOURS_OPERATION,
      this.REMOTE_USERS,
      this.REMOTE_USER, // Included since it's a child of remote_users
      this.ADMINISTRATOR,
      this.PRIVACY_OFFICER,
      this.TECHNICAL_SUPPORT,
      this.SITE_REVIEW,
      this.NEXT_STEPS
    ];
  }

  public static siteRegistrationRoutes(): string[] {
    return [
      ...this.organizationRegistrationRouteOrder(),
      ...this.siteRegistrationRouteOrder()
    ];
  }

  public static claimOrganizationRoutes(): string[] {
    return [
      this.COLLECTION_NOTICE,
      this.ORGANIZATION_SIGNING_AUTHORITY,
      this.ORGANIZATION_CLAIM,
      this.ORGANIZATION_CLAIM_CONFIRMATION
    ];
  }
}
