import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelper, tokenNotExpired } from 'angular2-jwt';
import { LoginKullanici } from '../models/loginKullanici';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router, private alertifyService: AlertifyService) {}
  private path = 'https://localhost:44343/api/users/';
  private TOKEN_KEY = 'token';
  private jwtHelper: JwtHelper = new JwtHelper();

  login(loginKullanici: LoginKullanici) {
    this.http.post(this.path + 'login', loginKullanici).subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      } else {
        localStorage.setItem(this.TOKEN_KEY, data['data']);
        this.alertifyService.success(data['message']);
        this.router.navigateByUrl('/tasinmazlar');
      }
    });
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigateByUrl('/login');
    this.alertifyService.success('Çıkış başarılı.');
  }

  loggedIn() {
    return tokenNotExpired(this.TOKEN_KEY);
  }

  get userRole() {
    return this.jwtHelper.decodeToken(localStorage.getItem(this.TOKEN_KEY))["role"]
  }

  get kullaniciId() {
    return Number(this.jwtHelper.decodeToken(localStorage.getItem(this.TOKEN_KEY))["nameid"])
  }
}
