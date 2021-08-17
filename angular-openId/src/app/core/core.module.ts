import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { AuthConfig, OAuthModule } from 'angular-oauth2-oidc';
import { authConfig } from './auth-config';

@NgModule({
  declarations: [],
  imports: [HttpClientModule, OAuthModule.forRoot()],
  // providers: [{ provide: AuthConfig, useValue: authConfig }],
})
export class CoreModule {}
