import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { OptionalFeatureComponent } from './optional-feature.component';

const optionalFeatureRoutes = [
  { path: '', component: OptionalFeatureComponent },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(optionalFeatureRoutes)],
})
export class OptionalFeatureRoutingModule {}
