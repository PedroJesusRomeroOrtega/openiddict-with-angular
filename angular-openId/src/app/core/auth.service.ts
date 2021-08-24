import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { authConfig } from './auth-config';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private stateSubject$ = new BehaviorSubject<{ isAuthenticated: boolean }>({
    isAuthenticated: false,
  });
  public state$ = this.stateSubject$.asObservable();

  constructor(private oauthService: OAuthService, private router: Router) {
    this.oauthService.events.subscribe((_) => {
      this.stateSubject$.next({
        isAuthenticated: this.oauthService.hasValidAccessToken(),
      });
    });
    // this.oauthService.setupAutomaticSilentRefresh();
  }

  public runInitialLoginSequence(): Promise<void> {
    return this.oauthService
      .loadDiscoveryDocument()
      .then(() => this.oauthService.tryLogin())
      .then(() => {
        if (this.oauthService.hasValidAccessToken()) {
          return Promise.resolve();
        }
        return this.oauthService.silentRefresh().then(() => Promise.resolve());
      });
  }

  public login(targetUrl?: string) {
    this.oauthService.configure(authConfig);
    this.oauthService.loadDiscoveryDocumentAndLogin();
    this.oauthService.events
      .pipe(filter((e) => e.type === 'token_received'))
      .subscribe((_) => this.oauthService.loadUserProfile());
    // this.oauthService.initLoginFlow(targetUrl || this.router.url);
  }

  public logout() {
    this.oauthService.logOut();
    // this.oauthService.revokeTokenAndLogout();
  }

  public get identityClaims() {
    return this.oauthService.getIdentityClaims();
  }
}
