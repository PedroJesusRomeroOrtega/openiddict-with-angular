import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

export const authConfig: AuthConfig = {
  issuer: environment.issuer,
  redirectUri: window.location.origin,
  clientId: environment.cliendId,
  responseType: 'code',
  scope: 'openid profile email offline_access', // Ask offline_access to support refresh token refreshes
  showDebugInformation: !environment.production,
  useSilentRefresh: true,
  silentRefreshTimeout: environment.production ? 20000 : 5000,
  timeoutFactor: environment.production ? 0.75 : 0.25,
  // sessionChecksEnabled: true,
  clearHashAfterLogin: false,
  silentRefreshRedirectUri: `${window.location.origin}/silent-refresh.html`,
};
