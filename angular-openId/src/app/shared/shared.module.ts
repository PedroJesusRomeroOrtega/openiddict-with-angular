import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoAuthComponent } from './no-auth/no-auth.component';
import { NavigationComponent } from './navigation/navigation.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { HasRoleDirective } from './has-role.directive';

const materialModules = [
  MatToolbarModule,
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule,
  MatTooltipModule,
];

@NgModule({
  declarations: [NoAuthComponent, NavigationComponent, HasRoleDirective],
  imports: [RouterModule, CommonModule, ...materialModules],
  exports: [NavigationComponent, HasRoleDirective, ...materialModules],
})
export class SharedModule {}
