import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthWithForcedLoginGuard } from './core/auth-with-forced-login.guard';
import { AuthGuard } from './core/auth.guard';
import { HomeComponent } from './home/home.component';
import { NoAuthComponent } from './shared/no-auth/no-auth.component';

const appRoutes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'principal',
    loadChildren: () =>
      import('./principal-feature/principal-feature.module').then(
        (m) => m.PrincipalFeatureModule
      ),
    canLoad: [AuthWithForcedLoginGuard],
  },
  {
    path: 'optional',
    loadChildren: () =>
      import('./optional-feature/optional-feature.module').then(
        (m) => m.OptionalFeatureModule
      ),
    canLoad: [AuthGuard],
  },
  { path: 'notauth', component: NoAuthComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes, {
      initialNavigation: 'enabledNonBlocking',
      enableTracing: false, // <-- debugging purposes only
      relativeLinkResolution: 'corrected',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
