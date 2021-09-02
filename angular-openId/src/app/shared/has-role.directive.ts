import {
  Directive,
  Input,
  OnDestroy,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../core/auth.service';
import { Role } from '../core/role';

@Directive({
  selector: '[hasRole]',
})
export class HasRoleDirective implements OnDestroy {
  private hasView = false;
  private destroyed$ = new Subject();

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  @Input() set hasRole(role: Role) {
    this.authService.isUserProfileLoaded$
      .pipe(takeUntil(this.destroyed$))
      .subscribe((_) => this.manageTemplate(role));
  }

  private manageTemplate(role: Role) {
    const hasRole = this.authService.hasRole(role);
    if (!this.hasView && hasRole) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (this.hasView && !hasRole) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }
}
