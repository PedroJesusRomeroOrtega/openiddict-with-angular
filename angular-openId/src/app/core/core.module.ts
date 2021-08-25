import { HttpClientModule } from '@angular/common/http';
import {
  ModuleWithProviders,
  NgModule,
  Optional,
  SkipSelf,
} from '@angular/core';
import {
  AuthConfig,
  OAuthModule,
  OAuthModuleConfig,
} from 'angular-oauth2-oidc';
import { authConfig } from './auth-config';
import { authModuleConfig } from './auth-module-config';

@NgModule({
  declarations: [],
  imports: [HttpClientModule, OAuthModule.forRoot()],
})
export class CoreModule {
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        { provide: AuthConfig, useValue: authConfig },
        { provide: OAuthModuleConfig, useValue: authModuleConfig },
      ],
    };
  }
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule is already loaded. Import it in the AppModule only'
      );
    }
  }
}
