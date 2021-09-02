import { NgModule } from '@angular/core';
import { PrincipalFeatureComponent } from './principal-feature.component';
import { PrincipalFeatureRoutingModule } from './principal-feature-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [PrincipalFeatureComponent],
  imports: [PrincipalFeatureRoutingModule, SharedModule],
})
export class PrincipalFeatureModule {}
