import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { AuthService } from '../core/auth.service';

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

  state$: Observable<{ isAuthenticated: boolean }> = this.authService.state$;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService
  ) {}

  login() {
    this.authService.login('/home');
  }

  logout() {
    this.authService.logout();
  }

  get name(): string {
    return this.authService.identityClaims
      ? (this.authService.identityClaims as any)['name']
      : '-';
  }
}
