import { Component } from '@angular/core';
import { Role } from '../core/role';

@Component({
  selector: 'app-principal-feature',
  templateUrl: './principal-feature.component.html',
  styleUrls: ['./principal-feature.component.scss'],
})
export class PrincipalFeatureComponent {
  public get Role() {
    return Role;
  }
}
