import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

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
  },
  {
    path: 'optional',
    loadChildren: () =>
      import('./optional-feature/optional-feature.module').then(
        (m) => m.OptionalFeatureModule
      ),
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes, {
      initialNavigation: 'enabled',
      enableTracing: false, // <-- debugging purposes only
      relativeLinkResolution: 'corrected',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
