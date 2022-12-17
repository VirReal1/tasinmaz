import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let adminMi = this.authService.adminMi;
    if (adminMi) {
      return true;
    }
    this.router.navigateByUrl('tasinmazlar'); //If the user is not logged in, this will navigate the user to login page if he presses any admin site
    return false;
  }
}
