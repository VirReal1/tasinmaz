import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router){}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      let logged = this.authService.loggedIn();
      if (logged) {
        return true;
      }
      this.router.navigate(['login']); //If the user is not logged in, this will navigate the user to login page if he presses any admin site
      return false;
  }
}