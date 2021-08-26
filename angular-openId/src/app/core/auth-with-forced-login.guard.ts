import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanLoad,
  Route,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthWithForcedLoginGuard implements CanActivate, CanLoad {
  constructor(private authService: AuthService) {}

  canLoad(
    route: Route,
    segments: UrlSegment[]
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const url = segments.reduce(
      (path, currentSegment) => `${path}/${currentSegment.path}`,
      ''
    );

    return this.manageAuthentication(url);
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.manageAuthentication(state.url);
  }

  private manageAuthentication(url: string): Observable<boolean> {
    return this.authService.canActivateProtectedRoutes$.pipe(
      tap((canActivate) => canActivate || this.authService.login(url))
    );
  }
}
