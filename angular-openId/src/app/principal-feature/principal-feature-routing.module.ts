import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PrincipalFeatureComponent } from './principal-feature.component';

const principalFeatureRoutes = [
  { path: '', component: PrincipalFeatureComponent },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(principalFeatureRoutes)],
})
export class PrincipalFeatureRoutingModule {}
