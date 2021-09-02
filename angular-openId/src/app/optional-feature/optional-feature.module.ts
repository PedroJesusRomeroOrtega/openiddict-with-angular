import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { OptionalFeatureRoutingModule } from './optional-feature-routing.module';
import { OptionalFeatureComponent } from './optional-feature.component';

@NgModule({
  declarations: [OptionalFeatureComponent],
  imports: [OptionalFeatureRoutingModule, SharedModule, CommonModule],
})
export class OptionalFeatureModule {}
