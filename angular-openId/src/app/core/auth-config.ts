import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

export const authConfig: AuthConfig = {
  issuer: 'https://localhost:5001/',
  redirectUri: window.location.origin,
  clientId: 'angularClient',
  responseType: 'code',
  scope: 'openid profile email offline_access', // Ask offline_access to support refresh token refreshes
  showDebugInformation: true,
  useSilentRefresh: true,
  silentRefreshTimeout: 5000, // For faster testing
  timeoutFactor: 0.25, // For faster testing
  // sessionChecksEnabled: true,
  silentRefreshRedirectUri: `${window.location.origin}/silent-refresh.html`,
};
