import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay()
    );

  isAuthenticated$ = this.oidcSecurityService.isAuthenticated$;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private oidcSecurityService: OidcSecurityService
  ) {}

  login(): void {
    this.oidcSecurityService.authorize();
  }
}
