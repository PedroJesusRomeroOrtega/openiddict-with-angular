import { Component, OnInit } from '@angular/core';
import { Role } from '../core/role';

@Component({
  selector: 'app-principal-feature',
  templateUrl: './principal-feature.component.html',
  styleUrls: ['./principal-feature.component.scss'],
})
export class PrincipalFeatureComponent implements OnInit {
  public get Role() {
    return Role;
  }

  constructor() {}

  ngOnInit(): void {}
}
