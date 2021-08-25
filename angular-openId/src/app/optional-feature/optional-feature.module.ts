import { NgModule } from '@angular/core';
import { OptionalFeatureRoutingModule } from './optional-feature-routing.module';
import { OptionalFeatureComponent } from './optional-feature.component';

@NgModule({
  declarations: [OptionalFeatureComponent],
  imports: [OptionalFeatureRoutingModule],
})
export class OptionalFeatureModule {}
